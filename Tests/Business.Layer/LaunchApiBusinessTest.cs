using System.Net.Http.Headers;
using System.Data;
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
using Newtonsoft.Json;
using RichardSzalay.MockHttp;
using Tests.Test.Objects;
using System.Text;
using Moq.Protected;

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
            mockHttp.When($"{EndPoints.TheSpaceDevsLaunchEndPoint}{TestLaunchObjects.Test1().ApiGuId}")
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
            var response = await client.GetAsync($"{EndPoints.TheSpaceDevsLaunchEndPoint}{TestLaunchObjects.Test1().ApiGuId}");

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
        public async Task 
    }
}
