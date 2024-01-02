using System.Data.Common;
using Business.Business;
using Data.Context;
using Data.Interface;
using Data.Repository;
using Microsoft.EntityFrameworkCore;

namespace Tests.Database
{
    public class TestDatabaseFixture : IDisposable
    {
        public FutureSpaceContext Context { get; private set; }
        public ILaunchRepository LaunchRepository { get; }
        public IUnitOfWork Uow { get; }
        public ILaunchBusiness LaunchBusiness { get; }

        public TestDatabaseFixture()
        {
            var options = new DbContextOptionsBuilder<FutureSpaceContext>()
                .UseInMemoryDatabase("futurespacedbtest")
                .Options;

            Context = new FutureSpaceContext(options);
            Uow = new UnitOfWork(Context);
            LaunchRepository = new LaunchRepository(Context);
            LaunchBusiness = new LaunchBusiness(Uow);

            SeedDatabase();
        }

        public void SeedDatabase()
        {
            Context.Launch.Add(new Launch()
            {
                Id = new Guid("000ebc80-d782-4dee-8606-1199d9074039"),
                AtualizationDate = DateTime.Now,
                ImportedT = DateTime.Now,
                EntityStatus = EStatus.PUBLISHED.GetDisplayName(),
                ApiGuId = new Guid("bb9c0abd-767f-4844-8e47-d02252d3415b"),
                Url = "https://ll.thespacedevs.com/2.2.0/launch/bb9c0abd-767f-4844-8e47-d02252d3415b/",
                LaunchLibraryId = null,
                Slug = "kosmos-11k63-ds-u2-m-2",
                Name = "Kosmos 11K63 | DS-U2-M 2",
                IdStatus = new Guid("3932b460-1d53-4533-b960-33cbc62a1f4b"),
                Status = new Status()
                {
                    Id = new Guid("3932b460-1d53-4533-b960-33cbc62a1f4b"),
                    IdFromApi = 3,
                    ImportedT = DateTime.Now,
                    AtualizationDate = DateTime.Now,
                    EntityStatus = EStatus.PUBLISHED.GetDisplayName(),
                    Name = "Launch Successful",
                    Abbrev = "Success",
                    Description = "The launch vehicle successfully inserted its payload(s) into the target orbit(s)."
                },
                Net = new DateTime(1967, 03, 03, 06, 44, 58),
                WindowStart = new DateTime(1967, 03, 03, 06, 44, 58),
                WindowEnd = new DateTime(1967, 03, 03, 06, 44, 58),
                Inhold = null,
                TbdTime = null,
                TbdDate = null,
                Probability = null,
                HoldReason = null,
                FailReason = null,
                Hashtag = null,
                IdLaunchServiceProvider = new Guid("c9c15036-f45a-4109-95ec-fdc387fe7733"),
                LaunchServiceProvider = new LaunchServiceProvider()
                {
                    Id = new Guid("c9c15036-f45a-4109-95ec-fdc387fe7733"),
                    IdFromApi = 66,
                    ImportedT = DateTime.Now,
                    AtualizationDate = DateTime.Now,
                    EntityStatus = EStatus.PUBLISHED.GetDisplayName(),
                    Url = "https://ll.thespacedevs.com/2.2.0/agencies/66/",
                    Name = "Soviet Space Program",
                    Type = "Government"
                },
                IdRocket = new Guid("6735c96e-a46e-4fef-942c-af827e04572d"),
                Rocket = new Rocket()
                {
                    Id = new Guid("6735c96e-a46e-4fef-942c-af827e04572d"),
                    IdFromApi = 3628,
                    ImportedT = DateTime.Now,
                    AtualizationDate = DateTime.Now,
                    EntityStatus = EStatus.PUBLISHED.GetDisplayName(),
                    IdConfiguration = new Guid("a1fea86a-874f-4bfa-8317-821b200ee6e7"),
                    Configuration = new Configuration()
                    {
                        Id = new Guid("a1fea86a-874f-4bfa-8317-821b200ee6e7"),
                        IdFromApi = 327,
                        ImportedT = DateTime.Now,
                        AtualizationDate = DateTime.Now,
                        EntityStatus = EStatus.PUBLISHED.GetDisplayName(),
                        Url = "https://ll.thespacedevs.com/2.2.0/config/launcher/327/",
                        Name = "Kosmos 11K63",
                        Family = "Kosmos",
                        FullName = "Kosmos 11K63",
                        Variant = "11K63"
                    }
                },
                IdMission = new Guid("b7c69fdc-32fb-4ad6-8722-c8a864a3d61d"),
                Mission = new Mission()
                {
                    Id = new Guid("b7c69fdc-32fb-4ad6-8722-c8a864a3d61d"),
                    IdFromApi = 2046,
                    ImportedT = DateTime.Now,
                    AtualizationDate = DateTime.Now,
                    EntityStatus = EStatus.PUBLISHED.GetDisplayName(),
                    LaunchLibraryId = null,
                    Name = "DS-U2-M 2",
                    Description = "The DS-U2-M satellites were Soviet technological demonstrations satellites launched as part of the Dnepropetrovsk Sputnik program. They tested atomic clocks in space.",
                    Type = "Communications",
                    IdOrbit = new Guid("94d7a867-f918-4ad8-b93b-e0f108d66f2d"),
                    Orbit = new Orbit()
                    {
                        Id = new Guid("94d7a867-f918-4ad8-b93b-e0f108d66f2d"),
                        IdFromApi = 8,
                        ImportedT = DateTime.Now,
                        AtualizationDate = DateTime.Now,
                        EntityStatus = EStatus.PUBLISHED.GetDisplayName(),
                        Name = "Low Earth Orbit",
                        Abbrev = "LEO"
                    },
                    LaunchDesignator = null
                },
                IdPad = new Guid("a578ae03-a75a-45fa-9d9b-ddfee3c7a4e3"),
                Pad = new Pad()
                {
                    Id = new Guid("a578ae03-a75a-45fa-9d9b-ddfee3c7a4e3"),
                    IdFromApi = 139,
                    ImportedT = DateTime.Now,
                    AtualizationDate = DateTime.Now,
                    EntityStatus = EStatus.PUBLISHED.GetDisplayName(),
                    Url = "https://ll.thespacedevs.com/2.2.0/pad/139/",
                    AgencyId = null,
                    Name = "86/1",
                    InfoUrl = null,
                    WikiUrl = "https://en.wikipedia.org/wiki/Kapustin_Yar",
                    MapUrl = "https://www.google.com/maps?q=48.56935,46.293219",
                    Latitude = 48.56935,
                    Longitude = 46.293219,
                    IdLocation = new Guid("6600d3fc-f421-4e88-8509-3b9f3e69939e"),
                    Location = new Location()
                    {
                        Id = new Guid("6600d3fc-f421-4e88-8509-3b9f3e69939e"),
                        IdFromApi = 139,
                        ImportedT = DateTime.Now,
                        AtualizationDate = DateTime.Now,
                        EntityStatus = EStatus.PUBLISHED.GetDisplayName(),
                        Url = "https://ll.thespacedevs.com/2.2.0/location/30/",
                        Name = "Kapustin Yar, Russian Federation",
                        CountryCode = "RUS",
                        MapImage = "https://spacelaunchnow-prod-east.nyc3.digitaloceanspaces.com/media/launch_images/location_30_20200803142515.jpg",
                        TotalLaunchCount = 101,
                        TotalLandingCount = 0
                    },
                    MapImage = "https://spacelaunchnow-prod-east.nyc3.digitaloceanspaces.com/media/launch_images/pad_139_20200803143345.jpg",
                    TotalLaunchCount = 26
                },
                WebcastLive = false,
                Image = null,
                Infographic = null,
                Programs = null
            });

            Context.Launch.Add(new Launch()
            {
                Id = new Guid("001f670e-a06c-49dc-8797-777171f45263"),
                AtualizationDate = DateTime.Now,
                ImportedT = DateTime.Now,
                EntityStatus = EStatus.PUBLISHED.GetDisplayName(),
                ApiGuId = new Guid("8ce5a4c6-8ab4-40e1-af08-924da819d4b1"),
                Url = "https://ll.thespacedevs.com/2.2.0/launch/8ce5a4c6-8ab4-40e1-af08-924da819d4b1/",
                LaunchLibraryId = null,
                Slug = "thor-delta-l-pioneer-e",
                Name = "Thor Delta L | Pioneer E",
                IdStatus = new Guid("e9ae28ee-3202-4b73-a3fa-30f3156d1d38"),
                Status = new Status()
                {
                    Id = new Guid("e9ae28ee-3202-4b73-a3fa-30f3156d1d38"),
                    IdFromApi = 4,
                    ImportedT = DateTime.Now,
                    AtualizationDate = DateTime.Now,
                    EntityStatus = EStatus.PUBLISHED.GetDisplayName(),
                    Name = "Launch Failure",
                    Abbrev = "Failure",
                    Description = "Either the launch vehicle did not reach orbit, or the payload(s) failed to separate."
                },
                Net = new DateTime(1969, 08, 27, 21, 59, 00),
                WindowEnd = new DateTime(1969, 08, 27, 21, 59, 00),
                WindowStart = new DateTime(1969, 08, 27, 21, 59, 00),
                Inhold = null,
                TbdDate = null,
                TbdTime = null,
                Probability = null,
                HoldReason = null,
                FailReason = null,
                Hashtag = null,
                IdLaunchServiceProvider = new Guid("26f4868c-c973-4d9a-9423-ece20371bc7e"),
                LaunchServiceProvider = new LaunchServiceProvider()
                {
                    Id = new Guid("26f4868c-c973-4d9a-9423-ece20371bc7e"),
                    IdFromApi = 161,
                    ImportedT = DateTime.Now,
                    AtualizationDate = DateTime.Now,
                    EntityStatus = EStatus.PUBLISHED.GetDisplayName(),
                    Url = "https://ll.thespacedevs.com/2.2.0/agencies/161/",
                    Name = "United States Air Force",
                    Type = "Government"
                },
                IdRocket = new Guid("cfb7e247-646c-4349-a422-8742bb4668e1"),
                Rocket = new Rocket()
                {
                    Id = new Guid("cfb7e247-646c-4349-a422-8742bb4668e1"),
                    IdFromApi = 3937,
                    ImportedT = DateTime.Now,
                    AtualizationDate = DateTime.Now,
                    EntityStatus = EStatus.PUBLISHED.GetDisplayName(),
                    IdConfiguration = new Guid("2c6195be-ae6a-40ab-943f-079ffc75c7b0"),
                    Configuration = new Configuration()
                    {
                        Id = new Guid("2c6195be-ae6a-40ab-943f-079ffc75c7b0"),
                        IdFromApi = 408,
                        ImportedT = DateTime.Now,
                        AtualizationDate = DateTime.Now,
                        EntityStatus = EStatus.PUBLISHED.GetDisplayName(),
                        Url = "https://ll.thespacedevs.com/2.2.0/config/launcher/408/",
                        Name = "Thor Delta L",
                        Family = "Thor",
                        FullName = "Thor Delta L",
                        Variant = "Delta L"
                    }
                },
                IdMission = new Guid("18e959e6-21cc-4b0c-b9cd-65c95ee91598"),
                Mission = new Mission()
                {
                    Id = new Guid("18e959e6-21cc-4b0c-b9cd-65c95ee91598"),
                    IdFromApi = 2355,
                    ImportedT = DateTime.Now,
                    AtualizationDate = DateTime.Now,
                    EntityStatus = EStatus.PUBLISHED.GetDisplayName(),
                    LaunchLibraryId = null,
                    Name = "Pioneer E",
                    Description = "Pioneer A to E (Pioneer 6 to 9 after launch) were a series of five solar-orbiting, spin-stabilized, solar-cell and battery-powered satellites designed to obtain measurements of interplanetary phenomena from widely separated points in space on a continuing basis.",
                    Type = "Astrophysics",
                    IdOrbit = new Guid("4cc3a31f-5602-4156-975b-06a6bcc645ad"),
                    Orbit = new Orbit()
                    {
                        Id = new Guid("4cc3a31f-5602-4156-975b-06a6bcc645ad"),
                        IdFromApi = 6,
                        ImportedT = DateTime.Now,
                        AtualizationDate = DateTime.Now,
                        EntityStatus = EStatus.PUBLISHED.GetDisplayName(),
                        Name = "Heliocentric N/A",
                        Abbrev = "Helio-N/A"
                    },
                    LaunchDesignator = null
                },
                IdPad = new Guid("3f1eac5b-1496-4308-8e94-0e0888a56bb9"),
                Pad = new Pad()
                {
                    Id = new Guid("3f1eac5b-1496-4308-8e94-0e0888a56bb9"),
                    IdFromApi = 14,
                    ImportedT = DateTime.Now,
                    AtualizationDate = DateTime.Now,
                    EntityStatus = EStatus.PUBLISHED.GetDisplayName(),
                    Url = "https://ll.thespacedevs.com/2.2.0/pad/14/",
                    AgencyId = null,
                    Name = "Space Launch Complex 17A",
                    InfoUrl = null,
                    WikiUrl = "https://en.wikipedia.org/wiki/Cape_Canaveral_Space_Launch_Complex_17",
                    MapUrl = "https://www.google.com/maps?q=28.4472,-80.565",
                    Latitude = 28.4472,
                    Longitude = -80.565,
                    IdLocation = new Guid("89d4177c-08b2-431f-a258-d7187d9db97c"),
                    Location = new Location()
                    {
                        Id = new Guid("89d4177c-08b2-431f-a258-d7187d9db97c"),
                        IdFromApi = 12,
                        ImportedT = DateTime.Now,
                        AtualizationDate = DateTime.Now,
                        EntityStatus = EStatus.PUBLISHED.GetDisplayName(),
                        Url = "https://ll.thespacedevs.com/2.2.0/location/12/",
                        Name = "Cape Canaveral, FL, USA",
                        CountryCode = "USA",
                        MapImage = "https://spacelaunchnow-prod-east.nyc3.digitaloceanspaces.com/media/launch_images/location_12_20200803142519.jpg",
                        TotalLandingCount = 42,
                        TotalLaunchCount = 927
                    },
                    MapImage = "https://spacelaunchnow-prod-east.nyc3.digitaloceanspaces.com/media/launch_images/pad_14_20200803143451.jpg",
                    TotalLaunchCount = 145
                },
                WebcastLive = false,
                Image = null,
                Infographic = null,
                Programs = null
            });

            Context.Launch.Add(new Launch()
            {
                Id = new Guid("0022751c-b755-4d0c-a23a-294ce9c95c71"),
                AtualizationDate = DateTime.Now,
                ImportedT = DateTime.Now,
                EntityStatus = EStatus.PUBLISHED.GetDisplayName(),
                ApiGuId = new Guid("d849ffac-babd-4c86-a61c-3ec6b0927a0a"),
                Url = "https://ll.thespacedevs.com/2.2.0/launch/d849ffac-babd-4c86-a61c-3ec6b0927a0a/",
                LaunchLibraryId = null,
                Slug = "soyuz-u-zenit-4mkm-36",
                Name = "Soyuz U | Zenit-4MKM 36",
                IdStatus = new Guid("3932b460-1d53-4533-b960-33cbc62a1f4b"),
                Status = new Status()
                {
                    Id = new Guid("3932b460-1d53-4533-b960-33cbc62a1f4b"),
                    IdFromApi = 3,
                    ImportedT = DateTime.Now,
                    AtualizationDate = DateTime.Now,
                    EntityStatus = EStatus.PUBLISHED.GetDisplayName(),
                    Name = "Launch Successful",
                    Abbrev = "Success",
                    Description = "The launch vehicle successfully inserted its payload(s) into the target orbit(s)."
                },
                Net = new DateTime(1980, 02, 21, 12, 00, 00),
                WindowEnd = new DateTime(1980, 02, 21, 12, 00, 00),
                WindowStart = new DateTime(1980, 02, 21, 12, 00, 00),
                Inhold = null,
                TbdDate = null,
                TbdTime = null,
                Probability = null,
                HoldReason = null,
                FailReason = null,
                Hashtag = null,
                IdLaunchServiceProvider = new Guid("c9c15036-f45a-4109-95ec-fdc387fe7733"),
                LaunchServiceProvider = new LaunchServiceProvider()
                {
                    Id = new Guid("c9c15036-f45a-4109-95ec-fdc387fe7733"),
                    IdFromApi = 66,
                    ImportedT = DateTime.Now,
                    AtualizationDate = DateTime.Now,
                    EntityStatus = EStatus.PUBLISHED.GetDisplayName(),
                    Url = "https://ll.thespacedevs.com/2.2.0/agencies/66/",
                    Name = "Soviet Space Program",
                    Type = "Government"
                },
                IdRocket = new Guid("d86d7bc6-30ce-4baa-b4ac-4d46e9303b06"),
                Rocket = new Rocket()
                {
                    Id = new Guid("d86d7bc6-30ce-4baa-b4ac-4d46e9303b06"),
                    IdFromApi = 5153,
                    ImportedT = DateTime.Now,
                    AtualizationDate = DateTime.Now,
                    EntityStatus = EStatus.PUBLISHED.GetDisplayName(),
                    IdConfiguration = new Guid("d80ee7ed-b1ff-498e-805f-a8a5b11d38e1"),
                    Configuration = new Configuration()
                    {
                        Id = new Guid("d80ee7ed-b1ff-498e-805f-a8a5b11d38e1"),
                        IdFromApi = 37,
                        ImportedT = DateTime.Now,
                        AtualizationDate = DateTime.Now,
                        EntityStatus = EStatus.PUBLISHED.GetDisplayName(),
                        LaunchLibraryId = null,
                        Url = "https://ll.thespacedevs.com/2.2.0/config/launcher/37/",
                        Name = "Soyuz U",
                        Family = "Soyuz",
                        FullName = "Soyuz U",
                        Variant = string.Empty
                    }
                },
                IdMission = new Guid("5ebb423d-988a-4645-a719-ed75491dd41f"),
                Mission = new Mission()
                {
                    Id = new Guid("5ebb423d-988a-4645-a719-ed75491dd41f"),
                    IdFromApi = 3564,
                    ImportedT = DateTime.Now,
                    AtualizationDate = DateTime.Now,
                    EntityStatus = EStatus.PUBLISHED.GetDisplayName(),
                    LaunchLibraryId = null,
                    Name = "Zenit-4MKM 36",
                    Description = "High-resolution film-return Zenit reconnaissance satellite",
                    Type = "Government/Top Secret",
                    IdOrbit = new Guid("94d7a867-f918-4ad8-b93b-e0f108d66f2d"),
                    Orbit = new Orbit()
                    {
                        Id = new Guid("94d7a867-f918-4ad8-b93b-e0f108d66f2d"),
                        IdFromApi = 8,
                        ImportedT = DateTime.Now,
                        AtualizationDate = DateTime.Now,
                        EntityStatus = EStatus.PUBLISHED.GetDisplayName(),
                        Name = "Low Earth Orbit",
                        Abbrev = "LEO"
                    },
                    LaunchDesignator = null
                },
                IdPad = new Guid("42796995-8d7e-4bf2-8f34-6d739c967204"),
                Pad = new Pad()
                {
                    Id = new Guid("42796995-8d7e-4bf2-8f34-6d739c967204"),
                    IdFromApi = 85,
                    ImportedT = DateTime.Now,
                    AtualizationDate = DateTime.Now,
                    EntityStatus = EStatus.PUBLISHED.GetDisplayName(),
                    Url = "https://ll.thespacedevs.com/2.2.0/pad/85/",
                    AgencyId = 163,
                    Name = "43/4 (43R)",
                    InfoUrl = null,
                    WikiUrl = string.Empty,
                    MapUrl = "https://www.google.com/maps?q=62.92883,40.457098",
                    Latitude = 62.92883,
                    Longitude = 40.457098,
                    IdLocation = new Guid("e12c85df-3353-44f9-b728-331a33ef8032"),
                    Location = new Location()
                    {
                        Id = new Guid("e12c85df-3353-44f9-b728-331a33ef8032"),
                        IdFromApi = 6,
                        ImportedT = DateTime.Now,
                        AtualizationDate = DateTime.Now,
                        EntityStatus = EStatus.PUBLISHED.GetDisplayName(),
                        Url = "https://ll.thespacedevs.com/2.2.0/location/6/",
                        Name = "Plesetsk Cosmodrome, Russian Federation",
                        CountryCode = "RUS",
                        MapImage = "https://spacelaunchnow-prod-east.nyc3.digitaloceanspaces.com/media/launch_images/location_6_20200803142434.jpg",
                        TotalLaunchCount = 1666,
                        TotalLandingCount = 0
                    },
                    MapImage = "https://spacelaunchnow-prod-east.nyc3.digitaloceanspaces.com/media/launch_images/pad_85_20200803143554.jpg",
                    TotalLaunchCount = 318
                },
                WebcastLive = false,
                Image = "https://spacelaunchnow-prod-east.nyc3.digitaloceanspaces.com/media/launcher_images/soyuz2520u_image_20190222031023.jpeg",
                Infographic = null,
                Programs = null
            });

            Context.SaveChanges();
        }

        public void Dispose()
        {
            Context.Database.EnsureDeleted();
            Context.Database.EnsureCreated();

            Context.Dispose();
        }
    }

    [CollectionDefinition("Database Launch")]
    public class DatabaseCollection : ICollectionFixture<TestDatabaseFixture>
    {

    }
}