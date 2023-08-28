using Application.DTO;
using AutoMapper;
using Business.Interface;
using Data.Interface;
using Domain.Entities;
using Domain.Enum;
using Domain.Helper;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

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

            using var conn = new MySqlConnection(_configuration.GetSection("ConnectionStrings:default").Value);
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
            ILaunchDesignatorBusiness _launchDesignatorBusiness = GetBusiness(typeof(ILaunchDesignatorBusiness)) as ILaunchDesignatorBusiness;
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
                    int id = _statusBusiness.GetSelected(filter: s => s.IdFromApi == launch.Status.IdFromApi, selectColumns: s => s.Id);

                    status.Id = id > 0 ? id : 0;
                    status.Name = launch.Status.Name;
                    status.IdFromApi = launch.Status.IdFromApi;
                    status.EntityStatus = EStatus.DRAFT.GetDisplayName();

                    _statusBusiness.SaveTransaction(status);
                }

                LaunchServiceProvider launchServiceProvider = new();
                if (launch.LaunchServiceProvider != null)
                {
                    int id = _launchServiceProviderBusiness.GetSelected(filter: s => s.IdFromApi == launch.LaunchServiceProvider.IdFromApi, selectColumns: s => s.Id);

                    launchServiceProvider.Id = id > 0 ? id : 0;
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
                        int idConfiguration = _configurationBusiness.GetSelected(filter: s => s.IdFromApi == launch.Rocket.Configuration.IdFromApi, selectColumns: s => s.Id);

                        configuration.Id = idConfiguration > 0 ? idConfiguration : 0;
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

                    int idRocket = _rocketBusiness.GetSelected(filter: s => s.IdFromApi == launch.Rocket.IdFromApi, selectColumns: s => s.Id);

                    rocket.Id = idRocket > 0 ? idRocket : 0;
                    rocket.IdConfiguration = configuration.Id == 0 ? null : configuration.Id;
                    rocket.IdFromApi = launch.Rocket.IdFromApi;
                    rocket.EntityStatus = EStatus.DRAFT.GetDisplayName();

                    _rocketBusiness.SaveTransaction(rocket);
                }

                Mission mission = new();
                if (launch.Mission != null)
                {
                    LaunchDesignator launchDesignator = new();
                    if (launch.Mission.LaunchDesignator != null)
                    {
                        int id = _launchDesignatorBusiness.GetSelected(filter: s => s.IdFromApi == launch.Mission.LaunchDesignator.IdFromApi, selectColumns: s => s.Id);

                        launchDesignator.Id = id > 0 ? id : 0;
                        launchDesignator.IdFromApi = launch.Mission.LaunchDesignator.IdFromApi;
                        launchDesignator.EntityStatus = EStatus.DRAFT.GetDisplayName();

                        _launchDesignatorBusiness.SaveTransaction(launchDesignator);
                    }

                    Orbit orbit = new();
                    if (launch.Mission.Orbit != null)
                    {
                        int idOrbit = _orbitBusiness.GetSelected(filter: s => s.IdFromApi == launch.Mission.Orbit.IdFromApi, selectColumns: s => s.Id);

                        orbit.Id = idOrbit > 0 ? idOrbit : 0;
                        orbit.Name = launch.Mission.Orbit.Name;
                        orbit.Abbrev = launch.Mission.Orbit.Abbrev;
                        orbit.IdFromApi = launch.Mission.Orbit.IdFromApi;
                        orbit.EntityStatus = EStatus.DRAFT.GetDisplayName();

                        _orbitBusiness.SaveTransaction(orbit);
                    }

                    int idMission = _missionBusiness.GetSelected(filter: s => s.IdFromApi == launch.Mission.IdFromApi, selectColumns: s => s.Id);

                    mission.Id = idMission > 0 ? idMission : 0;
                    mission.Description = launch.Mission.Description;
                    mission.Name = launch.Mission.Name;
                    mission.Type = launch.Mission.Type;
                    mission.IdOrbit = orbit.Id == 0 ? null : orbit.Id;
                    mission.IdLaunchDesignator = launchDesignator.Id == 0 ? null : launchDesignator.Id;
                    mission.IdFromApi = launch.Mission.IdFromApi;
                    mission.EntityStatus = EStatus.DRAFT.GetDisplayName();

                    _missionBusiness.SaveTransaction(mission);
                }

                Pad pad = new Pad();
                if (launch.Pad != null)
                {
                    Location location = new();
                    if (launch.Pad.Location != null)
                    {
                        int idLocation = _locationBuiness.GetSelected(filter: s => s.IdFromApi == launch.Pad.Location.IdFromApi, selectColumns: s => s.Id);

                        location.Id = idLocation > 0 ? idLocation : 0;
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

                    int idPad = _padBusiness.GetSelected(filter: s => s.IdFromApi == launch.Pad.IdFromApi, selectColumns: s => s.Id);

                    pad.Id = idPad > 0 ? idPad : 0;
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
                    pad.IdLocation = location.Id == 0 ? null : location.Id;
                    pad.IdFromApi = launch.Pad.IdFromApi;
                    pad.EntityStatus = EStatus.DRAFT.GetDisplayName();

                    _padBusiness.SaveTransaction(pad);
                }

                int idLaunch = _launchBusiness.GetSelected(filter: s => s.ApiGuId == launch.ApiGuId, selectColumns: s => s.Id);
                Launch saveLaunch = new Launch()
                {
                    Id = idLaunch > 0 ? idLaunch : 0,
                    ApiGuId = launch.ApiGuId,
                    Url = launch.Url,
                    LaunchLibraryId = launch.LaunchLibraryId,
                    Slug = launch.Slug,
                    Name = launch.Name,
                    IdStatus = status.Id == 0 ? null : status.Id,
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
                    IdLaunchServiceProvider = launchServiceProvider.Id == 0 ? null : launchServiceProvider.Id,
                    IdRocket = rocket.Id == 0 ? null : rocket.Id,
                    IdMission = mission.Id == 0 ? null : mission.Id,
                    IdPad = pad.Id == 0 ? null : pad.Id,
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
