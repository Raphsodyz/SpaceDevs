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
                string url = $"{EndPoints.TheSpaceDevsLaunchEndPoint}{launch.ApiGuId}";
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
                catch (HttpRequestException ex)
                {
                    await GenerateLog(offset, ex.Message, entityCounter, false);
                    throw ex;
                }
                catch (InvalidOperationException ex)
                {
                    await GenerateLog(offset, ex.Message, entityCounter, false);
                    throw ex;
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

        private async Task SaveLaunch(Launch launch)
        {
            if (launch == null)
                throw new ArgumentNullException(ErrorMessages.NullArgument);

            using var trans = await _repository.GetTransaction();
            try
            {
                IStatusBusiness _statusBusiness = GetBusiness(typeof(IStatusBusiness)) as IStatusBusiness;

                Status status = new();
                if (launch.Status != null)
                {
                    Guid id = await _statusBusiness.GetSelected(filter: s => s.IdFromApi == launch.Status.IdFromApi, selectColumns: s => s.Id);

                    status.Id = id != Guid.Empty ? id : Guid.Empty;
                    status.Name = launch.Status.Name;
                    status.Abbrev = launch.Status.Abbrev;
                    status.Description = launch.Status.Description;
                    status.IdFromApi = launch.Status.IdFromApi;

                    await _statusBusiness.SaveTransaction(status);
                }

                LaunchServiceProvider launchServiceProvider = new();
                if (launch.LaunchServiceProvider != null)
                {
                    ILaunchServiceProviderBusiness _launchServiceProviderBusiness = GetBusiness(typeof(ILaunchServiceProviderBusiness)) as ILaunchServiceProviderBusiness;

                    Guid id = await _launchServiceProviderBusiness.GetSelected(filter: s => s.IdFromApi == launch.LaunchServiceProvider.IdFromApi, selectColumns: s => s.Id);

                    launchServiceProvider.Id = id != Guid.Empty ? id : Guid.Empty;
                    launchServiceProvider.Name = launch.LaunchServiceProvider.Name;
                    launchServiceProvider.Url = launch.LaunchServiceProvider.Url;
                    launchServiceProvider.Type = launch.LaunchServiceProvider.Type;
                    launchServiceProvider.IdFromApi = launch.LaunchServiceProvider.IdFromApi;

                    await _launchServiceProviderBusiness.SaveTransaction(launchServiceProvider);
                }

                Rocket rocket = new();
                if (launch.Rocket != null)
                {
                    Configuration configuration = new();
                    if (launch.Rocket.Configuration != null)
                    {
                        IConfigurationBusiness _configurationBusiness = GetBusiness(typeof(IConfigurationBusiness)) as IConfigurationBusiness;

                        Guid idConfiguration = await _configurationBusiness.GetSelected(filter: s => s.IdFromApi == launch.Rocket.Configuration.IdFromApi, selectColumns: s => s.Id);

                        configuration.Id = idConfiguration != Guid.Empty ? idConfiguration : Guid.Empty;
                        configuration.LaunchLibraryId = launch.Rocket.Configuration.LaunchLibraryId;
                        configuration.Url = launch.Rocket.Configuration.Url;
                        configuration.Name = launch.Rocket.Configuration.Name;
                        configuration.Family = launch.Rocket.Configuration.Family;
                        configuration.FullName = launch.Rocket.Configuration.FullName;
                        configuration.Variant = launch.Rocket.Configuration.Variant;
                        configuration.IdFromApi = launch.Rocket.Configuration.IdFromApi;

                        await _configurationBusiness.SaveTransaction(configuration);
                    }

                    IRocketBusiness _rocketBusiness = GetBusiness(typeof(IRocketBusiness)) as IRocketBusiness;

                    Guid idRocket = await _rocketBusiness.GetSelected(filter: s => s.IdFromApi == launch.Rocket.IdFromApi, selectColumns: s => s.Id);

                    rocket.Id = idRocket != Guid.Empty ? idRocket : Guid.Empty;
                    rocket.IdConfiguration = configuration.Id == Guid.Empty ? null : configuration.Id;
                    rocket.IdFromApi = launch.Rocket.IdFromApi;

                    await _rocketBusiness.SaveTransaction(rocket);
                }

                Mission mission = new();
                if (launch.Mission != null)
                {
                    Orbit orbit = new();
                    if (launch.Mission.Orbit != null)
                    {            
                        IOrbitBusiness _orbitBusiness = GetBusiness(typeof(IOrbitBusiness)) as IOrbitBusiness;

                        Guid idOrbit = await _orbitBusiness.GetSelected(filter: s => s.IdFromApi == launch.Mission.Orbit.IdFromApi, selectColumns: s => s.Id);

                        orbit.Id = idOrbit != Guid.Empty ? idOrbit : Guid.Empty;
                        orbit.Name = launch.Mission.Orbit.Name;
                        orbit.Abbrev = launch.Mission.Orbit.Abbrev;
                        orbit.IdFromApi = launch.Mission.Orbit.IdFromApi;

                        await _orbitBusiness.SaveTransaction(orbit);
                    }
                    IMissionBusiness _missionBusiness = GetBusiness(typeof(IMissionBusiness)) as IMissionBusiness;

                    Guid idMission = await _missionBusiness.GetSelected(filter: s => s.IdFromApi == launch.Mission.IdFromApi, selectColumns: s => s.Id);

                    mission.Id = idMission != Guid.Empty ? idMission : Guid.Empty;
                    mission.Description = launch.Mission.Description;
                    mission.Name = launch.Mission.Name;
                    mission.Type = launch.Mission.Type;
                    mission.IdOrbit = orbit.Id == Guid.Empty ? null : orbit.Id;
                    mission.IdFromApi = launch.Mission.IdFromApi;

                    await _missionBusiness.SaveTransaction(mission);
                }

                Pad pad = new();
                if (launch.Pad != null)
                {
                    Location location = new();
                    if (launch.Pad.Location != null)
                    {

                        ILocationBusiness _locationBuiness = GetBusiness(typeof(ILocationBusiness)) as ILocationBusiness;

                        Guid idLocation = await _locationBuiness.GetSelected(filter: s => s.IdFromApi == launch.Pad.Location.IdFromApi, selectColumns: s => s.Id);
                        
                        location.Id = idLocation != Guid.Empty ? idLocation : Guid.Empty;
                        location.Url = launch.Pad.Location.Url;
                        location.Name = launch.Pad.Location.Name;
                        location.CountryCode = launch.Pad.Location.CountryCode;
                        location.MapImage = launch.Pad.Location.MapImage;
                        location.TotalLandingCount = launch.Pad.Location.TotalLandingCount;
                        location.TotalLaunchCount = launch.Pad.Location.TotalLaunchCount;
                        location.IdFromApi = launch.Pad.Location.IdFromApi;

                        await _locationBuiness.SaveTransaction(location);
                    }

                    IPadBusiness _padBusiness = GetBusiness(typeof(IPadBusiness)) as IPadBusiness;

                    Guid idPad = await _padBusiness.GetSelected(filter: s => s.IdFromApi == launch.Pad.IdFromApi, selectColumns: s => s.Id);

                    pad.Id = idPad != Guid.Empty ? idPad : Guid.Empty;
                    pad.Url = launch.Pad.Url;
                    pad.AgencyId = launch.Pad.AgencyId;
                    pad.Name = launch.Pad.Name;
                    pad.InfoUrl = launch.Pad.InfoUrl;
                    pad.MapUrl = launch.Pad.MapUrl;
                    pad.WikiUrl = launch.Pad.WikiUrl;
                    pad.Latitude = launch.Pad.Latitude;
                    pad.Longitude = launch.Pad.Longitude;
                    pad.MapImage = launch.Pad.MapImage;
                    pad.TotalLaunchCount = launch.Pad.TotalLaunchCount;
                    pad.IdLocation = location.Id == Guid.Empty ? null : location.Id;
                    pad.IdFromApi = launch.Pad.IdFromApi;

                    await _padBusiness.SaveTransaction(pad);
                }
                
                ILaunchBusiness _launchBusiness = GetBusiness(typeof(ILaunchBusiness)) as ILaunchBusiness;

                Guid idLaunch = await _launchBusiness.GetSelected(filter: s => s.ApiGuId == launch.ApiGuId, selectColumns: s => s.Id);
                Launch saveLaunch = new()
                {
                    Id = idLaunch != Guid.Empty ? idLaunch : Guid.Empty,
                    ApiGuId = launch.ApiGuId,
                    Url = launch.Url,
                    LaunchLibraryId = launch.LaunchLibraryId,
                    Slug = launch.Slug,
                    Name = launch.Name,
                    IdStatus = status.Id == Guid.Empty ? null : status.Id,
                    Net = launch.Net,
                    WindowEnd = launch.WindowEnd,
                    WindowStart = launch.WindowStart,
                    Inhold = launch.Inhold,
                    TbdDate = launch.TbdDate,
                    TbdTime = launch.TbdTime,
                    Probability = launch.Probability,
                    HoldReason = launch.HoldReason,
                    FailReason = launch.FailReason,
                    Hashtag = launch.Hashtag,
                    IdLaunchServiceProvider = launchServiceProvider.Id == Guid.Empty ? null : launchServiceProvider.Id,
                    IdRocket = rocket.Id == Guid.Empty ? null : rocket.Id,
                    IdMission = mission.Id == Guid.Empty ? null : mission.Id,
                    IdPad = pad.Id == Guid.Empty ? null : pad.Id,
                    WebcastLive = launch.WebcastLive,
                    Image = launch.Image,
                    Infographic = launch.Infographic,
                    Programs = launch.Programs,
                    IdFromApi = launch.IdFromApi
                };
                await _launchBusiness.SaveTransaction(saveLaunch);
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

            var result = new Pagination<LaunchView>();
            result = _mapper.Map<Pagination<LaunchView>>(found);
            
            return result;
        }
    }
}
