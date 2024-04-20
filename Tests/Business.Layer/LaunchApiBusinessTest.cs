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
using Tests.Fixture;

namespace Tests.Business.Layer
{
    public class LaunchApiBusinessTest : IClassFixture<BusinessLayerObjFixture>
    {
        private readonly BusinessLayerObjFixture _fixture;
        public LaunchApiBusinessTest(BusinessLayerObjFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void LaunchApiBusiness_GetOneLaunch_ReturnOneLaunch()
        {
            //Arrange
            _fixture.Uow.Setup(u => u.Repository(typeof(ILaunchViewRepository))).Returns(_fixture.LaunchViewRepository.Object);
            _fixture.LaunchViewRepository.Setup(l => l.ViewExists()).ReturnsAsync(true);
            _fixture.LaunchViewRepository.Setup(l => l.GetById(It.IsAny<Expression<Func<LaunchView, bool>>>())).ReturnsAsync(TestLaunchViewObjects.Test1());
            var business = new LaunchApiBusiness(_fixture.Uow.Object, _fixture.Client.Object, _fixture.Mapper.Object);

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
            var business = new LaunchApiBusiness(_fixture.Uow.Object, _fixture.Client.Object, _fixture.Mapper.Object);

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
            _fixture.Uow.Setup(u => u.Repository(typeof(ILaunchViewRepository))).Returns(_fixture.LaunchViewRepository.Object);
            _fixture.LaunchViewRepository.Setup(l => l.ViewExists()).ReturnsAsync(false);
            _fixture.LaunchViewRepository.Setup(l => l.GetById(It.IsAny<Expression<Func<LaunchView, bool>>>())).ReturnsAsync(TestLaunchViewObjects.Test1());
            var business = new LaunchApiBusiness(_fixture.Uow.Object, _fixture.Client.Object, _fixture.Mapper.Object);

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
            _fixture.Uow.Setup(u => u.Repository(typeof(ILaunchViewRepository))).Returns(_fixture.LaunchViewRepository.Object);
            _fixture.LaunchViewRepository.Setup(l => l.ViewExists()).ReturnsAsync(true);
            _fixture.LaunchViewRepository.Setup(l => l.GetById(It.IsAny<Expression<Func<LaunchView, bool>>>())).Returns(Task.FromResult((LaunchView)null));
            var business = new LaunchApiBusiness(_fixture.Uow.Object, _fixture.Client.Object, _fixture.Mapper.Object);

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
            _fixture.Uow.Setup(u => u.Repository(typeof(ILaunchViewRepository))).Returns(_fixture.LaunchViewRepository.Object);
            _fixture.LaunchViewRepository.Setup(l => l.EntityCount(It.IsAny<Expression<Func<LaunchView, bool>>>())).ReturnsAsync(3);
            _fixture.LaunchViewRepository.Setup(l => l.GetViewPaged(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<List<Expression<Func<LaunchView, bool>>>>(), null))
                .ReturnsAsync(new Pagination<LaunchView>(){ CurrentPage = 0, NumberOfEntities = 3, NumberOfPages = 1, Entities = new List<LaunchView>() { TestLaunchViewObjects.Test1(), TestLaunchViewObjects.Test2(), TestLaunchViewObjects.Test3() } });
            var business = new LaunchApiBusiness(_fixture.Uow.Object, _fixture.Client.Object, _fixture.Mapper.Object);

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
            _fixture.Uow.Setup(u => u.Repository(typeof(ILaunchViewRepository))).Returns(_fixture.LaunchViewRepository.Object);
            _fixture.LaunchViewRepository.Setup(l => l.EntityCount(It.IsAny<Expression<Func<LaunchView, bool>>>())).ReturnsAsync(3);
            var business = new LaunchApiBusiness(_fixture.Uow.Object, _fixture.Client.Object, _fixture.Mapper.Object);

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
            _fixture.Uow.Setup(u => u.Repository(typeof(ILaunchViewRepository))).Returns(_fixture.LaunchViewRepository.Object);
            _fixture.LaunchViewRepository.Setup(l => l.EntityCount(It.IsAny<Expression<Func<LaunchView, bool>>>())).ReturnsAsync(3);
            _fixture.LaunchViewRepository.Setup(l => l.GetViewPaged(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<List<Expression<Func<LaunchView, bool>>>>(), null))
                .ReturnsAsync(new Pagination<LaunchView>(){ CurrentPage = 0, NumberOfEntities = 0, NumberOfPages = 1, Entities = new List<LaunchView>() });
            var business = new LaunchApiBusiness(_fixture.Uow.Object, _fixture.Client.Object, _fixture.Mapper.Object);

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
            var entity = TestLaunchObjects.Test1();

            _fixture.Uow.Setup(u => u.Repository(typeof(ILaunchRepository))).Returns(_fixture.LaunchRepository.Object);
            _fixture.LaunchRepository.Setup(l => l.EntityExist(It.IsAny<Expression<Func<Launch, bool>>>(), null)).ReturnsAsync(true);
            _fixture.LaunchRepository.Setup(l => l.UpdateOnQuery(It.IsAny<List<Expression<Func<Launch, bool>>>>(), It.IsAny<Expression<Func<Launch, Launch>>>(), null));
            _fixture.LaunchRepository.Setup(l => l.GetTransaction()).ReturnsAsync(Mock.Of<IDbContextTransaction>());

            _fixture.Uow.Setup(u => u.Repository(typeof(ILaunchViewRepository))).Returns(_fixture.LaunchViewRepository.Object);
            _fixture.LaunchViewRepository.Setup(l => l.RefreshView());

            var business = new LaunchApiBusiness(_fixture.Uow.Object, _fixture.Client.Object, _fixture.Mapper.Object);
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
            var entity = TestLaunchObjects.Test1();

            _fixture.Uow.Setup(u => u.Repository(typeof(ILaunchRepository))).Returns(_fixture.LaunchRepository.Object);
            var business = new LaunchApiBusiness(_fixture.Uow.Object, _fixture.Client.Object, _fixture.Mapper.Object);

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
            _fixture.Uow.Setup(u => u.Repository(typeof(ILaunchRepository))).Returns(_fixture.LaunchRepository.Object);
            _fixture.LaunchRepository.Setup(l => l.EntityExist(It.IsAny<Expression<Func<Launch, bool>>>(), null)).ReturnsAsync(false);
            var business = new LaunchApiBusiness(_fixture.Uow.Object, _fixture.Client.Object, _fixture.Mapper.Object);

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
            _fixture.Uow.Setup(u => u.Repository(typeof(ILaunchRepository))).Returns(_fixture.LaunchRepository.Object);
            _fixture.LaunchRepository.Setup(l => l.EntityExist(It.IsAny<Expression<Func<Launch, bool>>>(), null)).ReturnsAsync(true);
            _fixture.LaunchRepository.Setup(l => l.UpdateOnQuery(It.IsAny<List<Expression<Func<Launch, bool>>>>(), It.IsAny<Expression<Func<Launch, Launch>>>(), null)).ThrowsAsync(new Exception());
            _fixture.LaunchRepository.Setup(l => l.GetTransaction()).ReturnsAsync(Mock.Of<IDbContextTransaction>());

            var business = new LaunchApiBusiness(_fixture.Uow.Object, _fixture.Client.Object, _fixture.Mapper.Object);

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

            var client = mockHttp.ToHttpClient();
            _fixture.Uow.Setup(u => u.Repository(typeof(ILaunchRepository))).Returns(_fixture.LaunchRepository.Object);
            _fixture.Mapper.Setup(m => m.Map<Launch>(It.IsAny<LaunchDTO>())).Returns(TestLaunchObjects.Test1());

            _fixture.Client.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(() => client);
            _fixture.LaunchRepository.Setup(l => l.Get(It.IsAny<Expression<Func<Launch, bool>>>(), string.Empty)).ReturnsAsync(TestLaunchObjects.Test1());
            _fixture.LaunchRepository.Setup(l => l.GetTransaction()).ReturnsAsync(Mock.Of<IDbContextTransaction>());

            var response = await client.GetAsync($"{EndPoints.TheSpaceDevsLaunchEndPoint}{TestLaunchDTOObjects.Test1().Id}");
            var json = await response.Content.ReadFromJsonAsync<LaunchDTO>();
            _fixture.Uow.Setup(u =>u.Repository(typeof(ILaunchViewRepository))).Returns(_fixture.LaunchViewRepository.Object);
            _fixture.LaunchViewRepository.Setup(lv => lv.RefreshView());
            _fixture.LaunchViewRepository.Setup(lv => lv.GetById(It.IsAny<Expression<Func<LaunchView, bool>>>())).ReturnsAsync(TestLaunchViewObjects.Test1());
            
            var business = new LaunchApiBusiness(_fixture.Uow.Object, _fixture.Client.Object, _fixture.Mapper.Object);

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
            var business = new LaunchApiBusiness(_fixture.Uow.Object, _fixture.Client.Object, _fixture.Mapper.Object);
            
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
            _fixture.Uow.Setup(u => u.Repository(typeof(ILaunchRepository))).Returns(_fixture.LaunchRepository.Object);
            _fixture.LaunchRepository.Setup(l => l.Get(It.IsAny<Expression<Func<Launch, bool>>>(), string.Empty)).ThrowsAsync(new KeyNotFoundException(ErrorMessages.KeyNotFound));
            var business = new LaunchApiBusiness(_fixture.Uow.Object, _fixture.Client.Object, _fixture.Mapper.Object);

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
            
            var client = mockHttp.ToHttpClient();
            
            _fixture.Uow.Setup(u => u.Repository(typeof(ILaunchRepository))).Returns(_fixture.LaunchRepository.Object);
            _fixture.Client.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(client);
            _fixture.LaunchRepository.Setup(l => l.Get(It.IsAny<Expression<Func<Launch, bool>>>(), string.Empty)).ReturnsAsync(TestLaunchObjects.Test1());
            _fixture.LaunchRepository.Setup(l => l.GetTransaction()).ReturnsAsync(Mock.Of<IDbContextTransaction>());
            var response = await client.GetAsync($"{EndPoints.TheSpaceDevsLaunchEndPoint}{TestLaunchObjects.Test1().ApiGuid}");

            var business = new LaunchApiBusiness(_fixture.Uow.Object, _fixture.Client.Object, _fixture.Mapper.Object);

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

            var client = mockHttp.ToHttpClient();

            _fixture.Uow.Setup(u => u.Repository(typeof(ILaunchRepository))).Returns(_fixture.LaunchRepository.Object);
            _fixture.Mapper.Setup(m => m.Map<Launch>(It.IsAny<LaunchDTO>())).Returns(TestLaunchObjects.Test1());

            _fixture.Client.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(() => client);
            _fixture.LaunchRepository.Setup(l => l.Get(It.IsAny<Expression<Func<Launch, bool>>>(), string.Empty)).ReturnsAsync(TestLaunchObjects.Test1());
            _fixture.LaunchRepository.Setup(l => l.GetTransaction()).ReturnsAsync(Mock.Of<IDbContextTransaction>());

            var response = await client.GetAsync($"{EndPoints.TheSpaceDevsLaunchEndPoint}{TestLaunchDTOObjects.Test1().Id}");
            var json = await response.Content.ReadFromJsonAsync<LaunchDTO>();
            
            //Act
            var business = new LaunchApiBusiness(_fixture.Uow.Object, _fixture.Client.Object, _fixture.Mapper.Object);

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

            var client = mockHttp.ToHttpClient();

            _fixture.Uow.Setup(u => u.Repository(typeof(ILaunchRepository))).Returns(_fixture.LaunchRepository.Object);
            _fixture.Uow.Setup(u => u.Dapper<Status>()).Returns(_fixture.DapperStatusRepository.Object);
            _fixture.Uow.Setup(u => u.Dapper<LaunchServiceProvider>()).Returns(_fixture.DapperLaunchServiceProviderRepository.Object);
            _fixture.Uow.Setup(u => u.Dapper<Configuration>()).Returns(_fixture.DapperConfigurationRepository.Object);
            _fixture.Uow.Setup(u => u.Dapper<Rocket>()).Returns(_fixture.DapperRocketRepository.Object);
            _fixture.Uow.Setup(u => u.Dapper<Mission>()).Returns(_fixture.DapperMissionRepository.Object);
            _fixture.Uow.Setup(u => u.Dapper<Orbit>()).Returns(_fixture.DapperOrbitRepository.Object);
            _fixture.Uow.Setup(u => u.Dapper<Pad>()).Returns(_fixture.DapperPadRepository.Object);
            _fixture.Uow.Setup(u => u.Dapper<Location>()).Returns(_fixture.DapperLocationRepository.Object);
            _fixture.Uow.Setup(u => u.Repository(typeof(IUpdateLogRepository))).Returns(_fixture.UpdateLogRepository.Object);
            _fixture.Uow.Setup(u => u.Repository(typeof(ILaunchViewRepository))).Returns(_fixture.LaunchViewRepository.Object);

            _fixture.LaunchRepository.SetupSequence(l => l.EntityExist(
                It.IsAny<Expression<Func<Launch, bool>>>(),
                string.Empty))
                .ReturnsAsync(false)
                .ReturnsAsync(false)
                .ReturnsAsync(false);

            _fixture.LaunchRepository.SetupSequence(l => l.SaveTransaction(new Launch(
                TestLaunchObjects.Test1(),
                TestLaunchObjects.Test1().IdStatus,
                TestLaunchObjects.Test1().IdLaunchServiceProvider,
                TestLaunchObjects.Test1().IdRocket,
                TestLaunchObjects.Test1().IdMission,
                TestLaunchObjects.Test1().IdPad)));

            _fixture.LaunchRepository.SetupSequence(l => l.SaveTransaction(new Launch(
                TestLaunchObjects.Test2(),
                TestLaunchObjects.Test2().IdStatus,
                TestLaunchObjects.Test2().IdLaunchServiceProvider,
                TestLaunchObjects.Test2().IdRocket,
                TestLaunchObjects.Test2().IdMission,
                TestLaunchObjects.Test2().IdPad)));

            _fixture.LaunchRepository.SetupSequence(l => l.SaveTransaction(new Launch(
                TestLaunchObjects.Test3(),
                TestLaunchObjects.Test3().IdStatus,
                TestLaunchObjects.Test3().IdLaunchServiceProvider,
                TestLaunchObjects.Test3().IdRocket,
                TestLaunchObjects.Test3().IdMission,
                TestLaunchObjects.Test3().IdPad)));

            _fixture.DapperStatusRepository.SetupSequence(l => l.GetSelected<Guid>(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<object>(),
                It.IsAny<IDbContextTransaction>()))
                .ReturnsAsync((Guid)TestLaunchObjects.Test1().IdStatus)
                .ReturnsAsync((Guid)TestLaunchObjects.Test2().IdStatus)
                .ReturnsAsync((Guid)TestLaunchObjects.Test3().IdStatus);

            _fixture.DapperLaunchServiceProviderRepository.SetupSequence(l => l.GetSelected<Guid>(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<object>(),
                It.IsAny<IDbContextTransaction>()))
                .ReturnsAsync((Guid)TestLaunchObjects.Test1().IdLaunchServiceProvider)
                .ReturnsAsync((Guid)TestLaunchObjects.Test2().IdLaunchServiceProvider)
                .ReturnsAsync((Guid)TestLaunchObjects.Test3().IdLaunchServiceProvider);

            _fixture.DapperConfigurationRepository.SetupSequence(l => l.GetSelected<Guid>(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<object>(),
                It.IsAny<IDbContextTransaction>()))
                .ReturnsAsync((Guid)TestLaunchObjects.Test1().Rocket.IdConfiguration)
                .ReturnsAsync((Guid)TestLaunchObjects.Test2().Rocket.IdConfiguration)
                .ReturnsAsync((Guid)TestLaunchObjects.Test3().Rocket.IdConfiguration);

            _fixture.DapperRocketRepository.SetupSequence(l => l.GetSelected<Guid>(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<object>(),
                It.IsAny<IDbContextTransaction>()))
                .ReturnsAsync(TestLaunchObjects.Test1().Rocket.Id)
                .ReturnsAsync(TestLaunchObjects.Test2().Rocket.Id)
                .ReturnsAsync(TestLaunchObjects.Test3().Rocket.Id);

            _fixture.DapperMissionRepository.SetupSequence(l => l.GetSelected<Guid>(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<object>(),
                It.IsAny<IDbContextTransaction>()))
                .ReturnsAsync((Guid)TestLaunchObjects.Test1().IdMission)
                .ReturnsAsync((Guid)TestLaunchObjects.Test2().IdMission)
                .ReturnsAsync((Guid)TestLaunchObjects.Test3().IdMission);

            _fixture.DapperOrbitRepository.SetupSequence(l => l.GetSelected<Guid>(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<object>(),
                It.IsAny<IDbContextTransaction>()))
                .ReturnsAsync((Guid)TestLaunchObjects.Test1().Mission.IdOrbit)
                .ReturnsAsync((Guid)TestLaunchObjects.Test2().Mission.IdOrbit)
                .ReturnsAsync((Guid)TestLaunchObjects.Test3().Mission.IdOrbit);

            _fixture.DapperPadRepository.SetupSequence(l => l.GetSelected<Guid>(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<object>(),
                It.IsAny<IDbContextTransaction>()))
                .ReturnsAsync(TestLaunchObjects.Test1().Pad.Id)
                .ReturnsAsync(TestLaunchObjects.Test2().Pad.Id)
                .ReturnsAsync(TestLaunchObjects.Test3().Pad.Id);

            _fixture.DapperPadRepository.SetupSequence(l => l.GetSelected<Guid>(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<object>(),
                It.IsAny<IDbContextTransaction>()))
                .ReturnsAsync((Guid)TestLaunchObjects.Test1().Pad.IdLocation)
                .ReturnsAsync((Guid)TestLaunchObjects.Test2().Pad.IdLocation)
                .ReturnsAsync((Guid)TestLaunchObjects.Test3().Pad.IdLocation);

            _fixture.Mapper.SetupSequence(m => m.Map<Launch>(It.IsAny<LaunchDTO>()))
                .Returns(TestLaunchObjects.Test1())
                .Returns(TestLaunchObjects.Test2())
                .Returns(TestLaunchObjects.Test3());

            _fixture.LaunchViewRepository.Setup(lv => lv.RefreshView());
            _fixture.Client.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(() => client);
            _fixture.LaunchRepository.Setup(l => l.GetTransaction()).ReturnsAsync(Mock.Of<IDbContextTransaction>());
            var response = await client.GetAsync(url);
            var json = await response.Content.ReadFromJsonAsync<RequestLaunchDTO>();

            var business = new LaunchApiBusiness(_fixture.Uow.Object, _fixture.Client.Object, _fixture.Mapper.Object);

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
            var client = mockHttp.ToHttpClient();

            _fixture.Uow.Setup(u => u.Repository(typeof(IUpdateLogRepository))).Returns(_fixture.UpdateLogRepository.Object);
            _fixture.Uow.Setup(u => u.Repository(typeof(ILaunchViewRepository))).Returns(_fixture.LaunchViewRepository.Object);
            _fixture.Client.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(client);
            var response = await client.GetAsync(url);

            var business = new LaunchApiBusiness(_fixture.Uow.Object, _fixture.Client.Object, _fixture.Mapper.Object);
            
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
            var client = mockHttp.ToHttpClient();

            _fixture.Uow.Setup(u => u.Repository(typeof(IUpdateLogRepository))).Returns(_fixture.UpdateLogRepository.Object);
            _fixture.Uow.Setup(u => u.Repository(typeof(ILaunchViewRepository))).Returns(_fixture.LaunchViewRepository.Object);
            _fixture.Client.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(client);
            _fixture.Mapper.Setup(m => m.Map<RequestLaunchDTO>(It.IsAny<LaunchDTO>()))
                .Returns(new RequestLaunchDTO() { Count = 0, Next = null, Previous = null, Results = new List<LaunchDTO>() });
            
            var response = await client.GetAsync(url);
            var business = new LaunchApiBusiness(_fixture.Uow.Object, _fixture.Client.Object, _fixture.Mapper.Object);

            //Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(async () => await business.UpdateDataSet(request));
        }

        [Fact]
        public async Task LaunchApiBusiness_SearchByParam_FoundByParam()
        {
            //Arrange
            var request = new SearchLaunchRequest(){ Mission = "Pioneer" };
            var pagination = new Pagination<LaunchView>(){ NumberOfEntities = 1, CurrentPage = 0, NumberOfPages = 1, Entities = new List<LaunchView>() { TestLaunchViewObjects.Test2() } };

            _fixture.Uow.Setup(u => u.Repository(typeof(IMissionRepository))).Returns(_fixture.MissionRepository.Object);
            _fixture.Uow.Setup(u => u.Repository(typeof(ILaunchViewRepository))).Returns(_fixture.LaunchViewRepository.Object);
            
            _fixture.LaunchViewRepository.Setup(l => l.GetViewPaged(0, 10, It.IsAny<List<Expression<Func<LaunchView, bool>>>>(), null))
                .ReturnsAsync(pagination);
            _fixture.MissionRepository.Setup(m => m.ILikeSearch(request.Mission, It.IsAny<Expression<Func<Mission, Guid>>>(), null))
                .ReturnsAsync(new List<Guid>(){ (Guid)TestLaunchObjects.Test2().IdMission });

            var business = new LaunchApiBusiness(_fixture.Uow.Object, _fixture.Client.Object, _fixture.Mapper.Object);

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
            var business = new LaunchApiBusiness(_fixture.Uow.Object, _fixture.Client.Object, _fixture.Mapper.Object);

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
            _fixture.Uow.Setup(u => u.Repository(typeof(IMissionRepository))).Returns(_fixture.MissionRepository.Object);
            _fixture.Uow.Setup(u => u.Repository(typeof(ILaunchViewRepository))).Returns(_fixture.LaunchViewRepository.Object);
            
            _fixture.LaunchViewRepository.Setup(l => l.GetViewPaged(0, 10, It.IsAny<List<Expression<Func<LaunchView, bool>>>>(), null))
                .ReturnsAsync(pagination);
            _fixture.MissionRepository.Setup(m => m.ILikeSearch(request.Mission, It.IsAny<Expression<Func<Mission, Guid>>>(), null))
                .ReturnsAsync(new List<Guid>(){ (Guid)TestLaunchObjects.Test2().IdMission });

            var business = new LaunchApiBusiness(_fixture.Uow.Object, _fixture.Client.Object, _fixture.Mapper.Object);

            //Act
            var result = business.SearchByParam(request);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<AggregateException>(result.Exception);
            Assert.Equal(result.Exception.Message, "One or more errors occurred. (Attention! The requested data was not found.)");
        }
    }
}
