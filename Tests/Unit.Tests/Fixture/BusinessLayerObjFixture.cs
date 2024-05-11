using System.Data.Common;
using AutoMapper;
using Business.DTO.Entities;
using Data.Interface;
using Microsoft.EntityFrameworkCore.Storage;
using Tests.Test.Objects;

namespace Tests.Unit.Tests.Fixture
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

        public void SetUpSaveSequenceLaunchRepository()
        {
            LaunchRepository.SetupSequence(l => l.Save(new Launch(
                TestLaunchObjects.Test1(),
                TestLaunchObjects.Test1().IdStatus,
                TestLaunchObjects.Test1().IdLaunchServiceProvider,
                TestLaunchObjects.Test1().IdRocket,
                TestLaunchObjects.Test1().IdMission,
                TestLaunchObjects.Test1().IdPad)));

            LaunchRepository.SetupSequence(l => l.Save(new Launch(
                TestLaunchObjects.Test2(),
                TestLaunchObjects.Test2().IdStatus,
                TestLaunchObjects.Test2().IdLaunchServiceProvider,
                TestLaunchObjects.Test2().IdRocket,
                TestLaunchObjects.Test2().IdMission,
                TestLaunchObjects.Test2().IdPad)));

            LaunchRepository.SetupSequence(l => l.Save(new Launch(
                TestLaunchObjects.Test3(),
                TestLaunchObjects.Test3().IdStatus,
                TestLaunchObjects.Test3().IdLaunchServiceProvider,
                TestLaunchObjects.Test3().IdRocket,
                TestLaunchObjects.Test3().IdMission,
                TestLaunchObjects.Test3().IdPad)));
        }

        public void SetUpReturnGuidForDapper(bool emptyGuid)
        {
            DapperStatusRepository.SetupSequence(l => l.GetSelected<Guid>(
                "Id",
                "id_from_api = @IdFromApi",
                new { IdFromApi = It.IsAny<Guid>() },
                It.IsAny<DbConnection>(),
                It.IsAny<IDbContextTransaction>()))
                .ReturnsAsync(emptyGuid ? Guid.NewGuid() : (Guid)TestLaunchObjects.Test1().IdStatus)
                .ReturnsAsync(emptyGuid ? Guid.NewGuid() : (Guid)TestLaunchObjects.Test2().IdStatus)
                .ReturnsAsync(emptyGuid ? Guid.NewGuid() : (Guid)TestLaunchObjects.Test3().IdStatus);

            DapperLaunchServiceProviderRepository.SetupSequence(l => l.GetSelected<Guid>(
                "Id",
                "id_from_api = @IdFromApi",
                new { IdFromApi = It.IsAny<Guid>() },
                It.IsAny<DbConnection>(),
                It.IsAny<IDbContextTransaction>()))
                .ReturnsAsync(emptyGuid ? Guid.NewGuid() : (Guid)TestLaunchObjects.Test1().IdLaunchServiceProvider)
                .ReturnsAsync(emptyGuid ? Guid.NewGuid() : (Guid)TestLaunchObjects.Test2().IdLaunchServiceProvider)
                .ReturnsAsync(emptyGuid ? Guid.NewGuid() : (Guid)TestLaunchObjects.Test3().IdLaunchServiceProvider);

            DapperConfigurationRepository.SetupSequence(l => l.GetSelected<Guid>(
                "Id",
                "id_from_api = @IdFromApi",
                new { IdFromApi = It.IsAny<Guid>() },
                It.IsAny<DbConnection>(),
                It.IsAny<IDbContextTransaction>()))
                .ReturnsAsync(emptyGuid ? Guid.NewGuid() : (Guid)TestLaunchObjects.Test1().Rocket.IdConfiguration)
                .ReturnsAsync(emptyGuid ? Guid.NewGuid() : (Guid)TestLaunchObjects.Test2().Rocket.IdConfiguration)
                .ReturnsAsync(emptyGuid ? Guid.NewGuid() : (Guid)TestLaunchObjects.Test3().Rocket.IdConfiguration);

            DapperRocketRepository.SetupSequence(l => l.GetSelected<Guid>(
                "Id",
                "id_from_api = @IdFromApi",
                new { IdFromApi = It.IsAny<Guid>() },
                It.IsAny<DbConnection>(),
                It.IsAny<IDbContextTransaction>()))
                .ReturnsAsync(emptyGuid ? Guid.NewGuid() : TestLaunchObjects.Test1().Rocket.Id)
                .ReturnsAsync(emptyGuid ? Guid.NewGuid() : TestLaunchObjects.Test2().Rocket.Id)
                .ReturnsAsync(emptyGuid ? Guid.NewGuid() : TestLaunchObjects.Test3().Rocket.Id);

            DapperMissionRepository.SetupSequence(l => l.GetSelected<Guid>(
                "Id",
                "id_from_api = @IdFromApi",
                new { IdFromApi = It.IsAny<Guid>() },
                It.IsAny<DbConnection>(),
                It.IsAny<IDbContextTransaction>()))
                .ReturnsAsync(emptyGuid ? Guid.NewGuid() : (Guid)TestLaunchObjects.Test1().IdMission)
                .ReturnsAsync(emptyGuid ? Guid.NewGuid() : (Guid)TestLaunchObjects.Test2().IdMission)
                .ReturnsAsync(emptyGuid ? Guid.NewGuid() : (Guid)TestLaunchObjects.Test3().IdMission);

            DapperOrbitRepository.SetupSequence(l => l.GetSelected<Guid>(
                "Id",
                "id_from_api = @IdFromApi",
                new { IdFromApi = It.IsAny<Guid>() },
                It.IsAny<DbConnection>(),
                It.IsAny<IDbContextTransaction>()))
                .ReturnsAsync(emptyGuid ? Guid.NewGuid() : (Guid)TestLaunchObjects.Test1().Mission.IdOrbit)
                .ReturnsAsync(emptyGuid ? Guid.NewGuid() : (Guid)TestLaunchObjects.Test2().Mission.IdOrbit)
                .ReturnsAsync(emptyGuid ? Guid.NewGuid() : (Guid)TestLaunchObjects.Test3().Mission.IdOrbit);

            DapperPadRepository.SetupSequence(l => l.GetSelected<Guid>(
                "Id",
                "id_from_api = @IdFromApi",
                new { IdFromApi = It.IsAny<Guid>() },
                It.IsAny<DbConnection>(),
                It.IsAny<IDbContextTransaction>()))
                .ReturnsAsync(emptyGuid ? Guid.NewGuid() : TestLaunchObjects.Test1().Pad.Id)
                .ReturnsAsync(emptyGuid ? Guid.NewGuid() : TestLaunchObjects.Test2().Pad.Id)
                .ReturnsAsync(emptyGuid ? Guid.NewGuid() : TestLaunchObjects.Test3().Pad.Id);

            DapperLocationRepository.SetupSequence(l => l.GetSelected<Guid>(
                "Id",
                "id_from_api = @IdFromApi",
                new { IdFromApi = It.IsAny<Guid>() },
                It.IsAny<DbConnection>(),
                It.IsAny<IDbContextTransaction>()))
                .ReturnsAsync(emptyGuid ? Guid.NewGuid() : (Guid)TestLaunchObjects.Test1().Pad.IdLocation)
                .ReturnsAsync(emptyGuid ? Guid.NewGuid() : (Guid)TestLaunchObjects.Test2().Pad.IdLocation)
                .ReturnsAsync(emptyGuid ? Guid.NewGuid() : (Guid)TestLaunchObjects.Test3().Pad.IdLocation);
        }

        public void SetupDapperSave()
        {
            DapperStatusRepository.Setup(l => l.Save(It.IsAny<Status>(), It.IsAny<DbConnection>(), It.IsAny<IDbContextTransaction>()));
            DapperLaunchServiceProviderRepository.Setup(l => l.Save( It.IsAny<LaunchServiceProvider>(), It.IsAny<DbConnection>(), It.IsAny<IDbContextTransaction>()));
            DapperConfigurationRepository.Setup(l => l.Save( It.IsAny<Configuration>(), It.IsAny<DbConnection>(), It.IsAny<IDbContextTransaction>()));
            DapperRocketRepository.Setup(l => l.Save( It.IsAny<Rocket>(), It.IsAny<DbConnection>(), It.IsAny<IDbContextTransaction>()));
            DapperMissionRepository.Setup(l => l.Save( It.IsAny<Mission>(), It.IsAny<DbConnection>(), It.IsAny<IDbContextTransaction>()));
            DapperOrbitRepository.Setup(l => l.Save( It.IsAny<Orbit>(), It.IsAny<DbConnection>(), It.IsAny<IDbContextTransaction>()));
            DapperPadRepository.Setup(l => l.Save( It.IsAny<Pad>(), It.IsAny<DbConnection>(), It.IsAny<IDbContextTransaction>()));
            DapperLocationRepository.Setup(l => l.Save( It.IsAny<Location>(), It.IsAny<DbConnection>(), It.IsAny<IDbContextTransaction>()));
        }

        public void SetupDapperRecoveryBaseEntity()
        {
            DapperStatusRepository.SetupSequence(l => l.GetSelected<BaseEntityDTO>(
                "id as Id, id_from_api as IdFromApi, imported_t as ImportedT, status as Status",
                "id_from_api = @IdFromApi",
                It.IsAny<object>(),
                null,
                null))
                .ReturnsAsync(new BaseEntityDTO(TestLaunchObjects.Test1().Status.Id, TestLaunchObjects.Test1().Status.IdFromApi, TestLaunchObjects.Test1().Status.AtualizationDate, TestLaunchObjects.Test1().Status.ImportedT, TestLaunchObjects.Test1().Status.EntityStatus))
                .ReturnsAsync(new BaseEntityDTO(TestLaunchObjects.Test2().Status.Id, TestLaunchObjects.Test2().Status.IdFromApi, TestLaunchObjects.Test2().Status.AtualizationDate, TestLaunchObjects.Test2().Status.ImportedT, TestLaunchObjects.Test2().Status.EntityStatus))
                .ReturnsAsync(new BaseEntityDTO(TestLaunchObjects.Test3().Status.Id, TestLaunchObjects.Test3().Status.IdFromApi, TestLaunchObjects.Test3().Status.AtualizationDate, TestLaunchObjects.Test3().Status.ImportedT, TestLaunchObjects.Test3().Status.EntityStatus));

            DapperLaunchServiceProviderRepository.SetupSequence(l => l.GetSelected<BaseEntityDTO>(
                "id as Id, id_from_api as IdFromApi, imported_t as ImportedT, status as Status",
                "id_from_api = @IdFromApi",
                It.IsAny<object>(),
                null,
                null))
                .ReturnsAsync(new BaseEntityDTO(TestLaunchObjects.Test1().LaunchServiceProvider.Id, TestLaunchObjects.Test1().LaunchServiceProvider.IdFromApi, TestLaunchObjects.Test1().LaunchServiceProvider.AtualizationDate, TestLaunchObjects.Test1().LaunchServiceProvider.ImportedT, TestLaunchObjects.Test1().LaunchServiceProvider.EntityStatus))
                .ReturnsAsync(new BaseEntityDTO(TestLaunchObjects.Test2().LaunchServiceProvider.Id, TestLaunchObjects.Test2().LaunchServiceProvider.IdFromApi, TestLaunchObjects.Test2().LaunchServiceProvider.AtualizationDate, TestLaunchObjects.Test2().LaunchServiceProvider.ImportedT, TestLaunchObjects.Test2().LaunchServiceProvider.EntityStatus))
                .ReturnsAsync(new BaseEntityDTO(TestLaunchObjects.Test3().LaunchServiceProvider.Id, TestLaunchObjects.Test3().LaunchServiceProvider.IdFromApi, TestLaunchObjects.Test3().LaunchServiceProvider.AtualizationDate, TestLaunchObjects.Test3().LaunchServiceProvider.ImportedT, TestLaunchObjects.Test3().LaunchServiceProvider.EntityStatus));

            DapperConfigurationRepository.SetupSequence(l => l.GetSelected<BaseEntityDTO>(
                "id as Id, id_from_api as IdFromApi, imported_t as ImportedT, status as Status",
                "id_from_api = @IdFromApi",
                It.IsAny<object>(),
                null,
                null))
                .ReturnsAsync(new BaseEntityDTO(TestLaunchObjects.Test1().Rocket.Configuration.Id, TestLaunchObjects.Test1().Rocket.Configuration.IdFromApi, TestLaunchObjects.Test1().Rocket.Configuration.AtualizationDate, TestLaunchObjects.Test1().Rocket.Configuration.ImportedT, TestLaunchObjects.Test1().Rocket.Configuration.EntityStatus))
                .ReturnsAsync(new BaseEntityDTO(TestLaunchObjects.Test2().Rocket.Configuration.Id, TestLaunchObjects.Test2().Rocket.Configuration.IdFromApi, TestLaunchObjects.Test2().Rocket.Configuration.AtualizationDate, TestLaunchObjects.Test2().Rocket.Configuration.ImportedT, TestLaunchObjects.Test2().Rocket.Configuration.EntityStatus))
                .ReturnsAsync(new BaseEntityDTO(TestLaunchObjects.Test3().Rocket.Configuration.Id, TestLaunchObjects.Test3().Rocket.Configuration.IdFromApi, TestLaunchObjects.Test3().Rocket.Configuration.AtualizationDate, TestLaunchObjects.Test3().Rocket.Configuration.ImportedT, TestLaunchObjects.Test3().Rocket.Configuration.EntityStatus));

            DapperRocketRepository.SetupSequence(l => l.GetSelected<BaseEntityDTO>(
                "id as Id, id_from_api as IdFromApi, imported_t as ImportedT, status as Status",
                "id_from_api = @IdFromApi",
                It.IsAny<object>(),
                null,
                null))
                .ReturnsAsync(new BaseEntityDTO(TestLaunchObjects.Test1().Rocket.Id, TestLaunchObjects.Test1().Rocket.IdFromApi, TestLaunchObjects.Test1().Rocket.AtualizationDate, TestLaunchObjects.Test1().Rocket.ImportedT, TestLaunchObjects.Test1().Rocket.EntityStatus))
                .ReturnsAsync(new BaseEntityDTO(TestLaunchObjects.Test2().Rocket.Id, TestLaunchObjects.Test2().Rocket.IdFromApi, TestLaunchObjects.Test2().Rocket.AtualizationDate, TestLaunchObjects.Test2().Rocket.ImportedT, TestLaunchObjects.Test2().Rocket.EntityStatus))
                .ReturnsAsync(new BaseEntityDTO(TestLaunchObjects.Test3().Rocket.Id, TestLaunchObjects.Test3().Rocket.IdFromApi, TestLaunchObjects.Test3().Rocket.AtualizationDate, TestLaunchObjects.Test3().Rocket.ImportedT, TestLaunchObjects.Test3().Rocket.EntityStatus));

            DapperMissionRepository.SetupSequence(l => l.GetSelected<BaseEntityDTO>(
                "id as Id, id_from_api as IdFromApi, imported_t as ImportedT, status as Status",
                "id_from_api = @IdFromApi",
                It.IsAny<object>(),
                null,   
                null))
                .ReturnsAsync(new BaseEntityDTO(TestLaunchObjects.Test1().Mission.Id, TestLaunchObjects.Test1().Mission.IdFromApi, TestLaunchObjects.Test1().Mission.AtualizationDate, TestLaunchObjects.Test1().Mission.ImportedT, TestLaunchObjects.Test1().Mission.EntityStatus))
                .ReturnsAsync(new BaseEntityDTO(TestLaunchObjects.Test2().Mission.Id, TestLaunchObjects.Test2().Mission.IdFromApi, TestLaunchObjects.Test2().Mission.AtualizationDate, TestLaunchObjects.Test2().Mission.ImportedT, TestLaunchObjects.Test2().Mission.EntityStatus))
                .ReturnsAsync(new BaseEntityDTO(TestLaunchObjects.Test3().Mission.Id, TestLaunchObjects.Test3().Mission.IdFromApi, TestLaunchObjects.Test3().Mission.AtualizationDate, TestLaunchObjects.Test3().Mission.ImportedT, TestLaunchObjects.Test3().Mission.EntityStatus));

            DapperOrbitRepository.SetupSequence(l => l.GetSelected<BaseEntityDTO>(
                "id as Id, id_from_api as IdFromApi, imported_t as ImportedT, status as Status",
                "id_from_api = @IdFromApi",
                It.IsAny<object>(),
                null,
                null))
                .ReturnsAsync(new BaseEntityDTO(TestLaunchObjects.Test1().Mission.Orbit.Id, TestLaunchObjects.Test1().Mission.Orbit.IdFromApi, TestLaunchObjects.Test1().Mission.Orbit.AtualizationDate, TestLaunchObjects.Test1().Mission.Orbit.ImportedT, TestLaunchObjects.Test1().Mission.Orbit.EntityStatus))
                .ReturnsAsync(new BaseEntityDTO(TestLaunchObjects.Test2().Mission.Orbit.Id, TestLaunchObjects.Test2().Mission.Orbit.IdFromApi, TestLaunchObjects.Test2().Mission.Orbit.AtualizationDate, TestLaunchObjects.Test2().Mission.Orbit.ImportedT, TestLaunchObjects.Test2().Mission.Orbit.EntityStatus))
                .ReturnsAsync(new BaseEntityDTO(TestLaunchObjects.Test3().Mission.Orbit.Id, TestLaunchObjects.Test3().Mission.Orbit.IdFromApi, TestLaunchObjects.Test3().Mission.Orbit.AtualizationDate, TestLaunchObjects.Test3().Mission.Orbit.ImportedT, TestLaunchObjects.Test3().Mission.Orbit.EntityStatus));

            DapperPadRepository.SetupSequence(l => l.GetSelected<BaseEntityDTO>(
                "id as Id, id_from_api as IdFromApi, imported_t as ImportedT, status as Status",
                "id_from_api = @IdFromApi",
                It.IsAny<object>(),
                null,
                null))
                .ReturnsAsync(new BaseEntityDTO(TestLaunchObjects.Test1().Pad.Id, TestLaunchObjects.Test1().Pad.IdFromApi, TestLaunchObjects.Test1().Pad.AtualizationDate, TestLaunchObjects.Test1().Pad.ImportedT, TestLaunchObjects.Test1().Pad.EntityStatus))
                .ReturnsAsync(new BaseEntityDTO(TestLaunchObjects.Test2().Pad.Id, TestLaunchObjects.Test2().Pad.IdFromApi, TestLaunchObjects.Test2().Pad.AtualizationDate, TestLaunchObjects.Test2().Pad.ImportedT, TestLaunchObjects.Test2().Pad.EntityStatus))
                .ReturnsAsync(new BaseEntityDTO(TestLaunchObjects.Test3().Pad.Id, TestLaunchObjects.Test3().Pad.IdFromApi, TestLaunchObjects.Test3().Pad.AtualizationDate, TestLaunchObjects.Test3().Pad.ImportedT, TestLaunchObjects.Test3().Pad.EntityStatus));

            DapperLocationRepository.SetupSequence(l => l.GetSelected<BaseEntityDTO>(
                "id as Id, id_from_api as IdFromApi, imported_t as ImportedT, status as Status",
                "id_from_api = @IdFromApi",
                It.IsAny<object>(),
                null,
                null))
                .ReturnsAsync(new BaseEntityDTO(TestLaunchObjects.Test1().Pad.Location.Id, TestLaunchObjects.Test1().Pad.Location.IdFromApi, TestLaunchObjects.Test1().Pad.Location.AtualizationDate, TestLaunchObjects.Test1().Pad.Location.ImportedT, TestLaunchObjects.Test1().Pad.Location.EntityStatus))
                .ReturnsAsync(new BaseEntityDTO(TestLaunchObjects.Test2().Pad.Location.Id, TestLaunchObjects.Test2().Pad.Location.IdFromApi, TestLaunchObjects.Test2().Pad.Location.AtualizationDate, TestLaunchObjects.Test2().Pad.Location.ImportedT, TestLaunchObjects.Test2().Pad.Location.EntityStatus))
                .ReturnsAsync(new BaseEntityDTO(TestLaunchObjects.Test3().Pad.Location.Id, TestLaunchObjects.Test3().Pad.Location.IdFromApi, TestLaunchObjects.Test3().Pad.Location.AtualizationDate, TestLaunchObjects.Test3().Pad.Location.ImportedT, TestLaunchObjects.Test3().Pad.Location.EntityStatus));
        }

        public void ResetMockVerifyClasses()
        {
            LaunchRepository.Reset();
            Mapper.Reset();
            LaunchRepository.Reset();
            DapperStatusRepository.Reset();
            DapperLaunchServiceProviderRepository.Reset();
            DapperConfigurationRepository.Reset();
            DapperRocketRepository.Reset();
            DapperMissionRepository.Reset();
            DapperOrbitRepository.Reset();
            DapperLocationRepository.Reset();
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