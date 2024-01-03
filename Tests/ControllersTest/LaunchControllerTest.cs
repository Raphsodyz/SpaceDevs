
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using Data.Materializated.Views;
using Microsoft.AspNetCore.Mvc;
using Services.Controllers;
using Tests.Test.Objects;

namespace Tests.ControllersTest
{
    public class LaunchControllerTest
    {

        [Fact]
        public void LaunchersController_GetById_FoundObject()
        {
            //Arrange
            var launchApiBusiness = new Mock<ILaunchApiBusiness>();

            Guid id = new(TestLaunchViewObjects.Test1().Id.ToString());
            launchApiBusiness.Setup(a => a.GetOneLaunch(id)).ReturnsAsync(TestLaunchViewObjects.Test1());
            var controller = new LaunchController(launchApiBusiness.Object);

            //Act
            var result = controller.GetById(id).Result as OkObjectResult;

            //Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.IsType<LaunchView>(result.Value);
        }

        private static IList<ValidationResult> ValidateObject(LaunchView view)
        {
            var validate = new List<ValidationResult>();
            var context = new ValidationContext(view, null, null);
            Validator.TryValidateObject(view, context, validate, true);
            return validate;
        }
    }
}