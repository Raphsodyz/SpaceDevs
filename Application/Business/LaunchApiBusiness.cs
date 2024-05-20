using AutoMapper;
using Business.Interface;
using Domain.Entities;
using Cross.Cutting.Enum;
using Cross.Cutting.Helper;
using System.Linq.Expressions;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data.Common;
using System.Reflection;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Interface;
using Domain.Materializated.Views;
using Application.Interface;

namespace Application.Business
{
    public class LaunchApiBusiness : BusinessBase<Launch, ILaunchRepository>, ILaunchApiBusiness, IBusiness
    {
        private readonly IHttpClientFactory _client;
        private readonly IMapper _mapper;
        public LaunchApiBusiness(IUnitOfWork uow,
            IHttpClientFactory client,
            IMapper mapper):base(uow)
        {
            _client = client;
            _mapper = mapper;
        }

        public async Task<LaunchView> GetOneLaunch(Guid? launchId)
        {
            _ = launchId ?? throw new ArgumentNullException(ErrorMessages.NullArgument);
            
            ILaunchViewBusiness _launchViewBusiness = GetBusiness(typeof(ILaunchViewBusiness)) as ILaunchViewBusiness;
            Expression<Func<LaunchView, bool>> launchQuery = l => l.Id == launchId && l.EntityStatus == EStatus.PUBLISHED.GetDisplayName();
            
            if (!await _launchViewBusiness.ViewExists())
                throw new Exception(ErrorMessages.ViewNotExists);

            var launch = await _launchViewBusiness.GetById(filter: launchQuery) ?? throw new KeyNotFoundException(ErrorMessages.KeyNotFound);
            return launch;
        }

        public async Task<Pagination<LaunchView>> GetAllLaunchPaged(int? page)
        {
            ILaunchViewBusiness _launchViewBusiness = GetBusiness(typeof(ILaunchViewBusiness)) as ILaunchViewBusiness;
            List<Expression<Func<LaunchView, bool>>> publishedLaunchQuery = new(){ l => l.EntityStatus == EStatus.PUBLISHED.GetDisplayName() };
            
            var pagedResults = await _launchViewBusiness.GetViewPaged(
                page ?? 0, 10,
                filters: publishedLaunchQuery);

            if (!pagedResults.Entities.Any())
                throw new KeyNotFoundException(ErrorMessages.NoData);
            
            return pagedResults;
        }

        public async Task SoftDeleteLaunch(Guid? launchId)
        {
            _ = launchId ?? throw new ArgumentNullException(ErrorMessages.NullArgument);
            ILaunchBusiness _launchBusiness = GetBusiness(typeof(ILaunchBusiness)) as ILaunchBusiness;

            List<Expression<Func<Launch, bool>>> launchQuery = new()
            { l => l.Id == launchId && l.EntityStatus == EStatus.PUBLISHED.GetDisplayName() };
            var launchExists = await _launchBusiness.EntityExist(filter: launchQuery.FirstOrDefault());
            
            if(!launchExists)
                throw new KeyNotFoundException(ErrorMessages.KeyNotFound);

            using var trans = await _repository.GetTransaction();
            try
            {
                Expression<Func<Launch, Launch>> updateColumns = l => new Launch()
                { EntityStatus = EStatus.TRASH.GetDisplayName() };
                await _launchBusiness.UpdateOnQuery(launchQuery, updateColumns);

                await trans.CommitAsync();

                ILaunchViewBusiness _launchViewBusiness = GetBusiness(typeof(ILaunchViewBusiness)) as ILaunchViewBusiness;
                await _launchViewBusiness.RefreshView();
            }
            catch (Exception ex)
            {
                await trans.RollbackAsync();
                throw ex;
            }
            finally
            {
                await trans.DisposeAsync();
            }
        }

        public async Task<LaunchView> UpdateLaunch(Guid? launchId)
        {
            _ = launchId ?? throw new ArgumentNullException(ErrorMessages.NullArgument);
            ILaunchBusiness _launchBusiness = GetBusiness(typeof(ILaunchBusiness)) as ILaunchBusiness;

            Expression<Func<Launch, bool>> launchQuery = l => l.Id == launchId && l.EntityStatus == EStatus.PUBLISHED.GetDisplayName();
            var apiGuid = await _launchBusiness.GetSelected(
                filter: launchQuery,
                selectColumns: l => l.ApiGuid,
                buildObject: l => l);

            if(apiGuid == Guid.Empty)
                throw new KeyNotFoundException(ErrorMessages.KeyNotFound);

            var client = _client.CreateClient();
            try
            {
                string url = $"{EndPoints.TheSpaceDevsLaunchEndPoint}{apiGuid}";
                HttpResponseMessage response = await client.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                    throw new HttpRequestException($"{response.StatusCode} - {ErrorMessages.LaunchApiEndPointError}");
                
                var updatedLaunch = await response.Content.ReadFromJsonAsync<LaunchDTO>();
                if(ObjectHelper.IsObjectEmpty(updatedLaunch))
                    throw new JsonException(ErrorMessages.DeserializingContentError);

                var launch = _mapper.Map<Launch>(updatedLaunch);
                await SaveLaunch(launch, true);

                ILaunchViewBusiness _launchViewBusiness = GetBusiness(typeof(ILaunchViewBusiness)) as ILaunchViewBusiness;
                await _launchViewBusiness.RefreshView();

                var result = await _launchViewBusiness.GetById(l => l.Id == launchId && l.EntityStatus == EStatus.PUBLISHED.GetDisplayName());                
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> UpdateDataSet(UpdateLaunchRequest request)
        {
            request.Limit ??= 100;
            request.Iterations ??= 15;
            int offset = request.Skip ??= 0, entityCounter = 0, max = offset + ((int)request.Iterations * (int)request.Limit);
            
            for (int i = offset; i < max; i += (int)request.Limit)
            {
                var client = _client.CreateClient();
                try
                {
                    string url = $"{EndPoints.TheSpaceDevsLaunchEndPoint}?limit={request.Limit}&offset={offset}";
                    HttpResponseMessage response = await client.GetAsync(url);
                    if (!response.IsSuccessStatusCode)
                        throw new HttpRequestException($"{response.StatusCode} - {ErrorMessages.LaunchApiEndPointError}");

                    RequestLaunchDTO dataList = await response.Content.ReadFromJsonAsync<RequestLaunchDTO>() ?? throw new HttpRequestException(ErrorMessages.DeserializingContentError);
                    if ((bool)!dataList.Results?.Any())
                        throw new KeyNotFoundException(ErrorMessages.NoDataFromSpaceDevApi);

                    foreach(var data in dataList.Results)
                    {
                        var launch = _mapper.Map<Launch>(data);
                        await SaveLaunch(launch, request.ReplaceData ?? false);
                        entityCounter++;
                    }

                    await GenerateLog(offset, SuccessMessages.PartialImportSuccess, entityCounter, true);
                    entityCounter = 0;
                    offset += (int)request.Limit;
                }
                catch (Exception ex)
                {
                    await GenerateLog(offset, ex.Message, entityCounter, false);
                    throw ex;
                }
                finally
                {
                    await RefreshView();
                }
            }
            await GenerateLog(offset, SuccessMessages.ImportedDataSuccess, entityCounter, true);
            return true;
        }

        public async Task<Pagination<LaunchView>> SearchByParam(SearchLaunchRequest request)
        {
            List<Expression<Func<LaunchView, bool>>> query = new();
            if(!string.IsNullOrEmpty(request.Mission))
            {
                IMissionBusiness _missionBusiness = GetBusiness(typeof(IMissionBusiness)) as IMissionBusiness;

                var idsMission = await _missionBusiness.ILikeSearch(searchTerm: request.Mission.Trim(), selectColumns: m => m.Id);
                if(idsMission != null && idsMission.Any()) query.Add(l => idsMission.Contains((Guid)l.IdMission));
            }
                
            if(!string.IsNullOrWhiteSpace(request.Rocket))
            {            
                IConfigurationBusiness _configurationBusiness = GetBusiness(typeof(IConfigurationBusiness)) as IConfigurationBusiness;

                var idsRocket = await _configurationBusiness.ILikeSearch(searchTerm: request.Rocket.Trim(), selectColumns: r => r.Id);
                if(idsRocket != null && idsRocket.Any()) query.Add(l => idsRocket.Contains((Guid)l.Rocket.IdConfiguration));
            }
        
            if(!string.IsNullOrWhiteSpace(request.Location))
            {
                ILocationBusiness _locationBusiness = GetBusiness(typeof(ILocationBusiness)) as ILocationBusiness;

                var idsLocation = await _locationBusiness.ILikeSearch(searchTerm: request.Location.Trim(), selectColumns: l => l.Id);
                if(idsLocation != null && idsLocation.Any()) query.Add(l => idsLocation.Contains((Guid)l.Pad.IdLocation));
            }

            if(!string.IsNullOrWhiteSpace(request.Pad))
            {
                IPadBusiness _padBusiness = GetBusiness(typeof(IPadBusiness)) as IPadBusiness;

                var idsPad = await _padBusiness.ILikeSearch(searchTerm: request.Pad.Trim(), selectColumns: p => p.Id);
                if(idsPad != null && idsPad.Any()) query.Add(l => idsPad.Contains((Guid)l.IdPad));
            }

            if(!string.IsNullOrWhiteSpace(request.Launch))
            {
                ILaunchBusiness _launchBusiness = GetBusiness(typeof(ILaunchBusiness)) as ILaunchBusiness;

                var idsLaunch = await _launchBusiness.ILikeSearch(searchTerm: request.Launch.Trim(), selectColumns: l => l.Id);
                if(idsLaunch != null && idsLaunch.Any()) query.Add(l => idsLaunch.Contains(l.Id));
            }

            if(!query.Any())
                throw new KeyNotFoundException(ErrorMessages.KeyNotFound);

            ILaunchViewBusiness _launchViewBusiness = GetBusiness(typeof(ILaunchViewBusiness)) as ILaunchViewBusiness;
            var found = await _launchViewBusiness.GetViewPaged(request.Page ?? 0, 10, query);
            
            if(!found.Entities.Any())
                throw new KeyNotFoundException(ErrorMessages.KeyNotFound);

            return found;
        }

        private async Task SaveLaunch(Launch launch, bool replaceData)
        {
            if (ObjectHelper.IsObjectEmpty(launch))
                throw new ArgumentNullException(ErrorMessages.NullArgument);

            ILaunchBusiness _launchBusiness = GetBusiness(typeof(ILaunchBusiness)) as ILaunchBusiness;
            if(replaceData == false)
            {
                if(await _launchBusiness.EntityExist(l => l.ApiGuid == launch.ApiGuid))
                    return;
            }
            else
                await SetOriginalBaseEntityDataProcesses(launch);

            using var trans = await _repository.GetTransaction();
            try
            {
                var currentlyTransaction = _repository.GetCurrentlyTransaction();
                var efConnection = _repository.GetEfConnection();

                Guid? idStatus = await SaveLaunchEntitiesProcesses(new ForeignKeyManagerDTO<Status>(launch.Status, null, null), efConnection, currentlyTransaction, replaceData);
                Guid? idLaunchServiceProvider = await SaveLaunchEntitiesProcesses(new ForeignKeyManagerDTO<LaunchServiceProvider>(launch.LaunchServiceProvider, null, null), efConnection, currentlyTransaction,replaceData);
                Guid? idConfiguration = await SaveLaunchEntitiesProcesses(new ForeignKeyManagerDTO<Configuration>(launch.Rocket?.Configuration, null, null), efConnection, currentlyTransaction, replaceData);
                Guid? idRocket = await SaveLaunchEntitiesProcesses(new ForeignKeyManagerDTO<Rocket>(launch.Rocket, idConfiguration, LaunchNestedObjectsForeignKeys.ROCKET), efConnection, currentlyTransaction, replaceData);
                Guid? idOrbit = await SaveLaunchEntitiesProcesses(new ForeignKeyManagerDTO<Orbit>(launch.Mission?.Orbit, null, null), efConnection, currentlyTransaction, replaceData);
                Guid? idMission = await SaveLaunchEntitiesProcesses(new ForeignKeyManagerDTO<Mission>(launch.Mission, idOrbit, LaunchNestedObjectsForeignKeys.MISSION), efConnection, currentlyTransaction, replaceData);
                Guid? idLocation = await SaveLaunchEntitiesProcesses(new ForeignKeyManagerDTO<Location>(launch.Pad?.Location, null, null), efConnection, currentlyTransaction, replaceData);
                Guid? idPad = await SaveLaunchEntitiesProcesses(new ForeignKeyManagerDTO<Pad>(launch.Pad, idLocation, LaunchNestedObjectsForeignKeys.PAD), efConnection, currentlyTransaction, replaceData);
                await _launchBusiness.SaveOnUpdateLaunch(launch, idStatus, idLaunchServiceProvider, idRocket, idMission, idPad);
                
                await trans.CommitAsync();
            }
            catch (Exception ex)
            {
                await trans.RollbackAsync();
                throw ex;
            }
            finally
            {
                await trans.DisposeAsync();
            }
        }

        private async Task<Guid?> SaveLaunchEntitiesProcesses<T>(ForeignKeyManagerDTO<T> fkManager, DbConnection sharedConnection, IDbContextTransaction transaction, bool replaceData) where T : BaseEntity
        {
            if(ObjectHelper.IsObjectEmpty(fkManager.Entity)) return null;
            if(!string.IsNullOrWhiteSpace(fkManager.DesiredFk)) _uow.SetupForeignKey(fkManager.Entity, fkManager.DesiredFk, (Guid)fkManager.FkValue);
            if(replaceData == false) fkManager.Entity.Id = await DatabaseGuid(fkManager.Entity, sharedConnection, transaction);
            
            if(fkManager.Entity.Id == Guid.Empty)
                return await SaveNewLaunchEntity(fkManager.Entity, sharedConnection, transaction);

            if(replaceData == true)
                await UpdateExistingLaunch(fkManager.Entity, sharedConnection, transaction);

            return fkManager.Entity.Id;
        }

        private async Task<Guid> SaveNewLaunchEntity<T>(T entity, DbConnection sharedConnection, IDbContextTransaction transaction) where T : BaseEntity
        {
            var _dapper = _uow.Dapper();

            entity.Id = Guid.NewGuid();
            entity.ImportedT = DateTime.Now;
            entity.AtualizationDate = DateTime.Now;
            entity.EntityStatus = EStatus.PUBLISHED.GetDisplayName();

            await _dapper.Save(entity, sharedConnection, transaction);
            return entity.Id;
        }

        private async Task UpdateExistingLaunch<T>(T entity, DbConnection sharedConnection, IDbContextTransaction transaction) where T : BaseEntity
        {
            var _dapper = _uow.Dapper();
            await _dapper.FullUpdate(entity, "id_from_api = @IdFromApi", sharedConnection, transaction);
        }

        private async Task<Guid> DatabaseGuid<T>(T entity, DbConnection sharedConnection, IDbContextTransaction transaction) where T : BaseEntity
        {
            var _dapper = _uow.Dapper();
            return await _dapper.GetSelected<Guid>(
                query: $"SELECT Id FROM {typeof(T)?.GetCustomAttribute<TableAttribute>()?.Name} WHERE id_from_api = @IdFromApi",
                parameters: new { IdFromApi = entity.IdFromApi },
                sharedConnection: sharedConnection,
                transaction: transaction);
        }

        private async Task GenerateLog(int offset, string message, int entityCount, bool success)
        {
            IUpdateLogBusiness _updateLogBusiness = GetBusiness(typeof(IUpdateLogBusiness)) as IUpdateLogBusiness;
            
            var log = new UpdateLog()
            {
                TransactionDate = DateTime.Now,
                OffSet = offset,
                Success = success,
                Message = message,
                EntityCount = entityCount,
                Origin = EOrigin.API_UPDATE.GetDisplayName(),
                EntityStatus = EStatus.PUBLISHED.GetDisplayName()
            };
            await _updateLogBusiness.Save(log);
        }
    
        private async Task SetOriginalBaseEntityDataProcesses(Launch launch)
        {
            LaunchBaseEntityAggregate launchBaseEntityAggregate = new();
            await Task.WhenAll(
                SetOriginalBaseEntityData(launch.Status, launchBaseEntityAggregate),
                SetOriginalBaseEntityData(launch.LaunchServiceProvider, launchBaseEntityAggregate),
                SetOriginalBaseEntityData(launch.Rocket.Configuration, launchBaseEntityAggregate),
                SetOriginalBaseEntityData(launch.Rocket, launchBaseEntityAggregate),
                SetOriginalBaseEntityData(launch.Mission.Orbit, launchBaseEntityAggregate),
                SetOriginalBaseEntityData(launch.Mission, launchBaseEntityAggregate),
                SetOriginalBaseEntityData(launch.Pad.Location, launchBaseEntityAggregate),
                SetOriginalBaseEntityData(launch.Pad, launchBaseEntityAggregate),
                SetOriginalBaseEntityLaunchData(launch, launchBaseEntityAggregate)
            );
            _mapper.Map(launchBaseEntityAggregate, launch);
        }

        private async Task SetOriginalBaseEntityLaunchData(Launch launch, LaunchBaseEntityAggregate aggregateData)
        {
            ILaunchBusiness _launchBusiness = GetBusiness(typeof(ILaunchBusiness)) as ILaunchBusiness;

            Expression<Func<Launch, bool>> qryBaseDataLaunch = l => l.ApiGuid == launch.ApiGuid;
            var originalData = await _launchBusiness.GetSelected(
                filter: qryBaseDataLaunch,
                selectColumns: l => new { l.ApiGuid, l.Id, l.ImportedT, l.EntityStatus },
                buildObject: l => new BaseEntityLaunchDTO() 
                { 
                    ApiGuid = l.ApiGuid,
                    Id = l.Id,
                    ImportedT = l.ImportedT,
                    Status = l.EntityStatus,
                    AtualizationDate = DateTime.Now 
                }
            );

            PopulateBaseEntityAggregate("Launch", aggregateData, originalLaunchData: originalData);
        }

        private async Task SetOriginalBaseEntityData<T>(T entity, LaunchBaseEntityAggregate aggregateData) where T : BaseEntity
        {
            if(ObjectHelper.IsObjectEmpty(entity))
                return;

            var originalData = await RecoveryOriginalBaseEntity(entity);
            PopulateBaseEntityAggregate(typeof(T).Name, aggregateData, originalData);
        }

        private async Task<BaseEntityDTO> RecoveryOriginalBaseEntity<T>(T entity) where T : BaseEntity
        {
            var _dapper = _uow.Dapper();
            var result = await _dapper.GetSelected<BaseEntityDTO>(
                query: "SELECT id as Id, id_from_api as IdFromApi, imported_t as ImportedT, status as Status " +
                $"FROM {typeof(T)?.GetCustomAttribute<TableAttribute>()?.Name} WHERE id_from_api = @IdFromApi",
                parameters: new { IdFromApi = entity.IdFromApi });

            if(result != null)
                result.AtualizationDate = DateTime.Now;
            
            return result;
        }

        private void PopulateBaseEntityAggregate(string prop, LaunchBaseEntityAggregate launchBaseEntityAggregate, BaseEntityDTO? originalData = null, BaseEntityLaunchDTO originalLaunchData = null)
        {
            switch(prop)
            {
                case "Status": launchBaseEntityAggregate.StatusBaseEntity = originalData; break;
                case "LaunchServiceProvider": launchBaseEntityAggregate.LaunchServiceProviderBaseEntity = originalData; break;
                case "Rocket": launchBaseEntityAggregate.RocketBaseEntity = originalData; break;
                case "Configuration": launchBaseEntityAggregate.ConfigurationBaseEntity = originalData; break;
                case "Mission": launchBaseEntityAggregate.MissionBaseEntity = originalData; break;
                case "Orbit": launchBaseEntityAggregate.OrbitBaseEntity = originalData; break;
                case "Pad": launchBaseEntityAggregate.PadBaseEntity = originalData; break;
                case "Location": launchBaseEntityAggregate.LocationBaseEntity = originalData; break;
                case "Launch": launchBaseEntityAggregate.LaunchBaseEntity = originalLaunchData; break;
                default: throw new NotImplementedException();
            };
        }

        private async Task RefreshView()
        {
            ILaunchViewBusiness _launchViewBusiness = GetBusiness(typeof(ILaunchViewBusiness)) as ILaunchViewBusiness;
            await _launchViewBusiness.RefreshView();
        }
    }
}
