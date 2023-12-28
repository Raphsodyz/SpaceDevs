using Business.DTO;
using AutoMapper;
using Business.Interface;
using Data.Interface;
using Domain.Entities;
using Cross.Cutting.Enum;
using Cross.Cutting.Helper;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Linq.Expressions;
using System.Net.Http.Json;

namespace Business.Business
{
    public class LaunchApiBusiness : BusinessBase<Launch, ILaunchRepository>, ILaunchApiBusiness, IBusiness
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        public LaunchApiBusiness(IUnitOfWork uow,
            IConfiguration configuration,
            IMapper mapper):base(uow)
        {
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<LaunchDTO> GetOneLaunch(Guid? launchId)
        {
            ILaunchBusiness _launchBusiness = GetBusiness(typeof(ILaunchBusiness)) as ILaunchBusiness;

            if (launchId == null)
                throw new ArgumentNullException(ErrorMessages.NullArgument);

            Expression<Func<Launch, bool>> launchQuery = l => l.Id == launchId && l.EntityStatus == EStatus.PUBLISHED.GetDisplayName();
            bool launchExist = await _launchBusiness.GetSelected(filter: launchQuery, selectColumns: l => l.Id) != Guid.Empty;
            if (!launchExist)
                throw new KeyNotFoundException(ErrorMessages.KeyNotFound);

            Launch launch = await _launchBusiness.Get(
                filter: launchQuery,
                includedProperties: "Status, LaunchServiceProvider, Rocket.Configuration, Mission.Orbit, Pad.Location");

            var result = _mapper.Map<LaunchDTO>(launch);
            return result;
        }

        public async Task<Pagination<LaunchDTO>> GetAllLaunchPaged(int? page)
        {
            ILaunchBusiness _launchBusiness = GetBusiness(typeof(ILaunchBusiness)) as ILaunchBusiness;

            int totalEntities = await _launchBusiness.EntityCount(l => l.EntityStatus == EStatus.PUBLISHED.GetDisplayName());
            int totalPages = totalEntities % 10 == 0 ? totalEntities / 10 : (totalEntities / 10) + 1;
            if (page > totalPages || page < 0)
                throw new InvalidOperationException($"{ErrorMessages.InvalidPageSelected} Total pages = {totalPages}");

            List<Expression<Func<Launch, bool>>> publishedLaunchQuery = new()
            { l => l.EntityStatus == EStatus.PUBLISHED.GetDisplayName() };
            var selectedPageLaunchList = await _launchBusiness.GetAllPaged(
                page ?? 0, 10,
                filters: publishedLaunchQuery,
                includedProperties: "Status, LaunchServiceProvider, Rocket.Configuration, Mission.Orbit, Pad.Location",
                orderBy: l => l.OrderBy(la => la.Id));

            if (selectedPageLaunchList.Entities?.Count == 0)
                throw new KeyNotFoundException(ErrorMessages.NoData);

            var resultado = new Pagination<LaunchDTO>();
            resultado = _mapper.Map<Pagination<LaunchDTO>>(selectedPageLaunchList);
            
            return resultado;
        }

        public async Task SoftDeleteLaunch(Guid? launchId)
        {
            ILaunchBusiness _launchBusiness = GetBusiness(typeof(ILaunchBusiness)) as ILaunchBusiness;

            if (launchId == null)
                throw new ArgumentNullException(ErrorMessages.NullArgument);

            Expression<Func<Launch, bool>> launchQuery = l => l.Id == launchId && l.EntityStatus == EStatus.PUBLISHED.GetDisplayName();
            var launch = await _launchBusiness.Get(filter: launchQuery) ?? throw new KeyNotFoundException(ErrorMessages.KeyNotFound);

            using var trans = await _repository.GetTransaction();
            try
            {
                launch.EntityStatus = EStatus.TRASH.GetDisplayName();
                await _launchBusiness.SaveTransaction(launch);
                await trans.CommitAsync();
            }
            catch (Exception ex)
            {
                await trans.RollbackAsync();
                throw ex;
            }
        }

        public async Task<LaunchDTO> UpdateLaunch(Guid? launchId)
        {
            ILaunchBusiness _launchBusiness = GetBusiness(typeof(ILaunchBusiness)) as ILaunchBusiness;

            if (launchId == null)
                throw new ArgumentNullException(ErrorMessages.NullArgument);

            Expression<Func<Launch, bool>> launchQuery = l => l.Id == launchId && l.EntityStatus == EStatus.PUBLISHED.GetDisplayName();
            var launch = await _launchBusiness.Get(filter: launchQuery) ?? throw new KeyNotFoundException(ErrorMessages.KeyNotFound);

            using var trans = await _repository.GetTransaction();
            using HttpClient client = new();
            try
            {
                string url = $"{_configuration.GetSection(EndPoints.TheSpaceDevsLaunchEndPoint).Value}{launch.ApiGuId}";
                HttpResponseMessage response = await client.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                    throw new HttpRequestException($"{response.StatusCode} - {ErrorMessages.LaunchApiEndPointError}");

                var updatedLaunch = (await response.Content.ReadFromJsonAsync<LaunchDTO>() ?? throw new HttpRequestException(ErrorMessages.DeserializingEndPointContentError)) ?? throw new KeyNotFoundException(ErrorMessages.KeyNotFound);
                launch = _mapper.Map<Launch>(updatedLaunch);
                launch.EntityStatus = EStatus.PUBLISHED.GetDisplayName();

                await trans.CommitAsync();
                return updatedLaunch;
            }
            catch (HttpRequestException ex)
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
        }

        public async Task<bool> UpdateDataSet(int? skip)
        {
            int limit = 100, offset = skip ?? 0, max = offset + 1500, entityCounter = 0;
            for (int i = offset; i < max; i += limit)
            {
                using HttpClient client = new();
                try
                {
                    string url = $"{_configuration.GetSection(EndPoints.TheSpaceDevsLaunchEndPoint).Value}?limit={limit}&offset={offset}";
                    HttpResponseMessage response = await client.GetAsync(url);
                    if (!response.IsSuccessStatusCode)
                        throw new HttpRequestException($"{response.StatusCode} - {ErrorMessages.LaunchApiEndPointError}");

                    RequestLaunchDTO dataList = await response.Content.ReadFromJsonAsync<RequestLaunchDTO>() ?? throw new HttpRequestException(ErrorMessages.DeserializingEndPointContentError);
                    if ((bool)!dataList.Results?.Any())
                        throw new InvalidOperationException(ErrorMessages.NoDataFromSpaceDevApi);

                    foreach(var data in dataList.Results)
                    {
                        var launch = _mapper.Map<Launch>(data);
                        await SaveLaunch(launch);
                        entityCounter++;
                    }

                    await GenerateLog(offset, SuccessMessages.PartialImportSuccess, entityCounter, true);
                    entityCounter = 0;
                    offset += limit;
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
            }

            await GenerateLog(offset, SuccessMessages.ImportedDataSuccess, entityCounter, true);
            return true;
        }

        private async Task SaveLaunch(Launch launch)
        {
            ILaunchBusiness _launchBusiness = GetBusiness(typeof(ILaunchBusiness)) as ILaunchBusiness;
            IConfigurationBusiness _configurationBusiness = GetBusiness(typeof(IConfigurationBusiness)) as IConfigurationBusiness;
            ILaunchServiceProviderBusiness _launchServiceProviderBusiness = GetBusiness(typeof(ILaunchServiceProviderBusiness)) as ILaunchServiceProviderBusiness;
            ILocationBusiness _locationBuiness = GetBusiness(typeof(ILocationBusiness)) as ILocationBusiness;
            IMissionBusiness _missionBusiness = GetBusiness(typeof(IMissionBusiness)) as IMissionBusiness;
            IOrbitBusiness _orbitBusiness = GetBusiness(typeof(IOrbitBusiness)) as IOrbitBusiness;
            IPadBusiness _padBusiness = GetBusiness(typeof(IPadBusiness)) as IPadBusiness;
            IRocketBusiness _rocketBusiness = GetBusiness(typeof(IRocketBusiness)) as IRocketBusiness;
            IStatusBusiness _statusBusiness = GetBusiness(typeof(IStatusBusiness)) as IStatusBusiness;

            if (launch == null)
                throw new ArgumentNullException(ErrorMessages.NullArgument);

            using var trans = await _repository.GetTransaction();
            try
            {
                Status status = new();
                if (launch.Status != null)
                {
                    Guid id = await _statusBusiness.GetSelected(filter: s => s.IdFromApi == launch.Status.IdFromApi, selectColumns: s => s.Id);

                    status.Id = id != Guid.Empty ? id : Guid.Empty;
                    status.Name = launch.Status.Name;
                    status.IdFromApi = launch.Status.IdFromApi;
                    status.EntityStatus = EStatus.PUBLISHED.GetDisplayName();

                    await _statusBusiness.SaveTransaction(status);
                }

                LaunchServiceProvider launchServiceProvider = new();
                if (launch.LaunchServiceProvider != null)
                {
                    Guid id = await _launchServiceProviderBusiness.GetSelected(filter: s => s.IdFromApi == launch.LaunchServiceProvider.IdFromApi, selectColumns: s => s.Id);

                    launchServiceProvider.Id = id != Guid.Empty ? id : Guid.Empty;
                    launchServiceProvider.Name = launch.LaunchServiceProvider.Name;
                    launchServiceProvider.Url = launch.LaunchServiceProvider.Url;
                    launchServiceProvider.Type = launch.LaunchServiceProvider.Type;
                    launchServiceProvider.IdFromApi = launch.LaunchServiceProvider.IdFromApi;
                    launchServiceProvider.EntityStatus = EStatus.PUBLISHED.GetDisplayName();

                    await _launchServiceProviderBusiness.SaveTransaction(launchServiceProvider);
                }

                Rocket rocket = new();
                if (launch.Rocket != null)
                {
                    Configuration configuration = new();
                    if (launch.Rocket.Configuration != null)
                    {
                        Guid idConfiguration = await _configurationBusiness.GetSelected(filter: s => s.IdFromApi == launch.Rocket.Configuration.IdFromApi, selectColumns: s => s.Id);

                        configuration.Id = idConfiguration != Guid.Empty ? idConfiguration : Guid.Empty;
                        configuration.LaunchLibraryId = launch.Rocket.Configuration.LaunchLibraryId;
                        configuration.Url = launch.Rocket.Configuration.Url;
                        configuration.Name = launch.Rocket.Configuration.Name;
                        configuration.Family = launch.Rocket.Configuration.Family;
                        configuration.FullName = launch.Rocket.Configuration.FullName;
                        configuration.Variant = launch.Rocket.Configuration.Variant;
                        configuration.IdFromApi = launch.Rocket.Configuration.IdFromApi;
                        configuration.EntityStatus = EStatus.PUBLISHED.GetDisplayName();

                        await _configurationBusiness.SaveTransaction(configuration);
                    }

                    Guid idRocket = await _rocketBusiness.GetSelected(filter: s => s.IdFromApi == launch.Rocket.IdFromApi, selectColumns: s => s.Id);

                    rocket.Id = idRocket != Guid.Empty ? idRocket : Guid.Empty;
                    rocket.IdConfiguration = configuration.Id == Guid.Empty ? null : configuration.Id;
                    rocket.IdFromApi = launch.Rocket.IdFromApi;
                    rocket.EntityStatus = EStatus.PUBLISHED.GetDisplayName();

                    await _rocketBusiness.SaveTransaction(rocket);
                }

                Mission mission = new();
                if (launch.Mission != null)
                {
                    Orbit orbit = new();
                    if (launch.Mission.Orbit != null)
                    {
                        Guid idOrbit = await _orbitBusiness.GetSelected(filter: s => s.IdFromApi == launch.Mission.Orbit.IdFromApi, selectColumns: s => s.Id);

                        orbit.Id = idOrbit != Guid.Empty ? idOrbit : Guid.Empty;
                        orbit.Name = launch.Mission.Orbit.Name;
                        orbit.Abbrev = launch.Mission.Orbit.Abbrev;
                        orbit.IdFromApi = launch.Mission.Orbit.IdFromApi;
                        orbit.EntityStatus = EStatus.PUBLISHED.GetDisplayName();

                        await _orbitBusiness.SaveTransaction(orbit);
                    }

                    Guid idMission = await _missionBusiness.GetSelected(filter: s => s.IdFromApi == launch.Mission.IdFromApi, selectColumns: s => s.Id);

                    mission.Id = idMission != Guid.Empty ? idMission : Guid.Empty;
                    mission.Description = launch.Mission.Description;
                    mission.Name = launch.Mission.Name;
                    mission.Type = launch.Mission.Type;
                    mission.IdOrbit = orbit.Id == Guid.Empty ? null : orbit.Id;
                    mission.IdFromApi = launch.Mission.IdFromApi;
                    mission.EntityStatus = EStatus.PUBLISHED.GetDisplayName();

                    await _missionBusiness.SaveTransaction(mission);
                }

                Pad pad = new();
                if (launch.Pad != null)
                {
                    Location location = new();
                    if (launch.Pad.Location != null)
                    {
                        Guid idLocation = await _locationBuiness.GetSelected(filter: s => s.IdFromApi == launch.Pad.Location.IdFromApi, selectColumns: s => s.Id);
                        
                        location.Id = idLocation != Guid.Empty ? idLocation : Guid.Empty;
                        location.Url = launch.Pad.Location.Url;
                        location.Name = launch.Pad.Location.Name;
                        location.CountryCode = launch.Pad.Location.CountryCode;
                        location.MapImage = launch.Pad.Location.MapImage;
                        location.TotalLandingCount = launch.Pad.Location.TotalLandingCount;
                        location.TotalLaunchCount = launch.Pad.Location.TotalLaunchCount;
                        location.IdFromApi = launch.Pad.Location.IdFromApi;
                        location.EntityStatus = EStatus.PUBLISHED.GetDisplayName();

                        await _locationBuiness.SaveTransaction(location);
                    }

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
                    pad.EntityStatus = EStatus.PUBLISHED.GetDisplayName();

                    await _padBusiness.SaveTransaction(pad);
                }

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
                    EntityStatus = EStatus.PUBLISHED.GetDisplayName(),
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
    
        public async Task<List<LaunchDTO>> SearchByParam(SearchLaunchDTO search)
        {
            IMissionBusiness _missionBusiness = GetBusiness(typeof(IMissionBusiness)) as IMissionBusiness;
            IConfigurationBusiness _configurationBusiness = GetBusiness(typeof(IConfigurationBusiness)) as IConfigurationBusiness;
            ILocationBusiness _locationBusiness = GetBusiness(typeof(ILocationBusiness)) as ILocationBusiness;
            IPadBusiness _padBusiness = GetBusiness(typeof(IPadBusiness)) as IPadBusiness;
            ILaunchBusiness _launchBusiness = GetBusiness(typeof(ILaunchBusiness)) as ILaunchBusiness;

            List<Expression<Func<Launch, bool>>> query = new();
            if(!string.IsNullOrEmpty(search.Mission))
            {
                var idsMission = await _missionBusiness.ILikeSearch(searchTerm: search.Mission.Trim(), selectColumns: m => m.Id);
                if(idsMission != null && idsMission.Any()) query.Add(l => idsMission.Contains((Guid)l.IdMission));
            }
                
            if(!string.IsNullOrWhiteSpace(search.Rocket))
            {
                var idsRocket = await _configurationBusiness.ILikeSearch(searchTerm: search.Rocket.Trim(), selectColumns: r => r.Id);
                if(idsRocket != null && idsRocket.Any()) query.Add(l => idsRocket.Contains((Guid)l.Rocket.IdConfiguration));
            }
        
            if(!string.IsNullOrWhiteSpace(search.Location))
            {
                var idsLocation = await _locationBusiness.ILikeSearch(searchTerm: search.Location.Trim(), selectColumns: l => l.Id);
                if(idsLocation != null && idsLocation.Any()) query.Add(l => idsLocation.Contains((Guid)l.Pad.IdLocation));
            }

            if(!string.IsNullOrWhiteSpace(search.Pad))
            {
                var idsPad = await _padBusiness.ILikeSearch(searchTerm: search.Pad.Trim(), selectColumns: p => p.Id);
                if(idsPad != null && idsPad.Any()) query.Add(l => idsPad.Contains((Guid)l.IdPad));
            }

            if(!string.IsNullOrWhiteSpace(search.Launch))
            {
                var idsLaunch = await _launchBusiness.ILikeSearch(searchTerm: search.Launch.Trim(), selectColumns: l => l.Id);
                if(idsLaunch != null && idsLaunch.Any()) query.Add(l => idsLaunch.Contains(l.Id));
            }

            if(!query.Any())
                throw new KeyNotFoundException(ErrorMessages.KeyNotFound);

            var found = await _launchBusiness.GetMany(query, includedProperties: "Status, LaunchServiceProvider, Rocket.Configuration, Mission.Orbit, Pad.Location", howMany: 20) ?? throw new KeyNotFoundException(ErrorMessages.KeyNotFound);
            return _mapper.Map<List<LaunchDTO>>(found);
        }
    }
}
