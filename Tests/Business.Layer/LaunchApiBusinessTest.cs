using System.Linq.Expressions;
using System.Net;
using System.Net.Http.Json;
using AutoMapper;
using Business.Business;
using Business.DTO.Entities;
using Data.Interface;
using Data.Materializated.Views;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Storage;
using RichardSzalay.MockHttp;
using Tests.Test.Objects;
using Business.DTO.Request;
using Tests.Helper;

namespace Tests.Business.Layer
{
    public class LaunchApiBusinessTest
    {
        [Fact]
        public void LaunchApiBusiness_GetOneLaunch_ReturnOneLaunch()
        {
            //Arrange
            var launchApiBusiness = new Mock<ILaunchApiBusiness>();
            var launchViewRepository = new Mock<ILaunchViewRepository>();

            var uow = new Mock<IUnitOfWork>();
            var client = new Mock<IHttpClientFactory>();
            var mapper = new Mock<IMapper>();

            uow.Setup(u => u.Repository(typeof(ILaunchViewRepository))).Returns(launchViewRepository.Object);
            launchViewRepository.Setup(l => l.ViewExists()).ReturnsAsync(true);
            launchViewRepository.Setup(l => l.GetById(It.IsAny<Expression<Func<LaunchView, bool>>>())).ReturnsAsync(TestLaunchViewObjects.Test1());
            var business = new LaunchApiBusiness(uow.Object, client.Object, mapper.Object);

            //Act
            var result = business.GetOneLaunch(It.IsAny<Guid>()).Result;

            //Assert
            Assert.NotNull(result);
            Assert.IsType<LaunchView>(result);
            Assert.Equal(result.Id, TestLaunchViewObjects.Test1().Id);
        }

        [Fact]
        public void LaunchApiBusiness_GetOneLaunch_NullGuid()
        {
            //Arrange
            var uow = new Mock<IUnitOfWork>();
            var client = new Mock<IHttpClientFactory>();
            var mapper = new Mock<IMapper>();

            var business = new LaunchApiBusiness(uow.Object, client.Object, mapper.Object);

            //Act
            var result = business.GetOneLaunch(null);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<AggregateException>(result.Exception);
            Assert.Equal(result.Exception.Message, $"One or more errors occurred. (Value cannot be null. (Parameter '{ErrorMessages.NullArgument}'))");
        }

        [Fact]
        public void LaunchApiBusiness_GetOneLaunch_EmptyLaunchView()
        {
            //Arrange
            var launchApiBusiness = new Mock<ILaunchApiBusiness>();
            var launchViewRepository = new Mock<ILaunchViewRepository>();

            var uow = new Mock<IUnitOfWork>();
            var client = new Mock<IHttpClientFactory>();
            var mapper = new Mock<IMapper>();

            uow.Setup(u => u.Repository(typeof(ILaunchViewRepository))).Returns(launchViewRepository.Object);
            launchViewRepository.Setup(l => l.ViewExists()).ReturnsAsync(false);
            launchViewRepository.Setup(l => l.GetById(It.IsAny<Expression<Func<LaunchView, bool>>>())).ReturnsAsync(TestLaunchViewObjects.Test1());
            var business = new LaunchApiBusiness(uow.Object, client.Object, mapper.Object);

            //Act
            var result = business.GetOneLaunch(It.IsAny<Guid>());

            //Assert
            Assert.NotNull(result);
            Assert.IsType<AggregateException>(result.Exception);
            Assert.Equal(result.Exception.Message, $"One or more errors occurred. ({ErrorMessages.ViewNotExists})");
        }

        [Fact]
        public void LaunchApiBusiness_GetOneLaunch_NotFoundLaunch()
        {
            //Arrange
            var launchApiBusiness = new Mock<ILaunchApiBusiness>();
            var launchViewRepository = new Mock<ILaunchViewRepository>();

            var uow = new Mock<IUnitOfWork>();
            var client = new Mock<IHttpClientFactory>();
            var mapper = new Mock<IMapper>();

            uow.Setup(u => u.Repository(typeof(ILaunchViewRepository))).Returns(launchViewRepository.Object);
            launchViewRepository.Setup(l => l.ViewExists()).ReturnsAsync(true);
            launchViewRepository.Setup(l => l.GetById(It.IsAny<Expression<Func<LaunchView, bool>>>())).Returns(Task.FromResult((LaunchView)null));
            var business = new LaunchApiBusiness(uow.Object, client.Object, mapper.Object);

            //Act
            var result = business.GetOneLaunch(It.IsAny<Guid>());

            //Assert
            Assert.NotNull(result);
            Assert.IsType<AggregateException>(result.Exception);
            Assert.Equal(result.Exception.Message, $"One or more errors occurred. ({ErrorMessages.KeyNotFound})");
        }

        [Fact]
        public void LaunchApiBusiness_GetAllLaunchPaged_ReturnPagedLaunchs()
        {
            //Arrange
            var launchApiBusiness = new Mock<ILaunchApiBusiness>();
            var launchViewRepository = new Mock<ILaunchViewRepository>();

            var uow = new Mock<IUnitOfWork>();
            var client = new Mock<IHttpClientFactory>();
            var mapper = new Mock<IMapper>();

            uow.Setup(u => u.Repository(typeof(ILaunchViewRepository))).Returns(launchViewRepository.Object);
            launchViewRepository.Setup(l => l.EntityCount(It.IsAny<Expression<Func<LaunchView, bool>>>())).ReturnsAsync(3);
            launchViewRepository.Setup(l => l.GetViewPaged(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<List<Expression<Func<LaunchView, bool>>>>(), null))
                .ReturnsAsync(new Pagination<LaunchView>(){ CurrentPage = 0, NumberOfEntities = 3, NumberOfPages = 1, Entities = new List<LaunchView>() { TestLaunchViewObjects.Test1(), TestLaunchViewObjects.Test2(), TestLaunchViewObjects.Test3() } });
            var business = new LaunchApiBusiness(uow.Object, client.Object, mapper.Object);

            //Act
            var result = business.GetAllLaunchPaged(It.IsAny<int>()).Result;

            //Assert
            Assert.NotNull(result);
            Assert.IsType<Pagination<LaunchView>>(result);
            Assert.Equal(result.NumberOfEntities, 3);
            Assert.Equal(result.NumberOfPages, 1);
        }

        [Fact]
        public void LaunchApiBusiness_GetAllLaunchPaged_PageOverTotalPages()
        {
             //Arrange
            var launchApiBusiness = new Mock<ILaunchApiBusiness>();
            var launchViewRepository = new Mock<ILaunchViewRepository>();

            var uow = new Mock<IUnitOfWork>();
            var client = new Mock<IHttpClientFactory>();
            var mapper = new Mock<IMapper>();

            uow.Setup(u => u.Repository(typeof(ILaunchViewRepository))).Returns(launchViewRepository.Object);
            launchViewRepository.Setup(l => l.EntityCount(It.IsAny<Expression<Func<LaunchView, bool>>>())).ReturnsAsync(3);
            var business = new LaunchApiBusiness(uow.Object, client.Object, mapper.Object);

            //Act
            var result = business.GetAllLaunchPaged(5);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<AggregateException>(result.Exception);
            Assert.Equal(result.Exception.Message, $"One or more errors occurred. ({ErrorMessages.InvalidPageSelected} Total pages = 1)");
        }

        [Fact]
        public void LaunchApiBusiness_GetAllLaunchPaged_NoEntities()
        {
            //Arrange
            var launchApiBusiness = new Mock<ILaunchApiBusiness>();
            var launchViewRepository = new Mock<ILaunchViewRepository>();

            var uow = new Mock<IUnitOfWork>();
            var client = new Mock<IHttpClientFactory>();
            var mapper = new Mock<IMapper>();

            uow.Setup(u => u.Repository(typeof(ILaunchViewRepository))).Returns(launchViewRepository.Object);
            launchViewRepository.Setup(l => l.EntityCount(It.IsAny<Expression<Func<LaunchView, bool>>>())).ReturnsAsync(3);
            launchViewRepository.Setup(l => l.GetViewPaged(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<List<Expression<Func<LaunchView, bool>>>>(), null))
                .ReturnsAsync(new Pagination<LaunchView>(){ CurrentPage = 0, NumberOfEntities = 0, NumberOfPages = 1, Entities = new List<LaunchView>() });
            var business = new LaunchApiBusiness(uow.Object, client.Object, mapper.Object);

            //Act
            var result = business.GetAllLaunchPaged(It.IsAny<int>());

            //Assert
            Assert.NotNull(result);
            Assert.IsType<AggregateException>(result.Exception);
            Assert.Equal(result.Exception.Message, $"One or more errors occurred. ({ErrorMessages.NoData})");
        }

        [Fact]
        public void LaunchApiBusiness_SoftDeleteLaunch_SoftDeleteLaunch()
        {
            //Arrange
            var launchApiBusiness = new Mock<ILaunchApiBusiness>();
            var launchRepository = new Mock<ILaunchRepository>();
            var launchViewRepository = new Mock<ILaunchViewRepository>();
            var entity = TestLaunchObjects.Test1();

            var uow = new Mock<IUnitOfWork>();
            var client = new Mock<IHttpClientFactory>();
            var mapper = new Mock<IMapper>();

            uow.Setup(u => u.Repository(typeof(ILaunchRepository))).Returns(launchRepository.Object);
            launchRepository.Setup(l => l.EntityExist(It.IsAny<Expression<Func<Launch, bool>>>(), null)).ReturnsAsync(true);
            launchRepository.Setup(l => l.UpdateOnQuery(It.IsAny<List<Expression<Func<Launch, bool>>>>(), It.IsAny<Expression<Func<Launch, Launch>>>(), null));
            launchRepository.Setup(l => l.GetTransaction()).ReturnsAsync(Mock.Of<IDbContextTransaction>());

            uow.Setup(u => u.Repository(typeof(ILaunchViewRepository))).Returns(launchViewRepository.Object);
            launchViewRepository.Setup(l => l.RefreshView());

            var business = new LaunchApiBusiness(uow.Object, client.Object, mapper.Object);
            entity.EntityStatus = EStatus.TRASH.GetDisplayName();

            //Act
            var result = business.SoftDeleteLaunch(entity.Id);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(entity.EntityStatus, EStatus.TRASH.GetDisplayName());
        }

        [Fact]
        public void LaunchApiBusiness_SoftDeleteLaunch_NullGuid()
        {
            //Arrange
            var launchApiBusiness = new Mock<ILaunchApiBusiness>();
            var launchRepository = new Mock<ILaunchRepository>();
            var entity = TestLaunchObjects.Test1();

            var uow = new Mock<IUnitOfWork>();
            var client = new Mock<IHttpClientFactory>();
            var mapper = new Mock<IMapper>();

            uow.Setup(u => u.Repository(typeof(ILaunchRepository))).Returns(launchRepository.Object);
            var business = new LaunchApiBusiness(uow.Object, client.Object, mapper.Object);

            //Act
            var result = business.SoftDeleteLaunch(null);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<AggregateException>(result.Exception);
            Assert.Equal(result.Exception.Message, $"One or more errors occurred. (Value cannot be null. (Parameter '{ErrorMessages.NullArgument}'))");
        }

        [Fact]
        public void LaunchApiBusiness_SoftDeleteLaunch_LaunchNotExists()
        {
            //Arrange
            var launchApiBusiness = new Mock<ILaunchApiBusiness>();
            var launchRepository = new Mock<ILaunchRepository>();

            var uow = new Mock<IUnitOfWork>();
            var client = new Mock<IHttpClientFactory>();
            var mapper = new Mock<IMapper>();

            uow.Setup(u => u.Repository(typeof(ILaunchRepository))).Returns(launchRepository.Object);
            launchRepository.Setup(l => l.EntityExist(It.IsAny<Expression<Func<Launch, bool>>>(), null)).ReturnsAsync(false);
            var business = new LaunchApiBusiness(uow.Object, client.Object, mapper.Object);

            //Act
            var result = business.SoftDeleteLaunch(It.IsAny<Guid>());

            //Assert
            Assert.NotNull(result);
            Assert.IsType<AggregateException>(result.Exception);
            Assert.Equal(result.Exception.Message, $"One or more errors occurred. ({ErrorMessages.KeyNotFound})");
        }

        [Fact]
        public void LaunchApiBusiness_SoftDeleteLaunch_UpdateError()
        {
            //Arrange
            var launchApiBusiness = new Mock<ILaunchApiBusiness>();
            var launchRepository = new Mock<ILaunchRepository>();

            var uow = new Mock<IUnitOfWork>();
            var client = new Mock<IHttpClientFactory>();
            var mapper = new Mock<IMapper>();

            uow.Setup(u => u.Repository(typeof(ILaunchRepository))).Returns(launchRepository.Object);
            launchRepository.Setup(l => l.EntityExist(It.IsAny<Expression<Func<Launch, bool>>>(), null)).ReturnsAsync(true);
            launchRepository.Setup(l => l.UpdateOnQuery(It.IsAny<List<Expression<Func<Launch, bool>>>>(), It.IsAny<Expression<Func<Launch, Launch>>>(), null)).ThrowsAsync(new Exception());
            launchRepository.Setup(l => l.GetTransaction()).ReturnsAsync(Mock.Of<IDbContextTransaction>());

            var business = new LaunchApiBusiness(uow.Object, client.Object, mapper.Object);

            //Act
            var result = business.SoftDeleteLaunch(It.IsAny<Guid>());

            //Assert
            Assert.NotNull(result);
            Assert.IsType<AggregateException>(result.Exception);
            Assert.Equal(result.Exception.Message, "One or more errors occurred. (Exception of type 'System.Exception' was thrown.)");
        }

        [Fact]
        public async Task LaunchApiBusiness_UpdateLaunch_ReturnUpdatedLaunch()
        {
            //Arrange
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When($"{EndPoints.TheSpaceDevsLaunchEndPoint}*")
                .Respond("application/json", TestLaunchRequestObjects.Test1);

            var launchApiBusiness = new Mock<ILaunchApiBusiness>();
            var launchRepository = new Mock<ILaunchRepository>();
            var launchViewRepository = new Mock<ILaunchViewRepository>();
            var client = mockHttp.ToHttpClient();

            var uow = new Mock<IUnitOfWork>();
            var factoryClient = new Mock<IHttpClientFactory>();
            var mapper = new Mock<IMapper>();

            uow.Setup(u => u.Repository(typeof(ILaunchRepository))).Returns(launchRepository.Object);
            mapper.Setup(m => m.Map<Launch>(It.IsAny<LaunchDTO>())).Returns(TestLaunchObjects.Test1());

            factoryClient.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(() => client);
            launchRepository.Setup(l => l.Get(It.IsAny<Expression<Func<Launch, bool>>>(), string.Empty)).ReturnsAsync(TestLaunchObjects.Test1());
            launchRepository.Setup(l => l.GetTransaction()).ReturnsAsync(Mock.Of<IDbContextTransaction>());

            var response = await client.GetAsync($"{EndPoints.TheSpaceDevsLaunchEndPoint}{TestLaunchDTOObjects.Test1().Id}");
            var json = await response.Content.ReadFromJsonAsync<LaunchDTO>();
            uow.Setup(u =>u.Repository(typeof(ILaunchViewRepository))).Returns(launchViewRepository.Object);
            launchViewRepository.Setup(lv => lv.RefreshView());
            launchViewRepository.Setup(lv => lv.GetById(It.IsAny<Expression<Func<LaunchView, bool>>>())).ReturnsAsync(TestLaunchViewObjects.Test1());
            
            var business = new LaunchApiBusiness(uow.Object, factoryClient.Object, mapper.Object);

            //Act
            var result = business.UpdateLaunch(TestLaunchObjects.Test1().Id).Result;

            //Assert
            Assert.NotNull(result);
            Assert.IsType<LaunchView>(result);
            Assert.Equal(result.Id, TestLaunchViewObjects.Test1().Id);
        }

        [Fact]
        public void LaunchApiBusiness_UpdateLaunch_NullArgument()
        {
            //Arrange
            var uow = new Mock<IUnitOfWork>();
            var factoryClient = new Mock<IHttpClientFactory>();
            var mapper = new Mock<IMapper>();

            var business = new LaunchApiBusiness(uow.Object, factoryClient.Object, mapper.Object);
            
            //Act
            var result = business.UpdateLaunch(null);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<AggregateException>(result.Exception);
            Assert.Equal(result.Exception.Message, $"One or more errors occurred. (Value cannot be null. (Parameter '{ErrorMessages.NullArgument}'))");
        }

        [Fact]
        public void LaunchApiBusiness_UpdateLaunch_NotFoundLaunch()
        {
            //Arrange
            var launchRepository = new Mock<ILaunchRepository>();
            var uow = new Mock<IUnitOfWork>();
            var factoryClient = new Mock<IHttpClientFactory>();
            var mapper = new Mock<IMapper>();

            uow.Setup(u => u.Repository(typeof(ILaunchRepository))).Returns(launchRepository.Object);
            launchRepository.Setup(l => l.Get(It.IsAny<Expression<Func<Launch, bool>>>(), string.Empty)).ThrowsAsync(new KeyNotFoundException(ErrorMessages.KeyNotFound));
            var business = new LaunchApiBusiness(uow.Object, factoryClient.Object, mapper.Object);

            //Act
            var result = business.UpdateLaunch(TestLaunchObjects.Test1().Id);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<AggregateException>(result.Exception);
            Assert.Equal(result.Exception.Message, $"One or more errors occurred. ({ErrorMessages.KeyNotFound})");        
        }

        [Fact]
        public async Task LaunchApiBusiness_UpdateLaunch_NotSuccessStatusCode()
        {
            //Arrange
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When($"{EndPoints.TheSpaceDevsLaunchEndPoint}{TestLaunchObjects.Test1().ApiGuid}")
                .Respond(_ => { return new HttpResponseMessage(HttpStatusCode.TooManyRequests); 
            });
            
            var launchRepository = new Mock<ILaunchRepository>();
            var uow = new Mock<IUnitOfWork>();
            var factoryClient = new Mock<IHttpClientFactory>();
            var mapper = new Mock<IMapper>();
            var client = mockHttp.ToHttpClient();
            
            uow.Setup(u => u.Repository(typeof(ILaunchRepository))).Returns(launchRepository.Object);
            factoryClient.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(client);
            launchRepository.Setup(l => l.Get(It.IsAny<Expression<Func<Launch, bool>>>(), string.Empty)).ReturnsAsync(TestLaunchObjects.Test1());
            launchRepository.Setup(l => l.GetTransaction()).ReturnsAsync(Mock.Of<IDbContextTransaction>());
            var response = await client.GetAsync($"{EndPoints.TheSpaceDevsLaunchEndPoint}{TestLaunchObjects.Test1().ApiGuid}");

            var business = new LaunchApiBusiness(uow.Object, factoryClient.Object, mapper.Object);

            //Bcoz the async nature of the business method, this is will be catch awaiting the UpdateLaunch method.
            //Act & Assert
            await Assert.ThrowsAsync<HttpRequestException>(async () => await business.UpdateLaunch(TestLaunchObjects.Test1().Id));
        }

        [Fact]
        public async Task LaunchApiBusiness_UpdateLaunch_DeserializationFailed()
        {
            //Arrange
            const string strangeObject = """{ "Sku": "Strange Object" }""";

            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When($"{EndPoints.TheSpaceDevsLaunchEndPoint}*")
                .Respond("application/json", strangeObject);

            var launchApiBusiness = new Mock<ILaunchApiBusiness>();
            var launchRepository = new Mock<ILaunchRepository>();
            var launchViewRepository = new Mock<ILaunchViewRepository>();
            var client = mockHttp.ToHttpClient();

            var uow = new Mock<IUnitOfWork>();
            var factoryClient = new Mock<IHttpClientFactory>();
            var mapper = new Mock<IMapper>();

            uow.Setup(u => u.Repository(typeof(ILaunchRepository))).Returns(launchRepository.Object);
            mapper.Setup(m => m.Map<Launch>(It.IsAny<LaunchDTO>())).Returns(TestLaunchObjects.Test1());

            factoryClient.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(() => client);
            launchRepository.Setup(l => l.Get(It.IsAny<Expression<Func<Launch, bool>>>(), string.Empty)).ReturnsAsync(TestLaunchObjects.Test1());
            launchRepository.Setup(l => l.GetTransaction()).ReturnsAsync(Mock.Of<IDbContextTransaction>());

            var response = await client.GetAsync($"{EndPoints.TheSpaceDevsLaunchEndPoint}{TestLaunchDTOObjects.Test1().Id}");
            var json = await response.Content.ReadFromJsonAsync<LaunchDTO>();
            
            //Act
            var business = new LaunchApiBusiness(uow.Object, factoryClient.Object, mapper.Object);

            //Assert
            await Assert.ThrowsAsync<System.Text.Json.JsonException>(async () => await business.UpdateLaunch(TestLaunchObjects.Test1().Id));
        }

        [Fact]
        public async Task LaunchApiBusiness_UpdateDataSet_SuccesfullUpdate()
        {
            //Arrange
            var request = new UpdateLaunchRequest(){ Limit = 3, Iterations = 1, Skip = 0 };
            int offset = 0;
            string url = $"{EndPoints.TheSpaceDevsLaunchEndPoint}?limit={request.Limit}&offset={offset}";
            string results = TestLaunchRequestObjects.DataListResults;
            
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When(url).Respond("application/json", results);

            var launchApiBusiness = new Mock<ILaunchApiBusiness>();

            var updateLogRepository = new Mock<IUpdateLogRepository>();
            var launchRepository = new Mock<ILaunchRepository>();
            var dapperStatusRepository = new Mock<IGenericDapperRepository<Status>>();
            var dapperLaunchServiceProviderRepository = new Mock<IGenericDapperRepository<LaunchServiceProvider>>();
            var dapperConfigurationRepository = new Mock<IGenericDapperRepository<Configuration>>();
            var dapperRocketRepository = new Mock<IGenericDapperRepository<Rocket>>();
            var dapperMissionRepository = new Mock<IGenericDapperRepository<Mission>>();
            var dapperOrbitRepository = new Mock<IGenericDapperRepository<Orbit>>();
            var dapperPadRepository = new Mock<IGenericDapperRepository<Pad>>();
            var dapperLocationRepository = new Mock<IGenericDapperRepository<Location>>();
            var launchViewRepository = new Mock<ILaunchViewRepository>();

            var client = mockHttp.ToHttpClient();

            var uow = new Mock<IUnitOfWork>();
            var factoryClient = new Mock<IHttpClientFactory>();
            var mapper = new Mock<IMapper>();

            uow.Setup(u => u.Repository(typeof(ILaunchRepository))).Returns(launchRepository.Object);
            uow.Setup(u => u.Dapper<Status>()).Returns(dapperStatusRepository.Object);
            uow.Setup(u => u.Dapper<LaunchServiceProvider>()).Returns(dapperLaunchServiceProviderRepository.Object);
            uow.Setup(u => u.Dapper<Configuration>()).Returns(dapperConfigurationRepository.Object);
            uow.Setup(u => u.Dapper<Rocket>()).Returns(dapperRocketRepository.Object);
            uow.Setup(u => u.Dapper<Mission>()).Returns(dapperMissionRepository.Object);
            uow.Setup(u => u.Dapper<Orbit>()).Returns(dapperOrbitRepository.Object);
            uow.Setup(u => u.Dapper<Pad>()).Returns(dapperPadRepository.Object);
            uow.Setup(u => u.Dapper<Location>()).Returns(dapperLocationRepository.Object);
            uow.Setup(u => u.Repository(typeof(IUpdateLogRepository))).Returns(updateLogRepository.Object);
            uow.Setup(u => u.Repository(typeof(ILaunchViewRepository))).Returns(launchViewRepository.Object);

            launchRepository.SetupSequence(l => l.EntityExist(
                It.IsAny<Expression<Func<Launch, bool>>>(),
                string.Empty))
                .ReturnsAsync(false)
                .ReturnsAsync(false)
                .ReturnsAsync(false);

            launchRepository.SetupSequence(l => l.SaveTransaction(new Launch(
                TestLaunchObjects.Test1(),
                TestLaunchObjects.Test1().IdStatus,
                TestLaunchObjects.Test1().IdLaunchServiceProvider,
                TestLaunchObjects.Test1().IdRocket,
                TestLaunchObjects.Test1().IdMission,
                TestLaunchObjects.Test1().IdPad)));

            launchRepository.SetupSequence(l => l.SaveTransaction(new Launch(
                TestLaunchObjects.Test2(),
                TestLaunchObjects.Test2().IdStatus,
                TestLaunchObjects.Test2().IdLaunchServiceProvider,
                TestLaunchObjects.Test2().IdRocket,
                TestLaunchObjects.Test2().IdMission,
                TestLaunchObjects.Test2().IdPad)));

            launchRepository.SetupSequence(l => l.SaveTransaction(new Launch(
                TestLaunchObjects.Test3(),
                TestLaunchObjects.Test3().IdStatus,
                TestLaunchObjects.Test3().IdLaunchServiceProvider,
                TestLaunchObjects.Test3().IdRocket,
                TestLaunchObjects.Test3().IdMission,
                TestLaunchObjects.Test3().IdPad)));

            dapperStatusRepository.SetupSequence(l => l.GetSelected<Guid>(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<object>(),
                It.IsAny<IDbContextTransaction>()))
                .ReturnsAsync((Guid)TestLaunchObjects.Test1().IdStatus)
                .ReturnsAsync((Guid)TestLaunchObjects.Test2().IdStatus)
                .ReturnsAsync((Guid)TestLaunchObjects.Test3().IdStatus);

            dapperLaunchServiceProviderRepository.SetupSequence(l => l.GetSelected<Guid>(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<object>(),
                It.IsAny<IDbContextTransaction>()))
                .ReturnsAsync((Guid)TestLaunchObjects.Test1().IdLaunchServiceProvider)
                .ReturnsAsync((Guid)TestLaunchObjects.Test2().IdLaunchServiceProvider)
                .ReturnsAsync((Guid)TestLaunchObjects.Test3().IdLaunchServiceProvider);

            dapperConfigurationRepository.SetupSequence(l => l.GetSelected<Guid>(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<object>(),
                It.IsAny<IDbContextTransaction>()))
                .ReturnsAsync((Guid)TestLaunchObjects.Test1().Rocket.IdConfiguration)
                .ReturnsAsync((Guid)TestLaunchObjects.Test2().Rocket.IdConfiguration)
                .ReturnsAsync((Guid)TestLaunchObjects.Test3().Rocket.IdConfiguration);

            dapperRocketRepository.SetupSequence(l => l.GetSelected<Guid>(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<object>(),
                It.IsAny<IDbContextTransaction>()))
                .ReturnsAsync(TestLaunchObjects.Test1().Rocket.Id)
                .ReturnsAsync(TestLaunchObjects.Test2().Rocket.Id)
                .ReturnsAsync(TestLaunchObjects.Test3().Rocket.Id);

            dapperMissionRepository.SetupSequence(l => l.GetSelected<Guid>(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<object>(),
                It.IsAny<IDbContextTransaction>()))
                .ReturnsAsync((Guid)TestLaunchObjects.Test1().IdMission)
                .ReturnsAsync((Guid)TestLaunchObjects.Test2().IdMission)
                .ReturnsAsync((Guid)TestLaunchObjects.Test3().IdMission);

            dapperOrbitRepository.SetupSequence(l => l.GetSelected<Guid>(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<object>(),
                It.IsAny<IDbContextTransaction>()))
                .ReturnsAsync((Guid)TestLaunchObjects.Test1().Mission.IdOrbit)
                .ReturnsAsync((Guid)TestLaunchObjects.Test2().Mission.IdOrbit)
                .ReturnsAsync((Guid)TestLaunchObjects.Test3().Mission.IdOrbit);

            dapperPadRepository.SetupSequence(l => l.GetSelected<Guid>(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<object>(),
                It.IsAny<IDbContextTransaction>()))
                .ReturnsAsync(TestLaunchObjects.Test1().Pad.Id)
                .ReturnsAsync(TestLaunchObjects.Test2().Pad.Id)
                .ReturnsAsync(TestLaunchObjects.Test3().Pad.Id);

            dapperPadRepository.SetupSequence(l => l.GetSelected<Guid>(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<object>(),
                It.IsAny<IDbContextTransaction>()))
                .ReturnsAsync((Guid)TestLaunchObjects.Test1().Pad.IdLocation)
                .ReturnsAsync((Guid)TestLaunchObjects.Test2().Pad.IdLocation)
                .ReturnsAsync((Guid)TestLaunchObjects.Test3().Pad.IdLocation);

            mapper.SetupSequence(m => m.Map<Launch>(It.IsAny<LaunchDTO>()))
                .Returns(TestLaunchObjects.Test1())
                .Returns(TestLaunchObjects.Test2())
                .Returns(TestLaunchObjects.Test3());

            launchViewRepository.Setup(lv => lv.RefreshView());
            factoryClient.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(() => client);
            launchRepository.Setup(l => l.GetTransaction()).ReturnsAsync(Mock.Of<IDbContextTransaction>());
            var response = await client.GetAsync(url);
            var json = await response.Content.ReadFromJsonAsync<RequestLaunchDTO>();

            var business = new LaunchApiBusiness(uow.Object, factoryClient.Object, mapper.Object);

            //Act
            var result = business.UpdateDataSet(request).Result;

            //Assert
            Assert.NotNull(result);
            Assert.IsType<bool>(result);
            Assert.Equal(result, true);
        }
    
        [Fact]
        public void LaunchApiBusiness_UpdateDataSet_ValidationRequestObj()
        {
            //Arrange
            var request = new UpdateLaunchRequest(){ Limit = 500, Iterations = 20, Skip = 0 };

            //Act
            var errorCount = ValidationHelper.ValidateObject(ref request);

            //Assert
            Assert.True(errorCount.Count == 2);
            Assert.Equal(errorCount[0].ErrorMessage, "The value on the field Limit must be greater than 0 and less 100.");
            Assert.Equal(errorCount[1].ErrorMessage, "The value on the field Iterations must be greater than 0 and less 15.");
        }

        [Fact]
        public async Task LaunchApiBusiness_UpdateDataSet_HttpError()
        {
            //Arrange
            var request = new UpdateLaunchRequest(){ Limit = 3, Iterations = 1, Skip = 0 };
            int offset = 0;
            string url = $"{EndPoints.TheSpaceDevsLaunchEndPoint}?limit={request.Limit}&offset={offset}";

            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When(url).Respond(_ => { return new HttpResponseMessage(HttpStatusCode.TooManyRequests); });

            var uow = new Mock<IUnitOfWork>();
            var factoryClient = new Mock<IHttpClientFactory>();
            var mapper = new Mock<IMapper>();
            var updateLogRepository = new Mock<IUpdateLogRepository>();
            var launchViewRepository = new Mock<ILaunchViewRepository>();

            var client = mockHttp.ToHttpClient();

            uow.Setup(u => u.Repository(typeof(IUpdateLogRepository))).Returns(updateLogRepository.Object);
            uow.Setup(u => u.Repository(typeof(ILaunchViewRepository))).Returns(launchViewRepository.Object);
            factoryClient.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(client);
            var response = await client.GetAsync(url);

            var business = new LaunchApiBusiness(uow.Object, factoryClient.Object, mapper.Object);
            
            //Act & Assert
            await Assert.ThrowsAsync<HttpRequestException>(async () => await business.UpdateDataSet(request));
        }

        [Fact]
        public async Task LaunchApiBusiness_UpdateDataSet_EmptyEntities()
        {
            //Arrange
            var request = new UpdateLaunchRequest(){ Limit = 3, Iterations = 1, Skip = 0 };
            int offset = 0;
            string url = $"{EndPoints.TheSpaceDevsLaunchEndPoint}?limit={request.Limit}&offset={offset}";
            string results = TestLaunchRequestObjects.EmptyDataListResults;

            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When(url).Respond("application/json", results);

            var uow = new Mock<IUnitOfWork>();
            var factoryClient = new Mock<IHttpClientFactory>();
            var mapper = new Mock<IMapper>();
            var updateLogRepository = new Mock<IUpdateLogRepository>();
            var launchViewRepository = new Mock<ILaunchViewRepository>();

            var client = mockHttp.ToHttpClient();

            uow.Setup(u => u.Repository(typeof(IUpdateLogRepository))).Returns(updateLogRepository.Object);
            uow.Setup(u => u.Repository(typeof(ILaunchViewRepository))).Returns(launchViewRepository.Object);
            factoryClient.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(client);
            mapper.Setup(m => m.Map<RequestLaunchDTO>(It.IsAny<LaunchDTO>()))
                .Returns(new RequestLaunchDTO() { Count = 0, Next = null, Previous = null, Results = new List<LaunchDTO>() });
            
            var response = await client.GetAsync(url);
            var business = new LaunchApiBusiness(uow.Object, factoryClient.Object, mapper.Object);

            //Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(async () => await business.UpdateDataSet(request));
        }

        [Fact]
        public async Task LaunchApiBusiness_SearchByParam_FoundByParam()
        {
            //Arrange
            var request = new SearchLaunchRequest(){ Mission = "Pioneer" };
            var pagination = new Pagination<LaunchView>(){ NumberOfEntities = 1, CurrentPage = 0, NumberOfPages = 1, Entities = new List<LaunchView>() { TestLaunchViewObjects.Test2() } };

            var missionRepository = new Mock<IMissionRepository>();
            var launchViewRepository = new Mock<ILaunchViewRepository>();

            var uow = new Mock<IUnitOfWork>();
            var factoryClient = new Mock<IHttpClientFactory>();
            var mapper = new Mock<IMapper>();

            uow.Setup(u => u.Repository(typeof(IMissionRepository))).Returns(missionRepository.Object);
            uow.Setup(u => u.Repository(typeof(ILaunchViewRepository))).Returns(launchViewRepository.Object);
            
            launchViewRepository.Setup(l => l.GetViewPaged(0, 10, It.IsAny<List<Expression<Func<LaunchView, bool>>>>(), null))
                .ReturnsAsync(pagination);
            missionRepository.Setup(m => m.ILikeSearch(request.Mission, It.IsAny<Expression<Func<Mission, Guid>>>(), null))
                .ReturnsAsync(new List<Guid>(){ (Guid)TestLaunchObjects.Test2().IdMission });

            var business = new LaunchApiBusiness(uow.Object, factoryClient.Object, mapper.Object);

            //Act
            var result = business.SearchByParam(request).Result;

            //Assert
            Assert.NotNull(result);
            Assert.IsType<Pagination<LaunchView>>(result);
            Assert.Equal(result, pagination);
        }

        [Fact]
        public async Task LaunchApiBusiness_SearchByParam_EmptyRequest()
        {
            //Arrange
            var request = new SearchLaunchRequest();

            var uow = new Mock<IUnitOfWork>();
            var factoryClient = new Mock<IHttpClientFactory>();
            var mapper = new Mock<IMapper>();

            var business = new LaunchApiBusiness(uow.Object, factoryClient.Object, mapper.Object);

            //Act
            var result = business.SearchByParam(request);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<AggregateException>(result.Exception);
            Assert.Equal(result.Exception.Message, "One or more errors occurred. (Attention! The requested data was not found.)");
        }

        [Fact]
        public async Task LaunchApiBusiness_SearchByParam_NotFoundEntry()
        {
            //Arrange
            var request = new SearchLaunchRequest(){ Mission = "Mission that not exists" };
            var pagination = new Pagination<LaunchView>(){ NumberOfEntities = 0, CurrentPage = 0, NumberOfPages = 1, Entities = new List<LaunchView>() };

            var missionRepository = new Mock<IMissionRepository>();
            var launchViewRepository = new Mock<ILaunchViewRepository>();

            var uow = new Mock<IUnitOfWork>();
            var factoryClient = new Mock<IHttpClientFactory>();
            var mapper = new Mock<IMapper>();

            uow.Setup(u => u.Repository(typeof(IMissionRepository))).Returns(missionRepository.Object);
            uow.Setup(u => u.Repository(typeof(ILaunchViewRepository))).Returns(launchViewRepository.Object);
            
            launchViewRepository.Setup(l => l.GetViewPaged(0, 10, It.IsAny<List<Expression<Func<LaunchView, bool>>>>(), null))
                .ReturnsAsync(pagination);
            missionRepository.Setup(m => m.ILikeSearch(request.Mission, It.IsAny<Expression<Func<Mission, Guid>>>(), null))
                .ReturnsAsync(new List<Guid>(){ (Guid)TestLaunchObjects.Test2().IdMission });

            var business = new LaunchApiBusiness(uow.Object, factoryClient.Object, mapper.Object);

            //Act
            var result = business.SearchByParam(request);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<AggregateException>(result.Exception);
            Assert.Equal(result.Exception.Message, "One or more errors occurred. (Attention! The requested data was not found.)");
        }
    }
}
