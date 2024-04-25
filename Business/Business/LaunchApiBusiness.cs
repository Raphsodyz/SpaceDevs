using AutoMapper;
using Business.Interface;
using Data.Interface;
using Domain.Entities;
using Cross.Cutting.Enum;
using Cross.Cutting.Helper;
using System.Linq.Expressions;
using System.Net.Http.Json;
using Data.Materializated.Views;
using Business.DTO.Entities;
using System.Text.Json;
using Business.DTO.Request;
using Microsoft.EntityFrameworkCore.Storage;

namespace Business.Business
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
            ILaunchViewBusiness _launchViewBusiness = GetBusiness(typeof(ILaunchViewBusiness)) as ILaunchViewBusiness;

            if (launchId == null)
                throw new ArgumentNullException(ErrorMessages.NullArgument);

            Expression<Func<LaunchView, bool>> launchQuery = l => l.Id == launchId && l.EntityStatus == EStatus.PUBLISHED.GetDisplayName();
            bool launchExist = await _launchViewBusiness.ViewExists();
            if (!launchExist)
                throw new Exception(ErrorMessages.ViewNotExists);

            var launch = await _launchViewBusiness.GetById(filter: launchQuery) ?? throw new KeyNotFoundException(ErrorMessages.KeyNotFound);
            return launch;
        }

        public async Task<Pagination<LaunchView>> GetAllLaunchPaged(int? page)
        {
            ILaunchViewBusiness _launchViewBusiness = GetBusiness(typeof(ILaunchViewBusiness)) as ILaunchViewBusiness;

            int totalEntities = await _launchViewBusiness.EntityCount(l => l.EntityStatus == EStatus.PUBLISHED.GetDisplayName());
            int totalPages = totalEntities % 10 == 0 ? totalEntities / 10 : (totalEntities / 10) + 1;
            if (page > totalPages || page < 0)
                throw new InvalidOperationException($"{ErrorMessages.InvalidPageSelected} Total pages = {totalPages}");

            List<Expression<Func<LaunchView, bool>>> publishedLaunchQuery = new()
            { l => l.EntityStatus == EStatus.PUBLISHED.GetDisplayName() };
            var selectedPageLaunchList = await _launchViewBusiness.GetViewPaged(
                page ?? 0, 10,
                filters: publishedLaunchQuery);

            if (selectedPageLaunchList.Entities?.Count == 0)
                throw new KeyNotFoundException(ErrorMessages.NoData);
            
            return selectedPageLaunchList;
        }

        public async Task SoftDeleteLaunch(Guid? launchId)
        {
            ILaunchBusiness _launchBusiness = GetBusiness(typeof(ILaunchBusiness)) as ILaunchBusiness;

            if (launchId == null)
                throw new ArgumentNullException(ErrorMessages.NullArgument);

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
                await _launchBusiness.UpdateOnQuery(launchQuery, updateColumns );

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
            ILaunchBusiness _launchBusiness = GetBusiness(typeof(ILaunchBusiness)) as ILaunchBusiness;

            if (launchId == null)
                throw new ArgumentNullException(ErrorMessages.NullArgument);

            Expression<Func<Launch, bool>> launchQuery = l => l.Id == launchId && l.EntityStatus == EStatus.PUBLISHED.GetDisplayName();
            var launch = await _launchBusiness.Get(filter: launchQuery) ?? throw new KeyNotFoundException(ErrorMessages.KeyNotFound);

            using var trans = await _repository.GetTransaction();
            var client = _client.CreateClient();
            try
            {
                string url = $"{EndPoints.TheSpaceDevsLaunchEndPoint}{launch.ApiGuid}";
                HttpResponseMessage response = await client.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                    throw new HttpRequestException($"{response.StatusCode} - {ErrorMessages.LaunchApiEndPointError}");
                
                var updatedLaunch = await response.Content.ReadFromJsonAsync<LaunchDTO>();
                if(ObjectHelper.IsObjectEmpty(updatedLaunch))
                    throw new JsonException(ErrorMessages.DeserializingContentError);

                launch = _mapper.Map<Launch>(updatedLaunch);
                launch.EntityStatus = EStatus.PUBLISHED.GetDisplayName();

                await trans.CommitAsync();

                ILaunchViewBusiness _launchViewBusiness = GetBusiness(typeof(ILaunchViewBusiness)) as ILaunchViewBusiness;
                await _launchViewBusiness.RefreshView();

                var result = await _launchViewBusiness.GetById(l => l.Id == launchId && l.EntityStatus == EStatus.PUBLISHED.GetDisplayName());                
                return result;
            }
            catch (HttpRequestException ex)
            {
                throw ex;
            }
            catch (JsonException ex)
            {
                throw ex;
            }
            catch (KeyNotFoundException ex)
            {
                throw ex;
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
                        await SaveLaunch(launch);
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

        private async Task SaveLaunch(Launch launch)
        {
            if (launch == null)
                throw new ArgumentNullException(ErrorMessages.NullArgument);

            ILaunchBusiness _launchBusiness = GetBusiness(typeof(ILaunchBusiness)) as ILaunchBusiness;
            if(await _launchBusiness.EntityExist(l => l.ApiGuid == launch.ApiGuid))
                return;

            using var trans = await _repository.GetTransaction();
            try
            {
                Guid? idStatus = await SaveLaunchEntitiesProcess(launch.Status, _repository.GetCurrentlyTransaction());
                Guid? idLaunchServiceProvider = await SaveLaunchEntitiesProcess(launch.LaunchServiceProvider, _repository.GetCurrentlyTransaction());
                Guid? idConfiguration = await SaveLaunchEntitiesProcess(launch.Rocket?.Configuration, _repository.GetCurrentlyTransaction());
                Guid? idRocket = await SaveLaunchEntitiesProcess(new Rocket(launch.Rocket?.IdFromApi, idConfiguration), _repository.GetCurrentlyTransaction());
                Guid? idOrbit = await SaveLaunchEntitiesProcess(launch.Mission?.Orbit, _repository.GetCurrentlyTransaction());
                Guid? idMission = await SaveLaunchEntitiesProcess(new Mission(launch.Mission, idOrbit), _repository.GetCurrentlyTransaction());
                Guid? idLocation = await SaveLaunchEntitiesProcess(launch.Pad?.Location, _repository.GetCurrentlyTransaction());
                Guid? idPad = await SaveLaunchEntitiesProcess(new Pad(launch.Pad, idLocation), _repository.GetCurrentlyTransaction());
                
                await _launchBusiness.SaveTransaction(new Launch(launch, idStatus, idLaunchServiceProvider, idRocket, idMission, idPad));
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

        private async Task<Guid?> SaveLaunchEntitiesProcess<T>(T entity, IDbContextTransaction transaction) where T : BaseEntity
        {
            if(entity == null)
                return null;

            Guid? dbGuid = await DatabaseGuid(entity, transaction);

            if(dbGuid == null || dbGuid == Guid.Empty)
                return await SaveNewLaunchEntity(entity, transaction);

            return dbGuid;
        }

        private async Task<Guid> SaveNewLaunchEntity<T>(T entity, IDbContextTransaction transaction) where T : BaseEntity
        {
            var _dapper = _uow.Dapper<T>() as IGenericDapperRepository<T>;

            entity.Id = Guid.NewGuid();
            entity.ImportedT = DateTime.Now;
            entity.AtualizationDate = DateTime.Now;
            entity.EntityStatus = EStatus.PUBLISHED.GetDisplayName();

            await _dapper.Save(entity, transaction);

            return entity.Id;
        }

        private async Task<Guid> DatabaseGuid<T>(T entity, IDbContextTransaction transaction) where T : BaseEntity
        {
            var _dapper = _uow.Dapper<T>() as IGenericDapperRepository<T>;
            return await _dapper.GetSelected<Guid>(
                columns: "Id",
                where: "id_from_api = @IdFromApi",
                parameters: new { IdFromApi = entity.IdFromApi },
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
    
        private async Task RefreshView()
        {
            ILaunchViewBusiness _launchViewBusiness = GetBusiness(typeof(ILaunchViewBusiness)) as ILaunchViewBusiness;
            await _launchViewBusiness.RefreshView();
        }
    }
}
