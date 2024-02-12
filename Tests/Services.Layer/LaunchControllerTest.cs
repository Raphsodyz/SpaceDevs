using System.ComponentModel.DataAnnotations;
using Business.DTO.Entities;
using Business.DTO.Request;
using Data.Materializated.Views;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Controllers;
using Tests.Test.Objects;
using Xunit.Sdk;

namespace Tests.Services.Layer
{
    public class LaunchControllerTest
    {

        [Fact]
        public void LaunchersController_SearchByParams_OkFoundData()
        {
            //Arrange
            var launchApiBusiness = new Mock<ILaunchApiBusiness>();

            SearchLaunchRequest request = new(){ Mission = "Zenit" };
            Pagination<LaunchView> pagination = new(){ CurrentPage = 1, Entities = new List<LaunchView>(){ TestLaunchViewObjects.Test3() }, NumberOfEntities = 1, NumberOfPages = 1 };

            launchApiBusiness.Setup(a => a.SearchByParam(request)).ReturnsAsync(pagination);
            var controller = new LaunchController(launchApiBusiness.Object);

            //Act
            var result = controller.SearchByParams(request).Result as OkObjectResult;

            //Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
            Assert.IsType<Pagination<LaunchView>>(result.Value);
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        }

        [Fact]
        public void LaunchersController_SearchByParams_NullParam()
        {
            //Arrange
            var launchApiBusiness = new Mock<ILaunchApiBusiness>();

            SearchLaunchRequest request = null;
            launchApiBusiness.Setup(a => a.SearchByParam(request));
            var controller = new LaunchController(launchApiBusiness.Object);

            //Act
            var result = controller.SearchByParams(request).Result as BadRequestObjectResult;

            //Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.IsType<string>(result.Value);
            Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
        }

        [Fact]
        public void LaunchersController_SearchByParams_InvalidRequest()
        {
            //Arrange
            SearchLaunchRequest request = new(){ Mission = "sdmfiosadffdsafdfasffafdisadfjoasidfjiosadjfioasdjfioasjdiksdjmfioasjkmdfasjf9noadfhjoiasudjfioadfjioadskaiofdjopisjwif2wj3490f2j390fn2jm390fn29i3nf90i21jnm3f0i12nm30i2fnm230fnm20i3nm210o3fnm12i3ofm1203fm1203fm1203fm201m3f12903mf9012m3f9021mf39okm,0eiowsqmfepdqwmfopw,edopfqwesnmfidfmsdfjwqiefjnmwqi0fj09inm203mi120o3jkmfi23nmf9inerw9iofnmwqiefm0qwiefjmkoqwiefmqwioefm,qwoefd" };

            //Act
            var errorCount = ValidateObject(ref request);

            //Assert
            Assert.True(errorCount.Count == 1);
            Assert.Equal(errorCount[0].ErrorMessage, "Attention! The character length in the field Mission is invalid.");
        }

        [Fact]
        public void LaunchersController_SearchByParams_NotFoundResults()
        {
            //Arrange
            var launchApiBusiness = new Mock<ILaunchApiBusiness>();

            SearchLaunchRequest request = new(){ Mission = "chicken" };
            launchApiBusiness.Setup(a => a.SearchByParam(request)).ThrowsAsync(new KeyNotFoundException());
            var controller = new LaunchController(launchApiBusiness.Object);

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
            var launchApiBusiness = new Mock<ILaunchApiBusiness>();

            var search = new SearchLaunchRequest();
            launchApiBusiness.Setup(a => a.SearchByParam(search)).ThrowsAsync(new Exception());
            var controller = new LaunchController(launchApiBusiness.Object);

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
            var launchApiBusiness = new Mock<ILaunchApiBusiness>();

            Guid? id = new(TestLaunchViewObjects.Test1().Id.ToString());
            launchApiBusiness.Setup(a => a.GetOneLaunch(id)).ReturnsAsync(TestLaunchViewObjects.Test1());
            var controller = new LaunchController(launchApiBusiness.Object);

            //Act
            var result = controller.GetById(id).Result as OkObjectResult;

            //Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
            Assert.IsType<LaunchView>(result.Value);
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        }

        [Fact]
        public void LaunchersController_GetById_NullId()
        {
            //Arrange
            var launchApiBusiness = new Mock<ILaunchApiBusiness>();

            Guid? id = null;
            launchApiBusiness.Setup(a => a.GetOneLaunch(id)).ThrowsAsync(new ArgumentNullException());
            var controller = new LaunchController(launchApiBusiness.Object);

            //Act
            var result = controller.GetById(id).Result as BadRequestObjectResult;

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
            var launchApiBusiness = new Mock<ILaunchApiBusiness>();

            Guid? id = Guid.NewGuid();
            launchApiBusiness.Setup(a => a.GetOneLaunch(id)).ThrowsAsync(new KeyNotFoundException());
            var controller = new LaunchController(launchApiBusiness.Object);

            //Act
            var result = controller.GetById(id).Result as NotFoundObjectResult;

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
            var launchApiBusiness = new Mock<ILaunchApiBusiness>();

            launchApiBusiness.Setup(a => a.GetOneLaunch(It.IsAny<Guid>())).ThrowsAsync(new Exception());
            var controller = new LaunchController(launchApiBusiness.Object);

            //Act
            var result = controller.GetById(It.IsAny<Guid>()).Result as ObjectResult;

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
            var launchApiBusiness = new Mock<ILaunchApiBusiness>();

            Pagination<LaunchView> pagination = new(){ CurrentPage = 1, Entities = new List<LaunchView>(){ TestLaunchViewObjects.Test1(), TestLaunchViewObjects.Test2(), TestLaunchViewObjects.Test3() }, NumberOfEntities = 3, NumberOfPages = 1 };
            launchApiBusiness.Setup(a => a.GetAllLaunchPaged(It.IsAny<int>())).ReturnsAsync(pagination);
            var controller = new LaunchController(launchApiBusiness.Object);

            //Act
            var result = controller.GetAllPaged(It.IsAny<int>()).Result as OkObjectResult;

            //Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        }

        [Fact]
        public void LaunchersController_GetAllPaged_InvalidPage()
        {
            //Arrange
            var launchApiBusiness = new Mock<ILaunchApiBusiness>();

            Pagination<LaunchView> pagination = new(){ CurrentPage = 1, Entities = new List<LaunchView>(){ TestLaunchViewObjects.Test1(), TestLaunchViewObjects.Test2(), TestLaunchViewObjects.Test3() }, NumberOfEntities = 3, NumberOfPages = 1 };
            launchApiBusiness.Setup(a => a.GetAllLaunchPaged(2)).ThrowsAsync(new InvalidOperationException(ErrorMessages.InvalidPageSelected));
            var controller = new LaunchController(launchApiBusiness.Object);

            //Act
            var result = controller.GetAllPaged(2).Result as BadRequestObjectResult;

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
            var launchApiBusiness = new Mock<ILaunchApiBusiness>();

            Pagination<LaunchView> pagination = new(){ CurrentPage = 1, Entities = new List<LaunchView>(), NumberOfEntities = 0, NumberOfPages = 1 };
            launchApiBusiness.Setup(a => a.GetAllLaunchPaged(It.IsAny<int>())).ThrowsAsync(new KeyNotFoundException(ErrorMessages.NoData));
            var controller = new LaunchController(launchApiBusiness.Object);

            //Act
            var result = controller.GetAllPaged(It.IsAny<int>()).Result as NotFoundObjectResult;

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
            var launchApiBusiness = new Mock<ILaunchApiBusiness>();

            launchApiBusiness.Setup(a => a.GetAllLaunchPaged(It.IsAny<int>())).ThrowsAsync(new Exception());
            var controller = new LaunchController(launchApiBusiness.Object);

            //Act
            var result = controller.GetAllPaged(It.IsAny<int>()).Result as ObjectResult;

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
            var launchApiBusiness = new Mock<ILaunchApiBusiness>();
            
            launchApiBusiness.Setup(a => a.SoftDeleteLaunch(It.IsAny<Guid>()));
            var controller = new LaunchController(launchApiBusiness.Object);

            //Act
            var result = controller.Delete(It.IsAny<Guid>()).Result as OkObjectResult;

            //Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        }

        [Fact]
        public void LaunchersController_Delete_NullGuid()
        {
            //Arrange
            var launchApiBusiness = new Mock<ILaunchApiBusiness>();

            launchApiBusiness.Setup(a => a.SoftDeleteLaunch(null)).ThrowsAsync(new ArgumentNullException());
            var controller = new LaunchController(launchApiBusiness.Object);

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
            var launchApiBusiness = new Mock<ILaunchApiBusiness>();

            launchApiBusiness.Setup(a => a.SoftDeleteLaunch(It.IsAny<Guid>())).ThrowsAsync(new KeyNotFoundException());
            var controller = new LaunchController(launchApiBusiness.Object);

            //Act
            var result = controller.Delete(It.IsAny<Guid>()).Result as NotFoundObjectResult;

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
            var launchApiBusiness = new Mock<ILaunchApiBusiness>();

            launchApiBusiness.Setup(a => a.SoftDeleteLaunch(It.IsAny<Guid>())).ThrowsAsync(new Exception());
            var controller = new LaunchController(launchApiBusiness.Object);

            //Act
            var result = controller.Delete(It.IsAny<Guid>()).Result as ObjectResult;

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
            var launchApiBusiness = new Mock<ILaunchApiBusiness>();

            launchApiBusiness.Setup(a => a.UpdateLaunch(It.IsAny<Guid>())).ReturnsAsync(TestLaunchViewObjects.Test1());
            var controller = new LaunchController(launchApiBusiness.Object);

            //Act
            var result = controller.Update(It.IsAny<Guid>()).Result as OkObjectResult;

            //Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
            Assert.IsType<LaunchView>(result.Value);
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        }

        [Fact]
        public void LaunchersController_Update_NullArgument()
        {
            //Arrange
            var launchApiBusiness = new Mock<ILaunchApiBusiness>();

            launchApiBusiness.Setup(a => a.UpdateLaunch(null)).ThrowsAsync(new ArgumentNullException());
            var controller = new LaunchController(launchApiBusiness.Object);

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
            var launchApiBusiness = new Mock<ILaunchApiBusiness>();

            launchApiBusiness.Setup(a => a.UpdateLaunch(It.IsAny<Guid>())).ThrowsAsync(new KeyNotFoundException());
            var controller = new LaunchController(launchApiBusiness.Object);

            //Act
            var result = controller.Update(It.IsAny<Guid>()).Result as NotFoundObjectResult;

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
            var launchApiBusiness = new Mock<ILaunchApiBusiness>();

            launchApiBusiness.Setup(a => a.UpdateLaunch(It.IsAny<Guid>())).ThrowsAsync(new HttpRequestException());
            var controller = new LaunchController(launchApiBusiness.Object);

            //Act
            var result = controller.Update(It.IsAny<Guid>()).Result as ObjectResult;

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
            var launchApiBusiness = new Mock<ILaunchApiBusiness>();

            launchApiBusiness.Setup(a => a.UpdateLaunch(It.IsAny<Guid>())).ThrowsAsync(new Exception());
            var controller = new LaunchController(launchApiBusiness.Object);

            //Act
            var result = controller.Update(It.IsAny<Guid>()).Result as ObjectResult;

            //Assert
            Assert.NotNull(result);
            Assert.IsType<ObjectResult>(result);
            Assert.IsType<string>(result.Value);
            Assert.Equal(result.Value, "Attention! The Service is unavailable.\nException of type 'System.Exception' was thrown."); //Automatic message.
            Assert.Equal(StatusCodes.Status500InternalServerError, result.StatusCode);
        }

        [Fact]
        public void LaunchersController_UpdateData_UpdateOk()
        {
            //Arrange
            var launchApiBusiness = new Mock<ILaunchApiBusiness>();

            launchApiBusiness.Setup(a => a.UpdateDataSet(It.IsAny<UpdateLaunchRequest>())).ReturnsAsync(true);
            var controller = new LaunchController(launchApiBusiness.Object);

            //Act
            var result = controller.BulkUpdateData(It.IsAny<UpdateLaunchRequest>()).Result as OkObjectResult;

            //Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
            Assert.IsType<string>(result.Value);
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        }

        [Fact]
        public void LaunchersController_UpdateData_UpdateFailed()
        {
            //Arrange
            var launchApiBusiness = new Mock<ILaunchApiBusiness>();

            launchApiBusiness.Setup(a => a.UpdateDataSet(It.IsAny<UpdateLaunchRequest>())).ReturnsAsync(false);
            var controller = new LaunchController(launchApiBusiness.Object);

            //Act
            var result = controller.BulkUpdateData(It.IsAny<UpdateLaunchRequest>()).Result as ObjectResult;

            //Assert
            Assert.NotNull(result);
            Assert.IsType<ObjectResult>(result);
            Assert.IsType<string>(result.Value);
            Assert.Equal(result.Value, ErrorMessages.InternalServerError); //Automatic message.
            Assert.Equal(StatusCodes.Status500InternalServerError, result.StatusCode);
        }

        [Fact]
        public void LaunchersController_UpdateData_TooManyRequests()
        {
            //Arrange
            var launchApiBusiness = new Mock<ILaunchApiBusiness>();

            launchApiBusiness.Setup(a => a.UpdateDataSet(It.IsAny<UpdateLaunchRequest>())).ThrowsAsync(new HttpRequestException());
            var controller = new LaunchController(launchApiBusiness.Object);

            //Act
            var result = controller.BulkUpdateData(It.IsAny<UpdateLaunchRequest>()).Result as ObjectResult;

            //Assert
            Assert.NotNull(result);
            Assert.IsType<ObjectResult>(result);
            Assert.IsType<string>(result.Value);
            Assert.Equal(result.Value, "Exception of type 'System.Net.Http.HttpRequestException' was thrown."); //Automatic message.
            Assert.Equal(StatusCodes.Status429TooManyRequests, result.StatusCode);
        }

        [Fact]
        public void LaunchersController_UpdateData_NoData()
        {
            //Arrange
            var launchApiBusiness = new Mock<ILaunchApiBusiness>();

            launchApiBusiness.Setup(a => a.UpdateDataSet(It.IsAny<UpdateLaunchRequest>())).ThrowsAsync(new KeyNotFoundException());
            var controller = new LaunchController(launchApiBusiness.Object);

            //Act
            var result = controller.BulkUpdateData(It.IsAny<UpdateLaunchRequest>()).Result as NotFoundObjectResult;

            //Assert
            Assert.NotNull(result);
            Assert.IsType<NotFoundObjectResult>(result);
            Assert.IsType<string>(result.Value);
            Assert.Equal(result.Value, "The given key was not present in the dictionary."); //Automatic message.
            Assert.Equal(StatusCodes.Status404NotFound, result.StatusCode);
        }

        [Fact]
        public void LaunchersController_UpdateData_InternalError()
        {
            //Arrange
            var launchApiBusiness = new Mock<ILaunchApiBusiness>();

            launchApiBusiness.Setup(a => a.UpdateDataSet(It.IsAny<UpdateLaunchRequest>())).ThrowsAsync(new Exception());
            var controller = new LaunchController(launchApiBusiness.Object);

            //Act
            var result = controller.BulkUpdateData(It.IsAny<UpdateLaunchRequest>()).Result as ObjectResult;

            //Assert
            Assert.NotNull(result);
            Assert.IsType<ObjectResult>(result);
            Assert.IsType<string>(result.Value);
            Assert.Equal(result.Value, "Attention! The Service is unavailable.\nException of type 'System.Exception' was thrown."); //Automatic message.
            Assert.Equal(StatusCodes.Status500InternalServerError, result.StatusCode);
        }

        private static IList<ValidationResult> ValidateObject<T>(ref T obj)
        {
            var validate = new List<ValidationResult>();
            var context = new ValidationContext(obj, null, null);
            Validator.TryValidateObject(obj, context, validate, true);
            return validate;
        }
    }
}