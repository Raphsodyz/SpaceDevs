using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Services.Controllers;
using Tests.Database;

namespace Tests.ControllersTest
{
    [Collection("Database Launch")]
    public class LaunchControllerTest
    {
        private readonly TestDatabaseFixture _context;
        public LaunchControllerTest(TestDatabaseFixture context)
        {
            _context = context;
        }

        [Fact]
        public void LaunchersController_GetById_FoundObject()
        {
            //Arrange
            var launchApiBusiness = new Mock<ILaunchApiBusiness>();
            var launchBusiness = new Mock<ILaunchViewBusiness>();

            launchBusiness.Setup(l => l.ViewExists()).ReturnsAsync(true);

            var controller = new LaunchController(launchApiBusiness.Object);

            launchApiBusiness.Setup(l => l.GetOneLaunch(new Guid("000ebc80-d782-4dee-8606-1199d9074039")));

            //Act
            

            //Assert


        }
    }
}