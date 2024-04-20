using AutoMapper;
using Data.Interface;
using RichardSzalay.MockHttp;

namespace Tests.Fixture
{
    public class BusinessLayerObjFixture : IDisposable
    {
        public Mock<IGenericDapperRepository<Status>> DapperStatusRepository { get; private set; }
        public Mock<IGenericDapperRepository<LaunchServiceProvider>> DapperLaunchServiceProviderRepository { get; private set; }
        public Mock<IGenericDapperRepository<Configuration>> DapperConfigurationRepository { get; private set;}
        public Mock<IGenericDapperRepository<Rocket>> DapperRocketRepository { get; private set; }
        public Mock<IGenericDapperRepository<Mission>> DapperMissionRepository { get; private set; }
        public Mock<IGenericDapperRepository<Orbit>> DapperOrbitRepository { get; private set; }
        public Mock<IGenericDapperRepository<Pad>> DapperPadRepository { get; private set; }
        public Mock<IGenericDapperRepository<Location>> DapperLocationRepository { get; private set; }
        public Mock<IUpdateLogRepository> UpdateLogRepository { get; private set; }
        public Mock<IConfigurationRepository> ConfigurationRepository { get; private set; }
        public Mock<ILaunchRepository> LaunchRepository { get; private set; }
        public Mock<ILaunchServiceProviderRepository> LaunchServiceProviderRepository { get; private set; }
        public Mock<ILocationRepository> LocationRepository { get; private set; }
        public Mock<IMissionRepository> MissionRepository { get; private set; }
        public Mock<IOrbitRepository> OrbitRepository { get; private set; }
        public Mock<IPadRepository> PadRepository { get; private set; }
        public Mock<IRocketRepository> RocketRepository { get; private set; }
        public Mock<IStatusRepository> StatusRepository { get; private set; }
        public Mock<ILaunchViewRepository> LaunchViewRepository { get; private set; }
        public Mock<IHttpClientFactory> Client { get; set; }
        public Mock<IUnitOfWork> Uow { get; private set; } 
        public Mock<IHttpClientFactory> FactoryClient { get; private set; }
        public Mock<IMapper> Mapper { get; private set; }
        public Mock<ILaunchApiBusiness> LaunchApiBusiness { get; private set; }

        public BusinessLayerObjFixture()
        {
            DapperStatusRepository = new Mock<IGenericDapperRepository<Status>>();
            DapperLaunchServiceProviderRepository = new Mock<IGenericDapperRepository<LaunchServiceProvider>>();
            DapperConfigurationRepository = new Mock<IGenericDapperRepository<Configuration>>();
            DapperRocketRepository = new Mock<IGenericDapperRepository<Rocket>>();
            DapperMissionRepository = new Mock<IGenericDapperRepository<Mission>>();
            DapperOrbitRepository = new Mock<IGenericDapperRepository<Orbit>>();
            DapperPadRepository = new Mock<IGenericDapperRepository<Pad>>();
            DapperLocationRepository = new Mock<IGenericDapperRepository<Location>>();
            UpdateLogRepository = new Mock<IUpdateLogRepository>();
            ConfigurationRepository = new Mock<IConfigurationRepository>();
            LaunchRepository = new Mock<ILaunchRepository>();
            LaunchServiceProviderRepository = new Mock<ILaunchServiceProviderRepository>();
            LocationRepository = new Mock<ILocationRepository>();
            MissionRepository = new Mock<IMissionRepository>();
            OrbitRepository = new Mock<IOrbitRepository>();
            PadRepository = new Mock<IPadRepository>();
            RocketRepository = new Mock<IRocketRepository>();
            StatusRepository = new Mock<IStatusRepository>();
            LaunchViewRepository = new Mock<ILaunchViewRepository>();
            Uow = new Mock<IUnitOfWork>();
            FactoryClient = new Mock<IHttpClientFactory>();
            Client = new Mock<IHttpClientFactory>();
            Mapper = new Mock<IMapper>();
            LaunchApiBusiness = new Mock<ILaunchApiBusiness>();
        }

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}