using Application.DTO;
using AutoMapper;
using Business.Interface;
using Data.Interface;
using Domain.Entities;
using Domain.Enum;
using Domain.Helper;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System.Data;
using System.Linq.Expressions;
using System.Net.Http.Json;

namespace Business.Business
{
    public class JobBusiness : BusinessBase<Launch, ILaunchRepository>, IJobBusiness, IBusiness
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        public JobBusiness(IUnitOfWork uow,
            IConfiguration configuration,
            IMapper mapper):base(uow)
        {
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<bool> UpdateDataSet()
        {
            int limit = 100;
            int offset = 0;
            int max = 2000;
            int entityCounter = 0;

            IUpdateLogBusiness _updateLogBusiness = GetBusiness(typeof(IUpdateLogBusiness)) as IUpdateLogBusiness;

            List<Expression<Func<UpdateLog, bool>>> qryLastUpdateJobLog = new()
            { u => u.Origin == EOrigin.API_UPDATE.GetDisplayName() };
            offset = _updateLogBusiness.GetAllSelectedColumns(
                filters: qryLastUpdateJobLog,
                selectColumns: c => c.OffSet,
                orderBy: u => u.OrderByDescending(d => d.TransactionDate)).FirstOrDefault();

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
                    if (dataList.Results?.Count == 0)
                        throw new InvalidOperationException(ErrorMessages.NoDataFromSpaceDevApi);

                    foreach (var data in dataList.Results)
                    {
                        var launch = _mapper.Map<Launch>(data);
                        SaveLaunch(launch);
                        entityCounter++;
                    }
                    GenerateLog(offset, SuccessMessages.PartialImportSuccess, entityCounter, true);
                    offset += limit;
                }
                catch (HttpRequestException ex)
                {
                    GenerateLog(offset, ex.Message, entityCounter, false);
                    throw ex;
                }
                catch (InvalidOperationException ex)
                {
                    GenerateLog(offset, ex.Message, entityCounter, false);
                    throw ex;
                }
                catch (Exception ex)
                {
                    GenerateLog(offset, ex.Message, entityCounter, false);
                    throw ex;
                }
            }
            GenerateLog(offset, SuccessMessages.ImportedDataSuccess, entityCounter, true);

            using var conn = new MySqlConnection(Environment.GetEnvironmentVariable(_configuration.GetSection("ConnectionStrings:default").Value));
            using var command = new MySqlCommand("sp_status_published_routine", conn) { CommandType = CommandType.StoredProcedure };
            try
            {
                conn.Open();
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                conn.Close();
                GenerateLog(offset, ErrorMessages.StoredProcedurePublishedRoutineError, entityCounter, false);
            }

            conn.Close();
            return true;
        }

        public void GenerateJobLog(bool success, string message)
        {
            IUpdateLogBusiness _updateLogBusiness = GetBusiness(typeof(IUpdateLogBusiness)) as IUpdateLogBusiness;

            List<Expression<Func<UpdateLog, bool>>> qryLastUpdateJobLog = new() 
            { u => u.Origin == EOrigin.API_UPDATE.GetDisplayName() };
            var lastLog = _updateLogBusiness.GetAllSelectedColumns(
                filters: qryLastUpdateJobLog,
                selectColumns: c => new { c.TransactionDate, c.OffSet, c.EntityCount },
                orderBy: u => u.OrderByDescending(d => d.TransactionDate)).FirstOrDefault();

            if(lastLog != null)
            {
                var log = new UpdateLog()
                {
                    TransactionDate = DateTime.Now,
                    OffSet = lastLog.OffSet,
                    Success = success,
                    Message = message,
                    EntityCount = lastLog.EntityCount,
                    Origin = EOrigin.JOB.GetDisplayName(),
                    EntityStatus = EStatus.PUBLISHED.GetDisplayName()
                };
                _updateLogBusiness.Save(log);
            }
        }

        private void SaveLaunch(Launch launch)
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

            using var trans = _repository.GetTransaction();
            try
            {
                Status status = new();
                if (launch.Status != null)
                {
                    Guid id = _statusBusiness.GetSelected(filter: s => s.IdFromApi == launch.Status.IdFromApi, selectColumns: s => s.Id);

                    status.Id = id != Guid.Empty ? id : Guid.Empty;
                    status.Name = launch.Status.Name;
                    status.IdFromApi = launch.Status.IdFromApi;
                    status.EntityStatus = EStatus.DRAFT.GetDisplayName();

                    _statusBusiness.SaveTransaction(status);
                }

                LaunchServiceProvider launchServiceProvider = new();
                if (launch.LaunchServiceProvider != null)
                {
                    Guid id = _launchServiceProviderBusiness.GetSelected(filter: s => s.IdFromApi == launch.LaunchServiceProvider.IdFromApi, selectColumns: s => s.Id);

                    launchServiceProvider.Id = id != Guid.Empty ? id : Guid.Empty;
                    launchServiceProvider.Name = launch.LaunchServiceProvider.Name;
                    launchServiceProvider.Url = launch.LaunchServiceProvider.Url;
                    launchServiceProvider.Type = launch.LaunchServiceProvider.Type;
                    launchServiceProvider.IdFromApi = launch.LaunchServiceProvider.IdFromApi;
                    launchServiceProvider.EntityStatus = EStatus.DRAFT.GetDisplayName();

                    _launchServiceProviderBusiness.SaveTransaction(launchServiceProvider);
                }

                Rocket rocket = new();
                if (launch.Rocket != null)
                {
                    Configuration configuration = new();
                    if (launch.Rocket.Configuration != null)
                    {
                        Guid idConfiguration = _configurationBusiness.GetSelected(filter: s => s.IdFromApi == launch.Rocket.Configuration.IdFromApi, selectColumns: s => s.Id);

                        configuration.Id = idConfiguration != Guid.Empty ? idConfiguration : Guid.Empty;
                        configuration.LaunchLibraryId = launch.Rocket.Configuration.LaunchLibraryId;
                        configuration.Url = launch.Rocket.Configuration.Url;
                        configuration.Name = launch.Rocket.Configuration.Name;
                        configuration.Family = launch.Rocket.Configuration.Family;
                        configuration.FullName = launch.Rocket.Configuration.FullName;
                        configuration.Variant = launch.Rocket.Configuration.Variant;
                        configuration.IdFromApi = launch.Rocket.Configuration.IdFromApi;
                        configuration.EntityStatus = EStatus.DRAFT.GetDisplayName();

                        _configurationBusiness.SaveTransaction(configuration);
                    }

                    Guid idRocket = _rocketBusiness.GetSelected(filter: s => s.IdFromApi == launch.Rocket.IdFromApi, selectColumns: s => s.Id);

                    rocket.Id = idRocket != Guid.Empty ? idRocket : Guid.Empty;
                    rocket.IdConfiguration = configuration.Id == Guid.Empty ? null : configuration.Id;
                    rocket.IdFromApi = launch.Rocket.IdFromApi;
                    rocket.EntityStatus = EStatus.DRAFT.GetDisplayName();

                    _rocketBusiness.SaveTransaction(rocket);
                }

                Mission mission = new();
                if (launch.Mission != null)
                {
                    Orbit orbit = new();
                    if (launch.Mission.Orbit != null)
                    {
                        Guid idOrbit = _orbitBusiness.GetSelected(filter: s => s.IdFromApi == launch.Mission.Orbit.IdFromApi, selectColumns: s => s.Id);

                        orbit.Id = idOrbit != Guid.Empty ? idOrbit : Guid.Empty;
                        orbit.Name = launch.Mission.Orbit.Name;
                        orbit.Abbrev = launch.Mission.Orbit.Abbrev;

                        orbit.IdFromApi = launch.Mission.Orbit.IdFromApi;
                        orbit.EntityStatus = EStatus.DRAFT.GetDisplayName();

                        _orbitBusiness.SaveTransaction(orbit);
                    }

                    Guid idMission = _missionBusiness.GetSelected(filter: s => s.IdFromApi == launch.Mission.IdFromApi, selectColumns: s => s.Id);

                    mission.Id = idMission != Guid.Empty ? idMission : Guid.Empty;
                    mission.Description = launch.Mission.Description;
                    mission.Name = launch.Mission.Name;
                    mission.Type = launch.Mission.Type;
                    mission.IdOrbit = orbit.Id == Guid.Empty ? null : orbit.Id;
                    mission.IdFromApi = launch.Mission.IdFromApi;
                    mission.EntityStatus = EStatus.DRAFT.GetDisplayName();

                    _missionBusiness.SaveTransaction(mission);
                }

                Pad pad = new();
                if (launch.Pad != null)
                {
                    Location location = new();
                    if (launch.Pad.Location != null)
                    {
                        Guid idLocation = _locationBuiness.GetSelected(filter: s => s.IdFromApi == launch.Pad.Location.IdFromApi, selectColumns: s => s.Id);

                        location.Id = idLocation != Guid.Empty ? idLocation : Guid.Empty;
                        location.Url = launch.Pad.Location.Url;
                        location.Name = launch.Pad.Location.Name;
                        location.CountryCode = launch.Pad.Location.CountryCode;
                        location.MapImage = launch.Pad.Location.MapImage;
                        location.TotalLandingCount = launch.Pad.Location.TotalLandingCount;
                        location.TotalLaunchCount = launch.Pad.Location.TotalLaunchCount;
                        location.IdFromApi = launch.Pad.Location.IdFromApi;
                        location.EntityStatus = EStatus.DRAFT.GetDisplayName();

                        _locationBuiness.SaveTransaction(location);
                    }

                    Guid idPad = _padBusiness.GetSelected(filter: s => s.IdFromApi == launch.Pad.IdFromApi, selectColumns: s => s.Id);

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
                    pad.EntityStatus = EStatus.DRAFT.GetDisplayName();

                    _padBusiness.SaveTransaction(pad);
                }

                Guid idLaunch = _launchBusiness.GetSelected(filter: s => s.ApiGuId == launch.ApiGuId, selectColumns: s => s.Id);
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
                    EntityStatus = EStatus.DRAFT.GetDisplayName(),
                    IdFromApi = launch.IdFromApi,
                };
                _launchBusiness.SaveTransaction(saveLaunch);

                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                throw ex;
            }
        }

        private void GenerateLog(int offset, string errorMessage, int entityCount, bool success)
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
            _updateLogBusiness.Save(log);
        }
    }
}
