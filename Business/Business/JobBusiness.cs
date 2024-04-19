using AutoMapper;
using Business.Interface;
using Data.Interface;
using Domain.Entities;
using Cross.Cutting.Enum;
using Cross.Cutting.Helper;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;
using Business.DTO.Entities;

namespace Business.Business
{
    public class JobBusiness : BusinessBase<Launch, ILaunchRepository>, IJobBusiness, IBusiness
    {
        private readonly IHttpClientFactory _client;
        private readonly IMapper _mapper;
        public JobBusiness(IUnitOfWork uow,
            IHttpClientFactory client,
            IMapper mapper):base(uow)
        {
            _client = client;
            _mapper = mapper;
        }

        public async Task UpdateDataSet()
        {
            IUpdateLogBusiness _updateLogBusiness = GetBusiness(typeof(IUpdateLogBusiness)) as IUpdateLogBusiness;

            int limit = 100, offset = await _updateLogBusiness.LastOffSet(), max = 1500, entityCounter = 0;
            for (int i = offset; i < max; i += limit)
            {
                var client = _client.CreateClient();
                try
                {
                    string url = $"{EndPoints.TheSpaceDevsLaunchEndPoint}?limit={limit}&offset={offset}";
                    HttpResponseMessage response = await client.GetAsync(url);
                    if (!response.IsSuccessStatusCode)
                        throw new HttpRequestException($"{response.StatusCode} - {ErrorMessages.LaunchApiEndPointError}");

                    RequestLaunchDTO dataList = await response.Content.ReadFromJsonAsync<RequestLaunchDTO>() ?? throw new HttpRequestException(ErrorMessages.DeserializingContentError);
                    if ((bool)!dataList.Results?.Any())
                        throw new InvalidOperationException(ErrorMessages.NoDataFromSpaceDevApi);

                    foreach (var data in dataList.Results)
                    {
                        var launch = _mapper.Map<Launch>(data);
                        await SaveLaunch(launch);
                        entityCounter++;
                    }
                    await GenerateLog(offset, SuccessMessages.PartialImportSuccess, entityCounter, true);
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
                    Guid id = await _statusBusiness.GetSelected(
                        filter: s => s.IdFromApi == launch.Status.IdFromApi,
                        selectColumns: s => s.Id,
                        buildObject: s => s);

                    status.Id = id != Guid.Empty ? id : Guid.Empty;
                    status.Name = launch.Status.Name;
                    status.IdFromApi = launch.Status.IdFromApi;
                    status.EntityStatus = EStatus.PUBLISHED.GetDisplayName();

                    await _statusBusiness.SaveTransaction(status);
                }

                LaunchServiceProvider launchServiceProvider = new();
                if (launch.LaunchServiceProvider != null)
                {
                    Guid id = await _launchServiceProviderBusiness.GetSelected(
                        filter: s => s.IdFromApi == launch.LaunchServiceProvider.IdFromApi,
                        selectColumns: s => s.Id,
                        buildObject: s => s);

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
                        Guid idConfiguration = await _configurationBusiness.GetSelected(
                            filter: s => s.IdFromApi == launch.Rocket.Configuration.IdFromApi,
                            selectColumns: s => s.Id,
                            buildObject: s => s);

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

                    Guid idRocket = await _rocketBusiness.GetSelected(
                        filter: s => s.IdFromApi == launch.Rocket.IdFromApi,
                        selectColumns: s => s.Id,
                        buildObject: s => s);

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
                        Guid idOrbit = await _orbitBusiness.GetSelected(
                            filter: s => s.IdFromApi == launch.Mission.Orbit.IdFromApi,
                            selectColumns: s => s.Id,
                            buildObject: s => s);

                        orbit.Id = idOrbit != Guid.Empty ? idOrbit : Guid.Empty;
                        orbit.Name = launch.Mission.Orbit.Name;
                        orbit.Abbrev = launch.Mission.Orbit.Abbrev;
                        orbit.IdFromApi = launch.Mission.Orbit.IdFromApi;
                        orbit.EntityStatus = EStatus.PUBLISHED.GetDisplayName();

                        await _orbitBusiness.SaveTransaction(orbit);
                    }

                    Guid idMission = await _missionBusiness.GetSelected(
                        filter: s => s.IdFromApi == launch.Mission.IdFromApi,
                        selectColumns: s => s.Id,
                        buildObject: s => s);

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
                        Guid idLocation = await _locationBuiness.GetSelected(
                            filter: s => s.IdFromApi == launch.Pad.Location.IdFromApi,
                            selectColumns: s => s.Id,
                            buildObject: s => s);

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

                    Guid idPad = await _padBusiness.GetSelected(
                        filter: s => s.IdFromApi == launch.Pad.IdFromApi,
                        selectColumns: s => s.Id,
                        buildObject: s => s);

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

                Guid idLaunch = await _launchBusiness.GetSelected(
                    filter: s => s.ApiGuid == launch.ApiGuid,
                    selectColumns: s => s.Id,
                    buildObject: s => s);
                    
                Launch saveLaunch = new()
                {
                    Id = idLaunch != Guid.Empty ? idLaunch : Guid.Empty,
                    ApiGuid = launch.ApiGuid,
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

        private async Task GenerateLog(int offset, string errorMessage, int entityCount, bool success)
        {
            IUpdateLogBusiness _updateLogBusiness = GetBusiness(typeof(IUpdateLogBusiness)) as IUpdateLogBusiness;

            var log = new UpdateLog()
            {
                TransactionDate = DateTime.Now,
                OffSet = offset,
                Success = success,
                Message = errorMessage,
                EntityCount = entityCount,
                EntityStatus = EStatus.PUBLISHED.GetDisplayName()
            };
            await _updateLogBusiness.Save(log);
        }
    }
}
