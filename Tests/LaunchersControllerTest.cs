using Application.DTO;
using Business.Business;
using Business.Interface;
using Data.Interface;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Services.Controllers;

namespace Tests
{
    public class LaunchersControllerTest
    {
        [Fact]
        public void GetOneLaunch()
        {
            //Arrange
            var launchApiBusiness = new Mock<ILaunchApiBusiness>();
            var controller = new LaunchController(launchApiBusiness.Object);
            int idDesiredObject = 1;

            var repository = Mock.Of<IRepository>

            launchApiBusiness.Setup(s => s.GetOneLaunch(idDesiredObject)).Callback(() => {  });

            //Act
            var result = controller.GetById(idDesiredObject) as ObjectResult;
            var model = result.Value as LaunchDTO;

            //Assert
            Assert.NotNull(result);
            Assert.True(result.Value is LaunchDTO);
            //replace for your data in database.
            Assert.Equal(model.Name, "Sputnik 8K74PS | Sputnik 1");
            Assert.Equal(model.Slug, "sputnik-8k74ps-sputnik-1");
        }
    }
}