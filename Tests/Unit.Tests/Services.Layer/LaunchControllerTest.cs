using System.ComponentModel.DataAnnotations;
using Application.Wrappers;
using Domain.Commands.Launch.Requests;
using Domain.Commands.Launch.Responses;
using Domain.Materializated.Views;
using Domain.Queries.Launch.Requests;
using Domain.Queries.Launch.Responses;
using Domain.Request;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Controllers;
using Tests.Test.Objects;
using Tests.Unit.Tests.Fixture;

namespace Tests.Unit.Tests.Services.Layer
{
    public class LaunchControllerTest : IClassFixture<ServicesTestFixture>
    {
        private readonly ServicesTestFixture _fixture;
        public LaunchControllerTest(ServicesTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void LaunchersController_SearchByParams_TestingInputs()
        {
            //Arrange
            Pagination<LaunchView> pagination = new(){ 
                CurrentPage = 1,
                Entities = new List<LaunchView>(){ TestLaunchViewObjects.Test3() },
                NumberOfEntities = 1,
                NumberOfPages = 1
            };

            SearchLaunchRequest request = new(){ Mission = "Zenit" };
            SeachByParamResponse response = new(true, string.Empty, pagination);

            _fixture.Mediator.Setup(m => m.Send(It.IsAny<MediatrRequestWrapper<SearchLaunchRequest, SeachByParamResponse>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);
            var controller = new LaunchController(_fixture.Mediator.Object);

            //Act
            var result = controller.SearchByParams(request).Result as OkObjectResult;

            //Assert
            Assert.NotNull(result);
            Assert.IsType<SeachByParamResponse>(result.Value);
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        }

        [Fact]
        public void LaunchersController_SearchByParams_NullParam()
        {
            //Arrange
            SearchLaunchRequest request = null;
            var controller = new LaunchController(_fixture.Mediator.Object);

            //Act
            var result = controller.SearchByParams(request).Result as BadRequestObjectResult;

            //Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.IsType<string>(result.Value);
            Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
        }

        [Fact]
        public void LaunchersController_SearchByParams_ValidationRequestObj422Exception()
        {
            //Arrange
            SearchLaunchRequest request = new(){ Mission = "sdmfiosadffdsafdfasffafdisadfjoasidfjiosadjfioasdjfioasjdiksdjmfioasjkmdfasjf9noadfhjoiasudjfioadfjioadskaiofdjopisjwif2wj3490f2j390fn2jm390fn29i3nf90i21jnm3f0i12nm30i2fnm230fnm20i3nm210o3fnm12i3ofm1203fm1203fm1203fm201m3f12903mf9012m3f9021mf39okm,0eiowsqmfepdqwmfopw,edopfqwesnmfidfmsdfjwqiefjnmwqi0fj09inm203mi120o3jkmfi23nmf9inerw9iofnmwqiefm0qwiefjmkoqwiefmqwioefm,qwoefd" };

            _fixture.Mediator.Setup(m => m.Send(It.IsAny<MediatrRequestWrapper<SearchLaunchRequest, SeachByParamResponse>>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new ValidationException(It.IsAny<string>()));

            var controller = new LaunchController(_fixture.Mediator.Object);

            //Act
            var result = controller.SearchByParams(request).Result as ObjectResult;

            //Assert
            Assert.NotNull(result);
            Assert.IsType<ObjectResult>(result);
            Assert.IsType<int>(result.StatusCode);
            Assert.Equal(result.Value, "Exception of type 'System.ComponentModel.DataAnnotations.ValidationException' was thrown."); //Automatic message.
            Assert.Equal(StatusCodes.Status422UnprocessableEntity, result.StatusCode);
        }

        [Fact]
        public void LaunchersController_SearchByParams_InvalidRequest()
        {
            //Arrange
            SearchLaunchRequest request = new(){ Mission = "sdmfiosadffdsafdfasffafdisadfjoasidfjiosadjfioasdjfioasjdiksdjmfioasjkmdfasjf9noadfhjoiasudjfioadfjioadskaiofdjopisjwif2wj3490f2j390fn2jm390fn29i3nf90i21jnm3f0i12nm30i2fnm230fnm20i3nm210o3fnm12i3ofm1203fm1203fm1203fm201m3f12903mf9012m3f9021mf39okm,0eiowsqmfepdqwmfopw,edopfqwesnmfidfmsdfjwqiefjnmwqi0fj09inm203mi120o3jkmfi23nmf9inerw9iofnmwqiefm0qwiefjmkoqwiefmqwioefm,qwoefd" };

            //Act
            var errorCount = ValidationHelper.ValidateObject(ref request);

            //Assert
            Assert.True(errorCount.Count == 1);
            Assert.Equal(errorCount[0].ErrorMessage, "Attention! The character length in the field Mission is invalid.");
        }

        [Fact]
        public void LaunchersController_SearchByParams_NotFoundResults()
        {
            //Arrange
            SearchLaunchRequest request = new(){ Mission = "chicken" };
            _fixture.Mediator.Setup(m => m.Send(It.IsAny<MediatrRequestWrapper<SearchLaunchRequest, SeachByParamResponse>>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new KeyNotFoundException());
            var controller = new LaunchController(_fixture.Mediator.Object);

            //Act
            var result = controller.SearchByParams(request).Result as NotFoundObjectResult;
        
            //Assert
            Assert.NotNull(result);
            Assert.IsType<NotFoundObjectResult>(result);
            Assert.IsType<string>(result.Value);
            Assert.Equal(result.Value, "The given key was not present in the dictionary."); //Automatic message.
            Assert.Equal(StatusCodes.Status404NotFound, result.StatusCode);
        }

        [Fact]
        public void LaunchersController_SearchByParams_InternalServerError()
        {
            //Arrange
            var search = new SearchLaunchRequest();
            _fixture.Mediator.Setup(m => m.Send(It.IsAny<MediatrRequestWrapper<SearchLaunchRequest, SeachByParamResponse>>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception());
            var controller = new LaunchController(_fixture.Mediator.Object);

            //Act
            var result = controller.SearchByParams(search).Result as ObjectResult;

            //Assert
            Assert.NotNull(result);
            Assert.IsType<ObjectResult>(result);
            Assert.IsType<string>(result.Value);
            Assert.Equal(result.Value, "Attention! The Service is unavailable.\nException of type 'System.Exception' was thrown."); //Automatic message.
            Assert.Equal(StatusCodes.Status500InternalServerError, result.StatusCode);
        }

        [Fact]
        public void LaunchersController_GetById_OkFoundObject()
        {
            //Arrange
            GetByIdRequest request = new(){ launchId = new Guid(TestLaunchViewObjects.Test1().Id.ToString()) };
            GetOneLaunchResponse response = new(true, string.Empty, TestLaunchViewObjects.Test1());

            _fixture.Mediator.Setup(m => m.Send(It.IsAny<MediatrRequestWrapper<GetByIdRequest, GetOneLaunchResponse>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);
            var controller = new LaunchController(_fixture.Mediator.Object);

            //Act
            var result = controller.GetById(request).Result as OkObjectResult;

            //Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
            Assert.IsType<GetOneLaunchResponse>(result.Value);
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        }

        [Fact]
        public void LaunchersController_GetById_NullId()
        {
            //Arrange
            _fixture.Mediator.Setup(m => m.Send(It.IsAny<MediatrRequestWrapper<GetByIdRequest, GetOneLaunchResponse>>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new ArgumentNullException());
            var controller = new LaunchController(_fixture.Mediator.Object);

            //Act
            var result = controller.GetById(null).Result as BadRequestObjectResult;

            //Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.IsType<string>(result.Value);
            Assert.Equal(result.Value, "Value cannot be null."); //Automatic message.
            Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
        }

        [Fact]
        public void LaunchersController_GetById_RandomNotFoundGuid()
        {
            //Arrange
            _fixture.Mediator.Setup(m => m.Send(It.IsAny<MediatrRequestWrapper<GetByIdRequest, GetOneLaunchResponse>>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new KeyNotFoundException());
            var controller = new LaunchController(_fixture.Mediator.Object);

            //Act
            var result = controller.GetById(new GetByIdRequest() { launchId = It.IsAny<Guid>() }).Result as NotFoundObjectResult;

            //Assert
            Assert.NotNull(result);
            Assert.IsType<NotFoundObjectResult>(result);
            Assert.IsType<string>(result.Value);
            Assert.Equal(result.Value, "The given key was not present in the dictionary."); //Automatic message.
            Assert.Equal(StatusCodes.Status404NotFound, result.StatusCode);
        }

        [Fact]
        public void LaunchersController_GetById_InternalServerError()
        {
            //Arrange
            _fixture.Mediator.Setup(m => m.Send(It.IsAny<MediatrRequestWrapper<GetByIdRequest, GetOneLaunchResponse>>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception());
            var controller = new LaunchController(_fixture.Mediator.Object);

            //Act
            var result = controller.GetById(new GetByIdRequest() { launchId = It.IsAny<Guid>() }).Result as ObjectResult;

            //Assert
            Assert.NotNull(result);
            Assert.IsType<ObjectResult>(result);
            Assert.IsType<string>(result.Value);
            Assert.Equal(result.Value, "Attention! The Service is unavailable.\nException of type 'System.Exception' was thrown."); //Automatic message.
            Assert.Equal(StatusCodes.Status500InternalServerError, result.StatusCode);
        }

        [Fact]
        public void LaunchersController_GetAllPaged_ReturnPagedLaunchs()
        {
            //Arrange
            Pagination<LaunchView> pagination = new()
            { 
                CurrentPage = 1,
                Entities = new List<LaunchView>(){ TestLaunchViewObjects.Test1(), TestLaunchViewObjects.Test2(), TestLaunchViewObjects.Test3() },
                NumberOfEntities = 3,
                NumberOfPages = 1
            };
            PageRequest request = new(){ Page = It.IsAny<int?>() };
            GetLaunchesPagedResponse response = new(true, string.Empty, pagination);

            _fixture.Mediator.Setup(m => m.Send(It.IsAny<MediatrRequestWrapper<PageRequest, GetLaunchesPagedResponse>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);
            var controller = new LaunchController(_fixture.Mediator.Object);

            //Act
            var result = controller.GetAllPaged(new PageRequest(It.IsAny<int?>())).Result as OkObjectResult;

            //Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        }

        [Fact]
        public void LaunchersController_GetAllPaged_InvalidPage()
        {
            //Arrange
            _fixture.Mediator.Setup(m => m.Send(It.IsAny<MediatrRequestWrapper<PageRequest, GetLaunchesPagedResponse>>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new InvalidOperationException(ErrorMessages.InvalidPageSelected));
            var controller = new LaunchController(_fixture.Mediator.Object);

            //Act
            var result = controller.GetAllPaged(new PageRequest(2)).Result as BadRequestObjectResult;

            //Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(result.Value, ErrorMessages.InvalidPageSelected);
            Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
        }

        [Fact]
        public void LaunchersController_GetAllPaged_NoDataFound()
        {
            //Arrange
            _fixture.Mediator.Setup(m => m.Send(It.IsAny<MediatrRequestWrapper<PageRequest, GetLaunchesPagedResponse>>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new KeyNotFoundException(ErrorMessages.NoData));
            var controller = new LaunchController(_fixture.Mediator.Object);

            //Act
            var result = controller.GetAllPaged(new PageRequest(It.IsAny<int?>())).Result as NotFoundObjectResult;

            //Assert
            Assert.NotNull(result);
            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(result.Value, ErrorMessages.NoData);
            Assert.Equal(StatusCodes.Status404NotFound, result.StatusCode);
        }

        [Fact]
        public void LaunchersController_GetAllPaged_InternalError()
        {
            //Arrange
            _fixture.Mediator.Setup(m => m.Send(It.IsAny<MediatrRequestWrapper<PageRequest, GetLaunchesPagedResponse>>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception());
            var controller = new LaunchController(_fixture.Mediator.Object);

            //Act
            var result = controller.GetAllPaged(new PageRequest(It.IsAny<int?>())).Result as ObjectResult;

            //Assert
            Assert.NotNull(result);
            Assert.IsType<ObjectResult>(result);
            Assert.IsType<string>(result.Value);
            Assert.Equal(result.Value, "Attention! The Service is unavailable.\nException of type 'System.Exception' was thrown."); //Automatic message.
            Assert.Equal(StatusCodes.Status500InternalServerError, result.StatusCode);
        }

        [Fact]
        public void LaunchersController_Delete_SoftDeleteLaunch()
        {
            //Arrange
            SoftDeleteLaunchRequest request = new(){ launchId = It.IsAny<Guid?>() };
            SoftDeleteLaunchResponse response = new(true, string.Empty);

            _fixture.Mediator.Setup(m => m.Send(It.IsAny<MediatrRequestWrapper<SoftDeleteLaunchRequest, SoftDeleteLaunchResponse>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);
            var controller = new LaunchController(_fixture.Mediator.Object);

            //Act
            var result = controller.Delete(new SoftDeleteLaunchRequest(){ launchId = It.IsAny<Guid?>() }).Result as OkObjectResult;

            //Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        }

        [Fact]
        public void LaunchersController_Delete_NullGuid()
        {
            //Arrange
            _fixture.Mediator.Setup(m => m.Send(It.IsAny<MediatrRequestWrapper<SoftDeleteLaunchRequest, SoftDeleteLaunchResponse>>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new ArgumentNullException());
            var controller = new LaunchController(_fixture.Mediator.Object);

            //Act
            var result = controller.Delete(null).Result as BadRequestObjectResult; 

            //Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.IsType<string>(result.Value);
            Assert.Equal(result.Value, "Value cannot be null."); //Automatic message.
            Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
        }

        [Fact]
        public void LaunchersController_Delete_NotFoundLaunch()
        {
            //Arrange
            _fixture.Mediator.Setup(m => m.Send(It.IsAny<MediatrRequestWrapper<SoftDeleteLaunchRequest, SoftDeleteLaunchResponse>>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new KeyNotFoundException());
            var controller = new LaunchController(_fixture.Mediator.Object);

            //Act
            var result = controller.Delete(new SoftDeleteLaunchRequest(){ launchId = It.IsAny<Guid?>() }).Result as NotFoundObjectResult;

            //Assert
            Assert.NotNull(result);
            Assert.IsType<NotFoundObjectResult>(result);
            Assert.IsType<string>(result.Value);
            Assert.Equal(result.Value, "The given key was not present in the dictionary."); //Automatic message.
            Assert.Equal(StatusCodes.Status404NotFound, result.StatusCode);
        }

        [Fact]
        public void LaunchersController_Delete_InternalServerError()
        {
            //Arrange
            _fixture.Mediator.Setup(m => m.Send(It.IsAny<MediatrRequestWrapper<SoftDeleteLaunchRequest, SoftDeleteLaunchResponse>>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception());
            var controller = new LaunchController(_fixture.Mediator.Object);

            //Act
            var result = controller.Delete(new SoftDeleteLaunchRequest(){ launchId = It.IsAny<Guid?>() }).Result as ObjectResult;

            //Assert
            Assert.NotNull(result);
            Assert.IsType<ObjectResult>(result);
            Assert.IsType<string>(result.Value);
            Assert.Equal(result.Value, "Attention! The Service is unavailable.\nException of type 'System.Exception' was thrown."); //Automatic message.
            Assert.Equal(StatusCodes.Status500InternalServerError, result.StatusCode);
        }

        [Fact]
        public void LaunchersController_Update_OkUpdated()
        {
            //Arrange
            UpdateOneLaunchRequest request = new(){ launchId = It.IsAny<Guid?>() };
            UpdateOneLaunchResponse response = new(true, string.Empty, TestLaunchViewObjects.Test1());

            _fixture.Mediator.Setup(m => m.Send(It.IsAny<MediatrRequestWrapper<UpdateOneLaunchRequest, UpdateOneLaunchResponse>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);
            var controller = new LaunchController(_fixture.Mediator.Object);

            //Act
            var result = controller.Update(new UpdateOneLaunchRequest() { launchId = It.IsAny<Guid?>() }).Result as OkObjectResult;

            //Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
            Assert.IsType<UpdateOneLaunchResponse>(result.Value);
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        }

        [Fact]
        public void LaunchersController_Update_NullArgument()
        {
            //Arrange
            _fixture.Mediator.Setup(m => m.Send(It.IsAny<MediatrRequestWrapper<UpdateOneLaunchRequest, UpdateOneLaunchResponse>>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new ArgumentNullException());
            var controller = new LaunchController(_fixture.Mediator.Object);

            //Act
            var result = controller.Update(null).Result as BadRequestObjectResult;

            //Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.IsType<string>(result.Value);
            Assert.Equal(result.Value, "Value cannot be null."); //Automatic message.
            Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
        }

        [Fact]
        public void LaunchersController_Update_LaunchNotFound()
        {
            //Arrange
            _fixture.Mediator.Setup(m => m.Send(It.IsAny<MediatrRequestWrapper<UpdateOneLaunchRequest, UpdateOneLaunchResponse>>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new KeyNotFoundException());
            var controller = new LaunchController(_fixture.Mediator.Object);

            //Act
            var result = controller.Update(new UpdateOneLaunchRequest() { launchId = It.IsAny<Guid?>() }).Result as NotFoundObjectResult;

            //Assert
            Assert.NotNull(result);
            Assert.IsType<NotFoundObjectResult>(result);
            Assert.IsType<string>(result.Value);
            Assert.Equal(result.Value, "The given key was not present in the dictionary."); //Automatic message.
            Assert.Equal(StatusCodes.Status404NotFound, result.StatusCode);
        }

        [Fact]
        public void LaunchersController_Update_HttpErrorTooManyRequests()
        {
            //Arrange
            _fixture.Mediator.Setup(m => m.Send(It.IsAny<MediatrRequestWrapper<UpdateOneLaunchRequest, UpdateOneLaunchResponse>>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new HttpRequestException());
            var controller = new LaunchController(_fixture.Mediator.Object);

            //Act
            var result = controller.Update(new UpdateOneLaunchRequest() { launchId = It.IsAny<Guid?>() }).Result as ObjectResult;

            //Assert
            Assert.NotNull(result);
            Assert.IsType<ObjectResult>(result);
            Assert.IsType<string>(result.Value);
            Assert.Equal(result.Value, "Exception of type 'System.Net.Http.HttpRequestException' was thrown."); //Automatic message.
            Assert.Equal(StatusCodes.Status429TooManyRequests, result.StatusCode);
        }

        [Fact]
        public void LaunchersController_Update_InternalError()
        {
            //Arrange
            _fixture.Mediator.Setup(m => m.Send(It.IsAny<MediatrRequestWrapper<UpdateOneLaunchRequest, UpdateOneLaunchResponse>>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception());
            var controller = new LaunchController(_fixture.Mediator.Object);

            //Act
            var result = controller.Update(new UpdateOneLaunchRequest() { launchId = It.IsAny<Guid?>() }).Result as ObjectResult;

            //Assert
            Assert.NotNull(result);
            Assert.IsType<ObjectResult>(result);
            Assert.IsType<string>(result.Value);
            Assert.Equal(result.Value, "Attention! The Service is unavailable.\nException of type 'System.Exception' was thrown."); //Automatic message.
            Assert.Equal(StatusCodes.Status500InternalServerError, result.StatusCode);
        }

        [Fact]
        public void LaunchersController_UpdateDataSet_UpdateOk()
        {
            //Arrange
            UpdateDataSetResponse response = new(true, string.Empty, true);

            _fixture.Mediator.Setup(m => m.Send(It.IsAny<MediatrRequestWrapper<UpdateLaunchSetRequest, UpdateDataSetResponse>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);
            var controller = new LaunchController(_fixture.Mediator.Object);

            //Act
            var result = controller.UpdateDataSet(It.IsAny<UpdateLaunchSetRequest>()).Result as OkObjectResult;

            //Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
            Assert.IsType<UpdateDataSetResponse>(result.Value);
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        }

        [Fact]
        public void LaunchersController_UpdateDataSet_UpdateFailed()
        {
            //Arrange
            UpdateDataSetResponse response = new(true, string.Empty, false);

            _fixture.Mediator.Setup(m => m.Send(It.IsAny<MediatrRequestWrapper<UpdateLaunchSetRequest, UpdateDataSetResponse>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);
            var controller = new LaunchController(_fixture.Mediator.Object);

            //Act
            var result = controller.UpdateDataSet(It.IsAny<UpdateLaunchSetRequest>()).Result as ObjectResult;

            //Assert
            Assert.NotNull(result);
            Assert.IsType<ObjectResult>(result);
            Assert.IsType<string>(result.Value);
            Assert.Equal(result.Value, ErrorMessages.InternalServerError); //Automatic message.
            Assert.Equal(StatusCodes.Status500InternalServerError, result.StatusCode);
        }

        [Fact]
        public void LaunchersController_UpdateDataSet_TooManyRequests()
        {
            //Arrange
            _fixture.Mediator.Setup(m => m.Send(It.IsAny<MediatrRequestWrapper<UpdateLaunchSetRequest, UpdateDataSetResponse>>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new HttpRequestException());
            var controller = new LaunchController(_fixture.Mediator.Object);

            //Act
            var result = controller.UpdateDataSet(It.IsAny<UpdateLaunchSetRequest>()).Result as ObjectResult;

            //Assert
            Assert.NotNull(result);
            Assert.IsType<ObjectResult>(result);
            Assert.IsType<string>(result.Value);
            Assert.Equal(result.Value, "Exception of type 'System.Net.Http.HttpRequestException' was thrown."); //Automatic message.
            Assert.Equal(StatusCodes.Status429TooManyRequests, result.StatusCode);
        }

        [Fact]
        public void LaunchersController_UpdateDataSet_NoData()
        {
            //Arrange
            _fixture.Mediator.Setup(m => m.Send(It.IsAny<MediatrRequestWrapper<UpdateLaunchSetRequest, UpdateDataSetResponse>>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new KeyNotFoundException());
            var controller = new LaunchController(_fixture.Mediator.Object);

            //Act
            var result = controller.UpdateDataSet(It.IsAny<UpdateLaunchSetRequest>()).Result as NotFoundObjectResult;

            //Assert
            Assert.NotNull(result);
            Assert.IsType<NotFoundObjectResult>(result);
            Assert.IsType<string>(result.Value);
            Assert.Equal(result.Value, "The given key was not present in the dictionary."); //Automatic message.
            Assert.Equal(StatusCodes.Status404NotFound, result.StatusCode);
        }

        [Fact]
        public void LaunchersController_UpdateDataSet_InternalError()
        {
            //Arrange
            _fixture.Mediator.Setup(m => m.Send(It.IsAny<MediatrRequestWrapper<UpdateLaunchSetRequest, UpdateDataSetResponse>>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception());
            var controller = new LaunchController(_fixture.Mediator.Object);

            //Act
            var result = controller.UpdateDataSet(It.IsAny<UpdateLaunchSetRequest>()).Result as ObjectResult;

            //Assert
            Assert.NotNull(result);
            Assert.IsType<ObjectResult>(result);
            Assert.IsType<string>(result.Value);
            Assert.Equal(result.Value, "Attention! The Service is unavailable.\nException of type 'System.Exception' was thrown."); //Automatic message.
            Assert.Equal(StatusCodes.Status500InternalServerError, result.StatusCode);
        }
        
        [Fact]
        public void LaunchApiBusiness_UpdateDataSet_ValidationRequestObj422Exception()
        {
            //Arrange
            var request = new UpdateLaunchSetRequest(){ Limit = 500, Iterations = 20, Skip = 0 };

            _fixture.Mediator.Setup(m => m.Send(It.IsAny<MediatrRequestWrapper<UpdateLaunchSetRequest, UpdateDataSetResponse>>(), It.IsAny<CancellationToken>()))
                .Throws(new ValidationException(It.IsAny<string>()));

            var controller = new LaunchController(_fixture.Mediator.Object);

            //Act
            var result = controller.UpdateDataSet(request).Result as ObjectResult;

            //Assert
            Assert.NotNull(result);
            Assert.IsType<ObjectResult>(result);
            Assert.IsType<int>(result.StatusCode);
            Assert.Equal(result.Value, "Exception of type 'System.ComponentModel.DataAnnotations.ValidationException' was thrown."); //Automatic message.
            Assert.Equal(StatusCodes.Status422UnprocessableEntity, result.StatusCode);
        }
    }
}