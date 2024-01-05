using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tests.Business.Layer
{
    public class LaunchApiBusinessTest
    {
        [Fact]
        public void LaunchApiBusiness_GetOneLaunch_ReturnOneLaunch()
        {
            //Arrange
            var launchViewBusiness  = new Mock<ILaunchViewBusiness>();

            launchViewBusiness.Setup(l => l.ViewExists()).ReturnsAsync(true);
            //launchViewBusiness.Setup(l => l.)

            //Act


            //Assert

        }
    }
}