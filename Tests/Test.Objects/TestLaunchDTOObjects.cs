using Business.DTO.Entities;

namespace Tests.Test.Objects
{
    public static class TestLaunchDTOObjects
    {
        public static LaunchDTO Test1()
        {
            return new LaunchDTO()
            {
                Id = new Guid("bb9c0abd-767f-4844-8e47-d02252d3415b"),
                Url = "https://ll.thespacedevs.com/2.2.0/launch/bb9c0abd-767f-4844-8e47-d02252d3415b/",
                Launch_Library_Id = null,
                Slug = "kosmos-11k63-ds-u2-m-2",
                Name = "Kosmos 11K63 | DS-U2-M 2",
                Status = new StatusDTO()
                {
                    IdFromApi = 3,
                    Name = "Launch Successful",
                    Abbrev = "Success",
                    Description = "The launch vehicle successfully inserted its payload(s) into the target orbit(s)."
                },
                Net = new DateTime(1967, 03, 03, 06, 44, 58),
                Window_Start = new DateTime(1967, 03, 03, 06, 44, 58),
                Window_End = new DateTime(1967, 03, 03, 06, 44, 58),
                Inhold = null,
                TbdTime = null,
                TbdDate = null,
                Probability = null,
                HoldReason = null,
                FailReason = null,
                Hashtag = null,
                Launch_Service_Provider = new LaunchServiceProviderDTO()
                {
                    IdFromApi = 66,
                    Url = "https://ll.thespacedevs.com/2.2.0/agencies/66/",
                    Name = "Soviet Space Program",
                    Type = "Government"
                },
                Rocket = new RocketDTO()
                {
                    IdFromApi = 3628,
                    Configuration = new ConfigurationDTO()
                    {
                        IdFromApi = 327,
                        Url = "https://ll.thespacedevs.com/2.2.0/config/launcher/327/",
                        Name = "Kosmos 11K63",
                        Family = "Kosmos",
                        full_name = "Kosmos 11K63",
                        Variant = "11K63"
                    }
                },
                Mission = new MissionDTO()
                {
                    IdFromApi = 2046,
                    Name = "DS-U2-M 2",
                    Description = "The DS-U2-M satellites were Soviet technological demonstrations satellites launched as part of the Dnepropetrovsk Sputnik program. They tested atomic clocks in space.",
                    Type = "Communications",
                    Orbit = new OrbitDTO()
                    {
                        IdFromApi = 8,
                        Name = "Low Earth Orbit",
                        Abbrev = "LEO"
                    }
                },
                Pad = new PadDTO()
                {
                    IdFromApi = 139,
                    Url = "https://ll.thespacedevs.com/2.2.0/pad/139/",
                    Agency_Id = null,
                    Name = "86/1",
                    Info_Url = null,
                    Wiki_Url = "https://en.wikipedia.org/wiki/Kapustin_Yar",
                    Map_Url = "https://www.google.com/maps?q=48.56935,46.293219",
                    Latitude = 48.56935,
                    Longitude = 46.293219,
                    Location = new LocationDTO()
                    {
                        IdFromApi = 139,
                        Url = "https://ll.thespacedevs.com/2.2.0/location/30/",
                        Name = "Kapustin Yar, Russian Federation",
                        Country_Code = "RUS",
                        Map_Image = "https://spacelaunchnow-prod-east.nyc3.digitaloceanspaces.com/media/launch_images/location_30_20200803142515.jpg",
                        Total_Launch_Count = 101,
                        Total_Landing_Count = 0
                    },
                    Map_Image = "https://spacelaunchnow-prod-east.nyc3.digitaloceanspaces.com/media/launch_images/pad_139_20200803143345.jpg",
                    Total_Launch_Count = 26
                },
                Webcast_Live = false,
                Image = null,
                Infographic = null,
                Programs = null
            };
        }
    
        public static LaunchDTO Test2()
        {
            return new LaunchDTO()
            {
                Id = new Guid("8ce5a4c6-8ab4-40e1-af08-924da819d4b1"),
                Url = "https://ll.thespacedevs.com/2.2.0/launch/8ce5a4c6-8ab4-40e1-af08-924da819d4b1/",
                Launch_Library_Id = null,
                Slug = "thor-delta-l-pioneer-e",
                Name = "Thor Delta L | Pioneer E",
                Status = new StatusDTO()
                {
                    IdFromApi = 4,
                    Name = "Launch Failure",
                    Abbrev = "Failure",
                    Description = "Either the launch vehicle did not reach orbit, or the payload(s) failed to separate."
                },
                Net = new DateTime(1969, 08, 27, 21, 59, 00),
                Window_End = new DateTime(1969, 08, 27, 21, 59, 00),
                Window_Start = new DateTime(1969, 08, 27, 21, 59, 00),
                Inhold = null,
                TbdDate = null,
                TbdTime = null,
                Probability = null,
                HoldReason = null,
                FailReason = null,
                Hashtag = null,
                Launch_Service_Provider = new LaunchServiceProviderDTO()
                {
                    IdFromApi = 161,
                    Url = "https://ll.thespacedevs.com/2.2.0/agencies/161/",
                    Name = "United States Air Force",
                    Type = "Government"
                },
                Rocket = new RocketDTO()
                {
                    IdFromApi = 3937,
                    Configuration = new ConfigurationDTO()
                    {
                        IdFromApi = 408,
                        Url = "https://ll.thespacedevs.com/2.2.0/config/launcher/408/",
                        Name = "Thor Delta L",
                        Family = "Thor",
                        full_name = "Thor Delta L",
                        Variant = "Delta L"
                    }
                },
                Mission = new MissionDTO()
                {
                    IdFromApi = 2355,
                    Launch_Library_Id = null,
                    Name = "Pioneer E",
                    Description = "Pioneer A to E (Pioneer 6 to 9 after launch) were a series of five solar-orbiting, spin-stabilized, solar-cell and battery-powered satellites designed to obtain measurements of interplanetary phenomena from widely separated points in space on a continuing basis.",
                    Type = "Astrophysics",
                    Orbit = new OrbitDTO()
                    {
                        IdFromApi = 6,
                        Name = "Heliocentric N/A",
                        Abbrev = "Helio-N/A"
                    }
                },
                Pad = new PadDTO()
                {
                    IdFromApi = 14,
                    Url = "https://ll.thespacedevs.com/2.2.0/pad/14/",
                    Agency_Id = null,
                    Name = "Space Launch Complex 17A",
                    Info_Url = null,
                    Wiki_Url = "https://en.wikipedia.org/wiki/Cape_Canaveral_Space_Launch_Complex_17",
                    Map_Url = "https://www.google.com/maps?q=28.4472,-80.565",
                    Latitude = 28.4472,
                    Longitude = -80.565,
                    Location = new LocationDTO()
                    {
                        IdFromApi = 12,
                        Url = "https://ll.thespacedevs.com/2.2.0/location/12/",
                        Name = "Cape Canaveral, FL, USA",
                        Country_Code = "USA",
                        Map_Image = "https://spacelaunchnow-prod-east.nyc3.digitaloceanspaces.com/media/launch_images/location_12_20200803142519.jpg",
                        Total_Landing_Count = 42,
                        Total_Launch_Count = 927
                    },
                    Map_Image = "https://spacelaunchnow-prod-east.nyc3.digitaloceanspaces.com/media/launch_images/pad_14_20200803143451.jpg",
                    Total_Launch_Count = 145
                },
                Webcast_Live = false,
                Image = null,
                Infographic = null,
                Programs = null
            };
        }
    
        public static LaunchDTO Test3()
        {
            return new LaunchDTO()
            {
                Id = new Guid("d849ffac-babd-4c86-a61c-3ec6b0927a0a"),
                Url = "https://ll.thespacedevs.com/2.2.0/launch/d849ffac-babd-4c86-a61c-3ec6b0927a0a/",
                Launch_Library_Id = null,
                Slug = "soyuz-u-zenit-4mkm-36",
                Name = "Soyuz U | Zenit-4MKM 36",
                Status = new StatusDTO()
                {
                    IdFromApi = 3,
                    Name = "Launch Successful",
                    Abbrev = "Success",
                    Description = "The launch vehicle successfully inserted its payload(s) into the target orbit(s)."
                },
                Net = new DateTime(1980, 02, 21, 12, 00, 00),
                Window_End = new DateTime(1980, 02, 21, 12, 00, 00),
                Window_Start = new DateTime(1980, 02, 21, 12, 00, 00),
                Inhold = null,
                TbdDate = null,
                TbdTime = null,
                Probability = null,
                HoldReason = null,
                FailReason = null,
                Hashtag = null,
                Launch_Service_Provider = new LaunchServiceProviderDTO()
                {
                    IdFromApi = 66,
                    Url = "https://ll.thespacedevs.com/2.2.0/agencies/66/",
                    Name = "Soviet Space Program",
                    Type = "Government"
                },
                Rocket = new RocketDTO()
                {
                    IdFromApi = 5153,
                    Configuration = new ConfigurationDTO()
                    {
                        IdFromApi = 37,
                        Launch_Library_Id = null,
                        Url = "https://ll.thespacedevs.com/2.2.0/config/launcher/37/",
                        Name = "Soyuz U",
                        Family = "Soyuz",
                        full_name = "Soyuz U",
                        Variant = string.Empty
                    }
                },
                Mission = new MissionDTO()
                {
                    IdFromApi = 3564,
                    Launch_Library_Id = null,
                    Name = "Zenit-4MKM 36",
                    Description = "High-resolution film-return Zenit reconnaissance satellite",
                    Type = "Government/Top Secret",
                    Orbit = new OrbitDTO()
                    {
                        IdFromApi = 8,
                        Name = "Low Earth Orbit",
                        Abbrev = "LEO"
                    }
                },
                Pad = new PadDTO()
                {
                    IdFromApi = 85,
                    Url = "https://ll.thespacedevs.com/2.2.0/pad/85/",
                    Agency_Id = 163,
                    Name = "43/4 (43R)",
                    Info_Url = null,
                    Wiki_Url = string.Empty,
                    Map_Url = "https://www.google.com/maps?q=62.92883,40.457098",
                    Latitude = 62.92883,
                    Longitude = 40.457098,
                    Location = new LocationDTO()
                    {
                        IdFromApi = 6,
                        Url = "https://ll.thespacedevs.com/2.2.0/location/6/",
                        Name = "Plesetsk Cosmodrome, Russian Federation",
                        Country_Code = "RUS",
                        Map_Image = "https://spacelaunchnow-prod-east.nyc3.digitaloceanspaces.com/media/launch_images/location_6_20200803142434.jpg",
                        Total_Launch_Count = 1666,
                        Total_Landing_Count = 0
                    },
                    Map_Image = "https://spacelaunchnow-prod-east.nyc3.digitaloceanspaces.com/media/launch_images/pad_85_20200803143554.jpg",
                    Total_Launch_Count = 318
                },
                Webcast_Live = false,
                Image = "https://spacelaunchnow-prod-east.nyc3.digitaloceanspaces.com/media/launcher_images/soyuz2520u_image_20190222031023.jpeg",
                Infographic = null,
                Programs = null
            };
        }
    }
}