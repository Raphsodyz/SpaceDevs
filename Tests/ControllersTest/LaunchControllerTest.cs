using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Services.Controllers;

namespace Tests.ControllersTest
{
    public class LaunchControllerTest
    {
        [Fact]
        public void LaunchersController_GetById_OkObject()
        {
            //Arrange
            Guid launchId = Guid.NewGuid();

            var launchApiBusiness = new Mock<ILaunchApiBusiness>();
            launchApiBusiness.Setup(l => l.GetOneLaunch(launchId)).ReturnsAsync(LaunchDTOTest);

            var controller = new LaunchController(launchApiBusiness.Object);

            //Act
            var result = controller.GetById(launchId).Result;

            //Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.IsType<LaunchDTO>((result as OkObjectResult).Value);
            Assert.Equal(((result as OkObjectResult).Value as LaunchDTO).Id, new Guid("e3df2ecd-c239-472f-95e4-2b89b4f75800"));
        }



        internal Launch LaunchTest = new()
        {
            Id = Guid.NewGuid(),
            ImportedT = DateTime.Now,
            EntityStatus = EStatus.PUBLISHED.GetDisplayName(),
            AtualizationDate = DateTime.Now,
            ApiGuId = new Guid("e3df2ecd-c239-472f-95e4-2b89b4f75800"),
            Url = "https://ll.thespacedevs.com/2.2.0/launch/e3df2ecd-c239-472f-95e4-2b89b4f75800/",
            LaunchLibraryId = null,
            Slug = "sputnik-8k74ps-sputnik-1",
            Name = "Sputnik 8K74PS | Sputnik 1",
            IdStatus = new Guid("e3df2ecd-c239-472f-95e4-2b89b4f75801"),
            Status = new Status
            {
                Id = new Guid("e3df2ecd-c239-472f-95e4-2b89b4f75801"),
                Name = "Launch Successful",
                Abbrev = "Success",
                Description = "The launch vehicle successfully inserted its payload(s) into the target orbit(s)."
            },
            Net = new DateTime(1957, 10, 04),
            WindowEnd = new DateTime(1957, 10, 04),
            WindowStart = new DateTime(1957, 10, 04),
            Probability = null,
            HoldReason = null,
            FailReason = null,
            Hashtag = null,
            IdLaunchServiceProvider = new Guid("e3df2ecd-c239-472f-95e4-2b89b4f75802"),
            LaunchServiceProvider = new LaunchServiceProvider
            {
                Id = new Guid("e3df2ecd-c239-472f-95e4-2b89b4f75802"),
                Url = "https://ll.thespacedevs.com/2.2.0/agencies/66/",
                Name = "Soviet Space Program",
                Type = "Government"
            },
            IdRocket = new Guid("e3df2ecd-c239-472f-95e4-2b89b4f75803"),
            Rocket = new Rocket
            {
                Id = new Guid("e3df2ecd-c239-472f-95e4-2b89b4f75803"),
                IdConfiguration = new Guid("e3df2ecd-c239-472f-95e4-2b89b4f75804"),
                Configuration = new Configuration
                {
                    Id = new Guid("e3df2ecd-c239-472f-95e4-2b89b4f75804"),
                    Url = "https://ll.thespacedevs.com/2.2.0/config/launcher/468/",
                    Name = "Sputnik 8K74PS",
                    Family = "Sputnik",
                    FullName = "Sputnik 8K74PS",
                    Variant = "8K74PS"
                }
            },
            IdMission = new Guid("e3df2ecd-c239-472f-95e4-2b89b4f75805"),
            Mission = new Mission
            {
                Id = new Guid("e3df2ecd-c239-472f-95e4-2b89b4f75805"),
                Name = "Sputnik 1",
                Description = "First artificial satellite consisting of a 58 cm pressurized aluminium shell containing two 1 W transmitters for a total mass of 83.6 kg.",
                LaunchDesignator = null,
                Type = "Test Flight",
                IdOrbit = new Guid("e3df2ecd-c239-472f-95e4-2b89b4f75806"),
                Orbit = new Orbit
                {
                    Id = new Guid("e3df2ecd-c239-472f-95e4-2b89b4f75806"),
                    Name = "Low Earth Orbit",
                    Abbrev = "LEO"
                }
            },
            IdPad = new Guid("e3df2ecd-c239-472f-95e4-2b89b4f75807"),
            Pad = new Pad
            {
                Id = new Guid("e3df2ecd-c239-472f-95e4-2b89b4f75807"),
                Url = "https://ll.thespacedevs.com/2.2.0/pad/32/",
                AgencyId = null,
                Name = "1/5",
                InfoUrl = null,
                WikiUrl = string.Empty,
                MapUrl = "https://www.google.com/maps?q=45.92,63.342",
                Latitude = 45.92,
                Longitude = 63.342,
                IdLocation = new Guid("e3df2ecd-c239-472f-95e4-2b89b4f75808"),
                Location = new Location
                {
                    Id = new Guid("e3df2ecd-c239-472f-95e4-2b89b4f75808"),
                    Url = "https://ll.thespacedevs.com/2.2.0/location/15/",
                    Name = "Baikonur Cosmodrome, Republic of Kazakhstan",
                    CountryCode = "KAZ",
                    MapImage = "https://spacelaunchnow-prod-east.nyc3.digitaloceanspaces.com/media/launch_images/location_15_20200803142517.jpg",
                    TotalLandingCount = 0,
                    TotalLaunchCount = 1542
                },
                MapImage = "https://spacelaunchnow-prod-east.nyc3.digitaloceanspaces.com/media/launch_images/pad_32_20200803143513.jpg",
                TotalLaunchCount = 487
            },
            WebcastLive = false,
            Image = "https://spacelaunchnow-prod-east.nyc3.digitaloceanspaces.com/media/launcher_images/sputnik_8k74ps_image_20210830185541.jpg",
            Infographic = null,
            Programs = null
        };

        internal LaunchDTO LaunchDTOTest = new()
        {
            Id = new Guid("e3df2ecd-c239-472f-95e4-2b89b4f75800"),
            Url = "https://ll.thespacedevs.com/2.2.0/launch/e3df2ecd-c239-472f-95e4-2b89b4f75800/",
            Launch_Library_Id = null,
            Slug = "sputnik-8k74ps-sputnik-1",
            Name = "Sputnik 8K74PS | Sputnik 1",
            Status = new StatusDTO
            {
                IdFromApi = 3,
                Name = "Launch Successful",
                Abbrev = "Success",
                Description = "The launch vehicle successfully inserted its payload(s) into the target orbit(s)."
            },
            Net = new DateTime(1957, 10, 04),
            Window_End = new DateTime(1957, 10, 04),
            Window_Start = new DateTime(1957, 10, 04),
            Probability = null,
            HoldReason = null,
            FailReason = null,
            Hashtag = null,
            Launch_Service_Provider = new LaunchServiceProviderDTO
            {
                IdFromApi = 66,
                Url = "https://ll.thespacedevs.com/2.2.0/agencies/66/",
                Name = "Soviet Space Program",
                Type = "Government"
            },
            Rocket = new RocketDTO
            {
                IdFromApi = 3003,
                Configuration = new ConfigurationDTO
                {
                    IdFromApi = 468,
                    Url = "https://ll.thespacedevs.com/2.2.0/config/launcher/468/",
                    Name = "Sputnik 8K74PS",
                    Family = "Sputnik",
                    full_name = "Sputnik 8K74PS",
                    Variant = "8K74PS"
                }
            },
            Mission = new MissionDTO
            {
                IdFromApi = 1430,
                Name = "Sputnik 1",
                Description = "First artificial satellite consisting of a 58 cm pressurized aluminium shell containing two 1 W transmitters for a total mass of 83.6 kg.",
                Type = "Test Flight",
                Orbit = new OrbitDTO
                {
                    IdFromApi = 8,
                    Name = "Low Earth Orbit",
                    Abbrev = "LEO"
                }
            },
            Pad = new PadDTO
            {
                IdFromApi = 32,
                Url = "https://ll.thespacedevs.com/2.2.0/pad/32/",
                Agency_Id = null,
                Name = "1/5",
                Info_Url = null,
                Wiki_Url = string.Empty,
                Map_Url = "https://www.google.com/maps?q=45.92,63.342",
                Latitude = 45.92,
                Longitude = 63.342,
                Location = new LocationDTO
                {
                    IdFromApi = 15,
                    Url = "https://ll.thespacedevs.com/2.2.0/location/15/",
                    Name = "Baikonur Cosmodrome, Republic of Kazakhstan",
                    Country_Code = "KAZ",
                    Map_Image = "https://spacelaunchnow-prod-east.nyc3.digitaloceanspaces.com/media/launch_images/location_15_20200803142517.jpg",
                    Total_Landing_Count = 0,
                    Total_Launch_Count = 1542
                },
                Map_Image = "https://spacelaunchnow-prod-east.nyc3.digitaloceanspaces.com/media/launch_images/pad_32_20200803143513.jpg",
                Total_Launch_Count = 487
            },
            Webcast_Live = false,
            Image = "https://spacelaunchnow-prod-east.nyc3.digitaloceanspaces.com/media/launcher_images/sputnik_8k74ps_image_20210830185541.jpg",
            Infographic = null,
            Programs = null
        };
    }
}