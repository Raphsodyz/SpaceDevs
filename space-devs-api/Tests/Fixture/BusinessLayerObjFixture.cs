using System.Data.Common;
using AutoMapper;
using Core.Database.Repository;

namespace Tests.Fixture
{
    public class BusinessLayerObjFixture : IDisposable
    {
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
        public Mock<IHttpClientFactory> FactoryClient { get; private set; }
        public Mock<IMapper> Mapper { get; private set; }

        public BusinessLayerObjFixture()
        {
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
            FactoryClient = new Mock<IHttpClientFactory>();
            Client = new Mock<IHttpClientFactory>();
            Mapper = new Mock<IMapper>();
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