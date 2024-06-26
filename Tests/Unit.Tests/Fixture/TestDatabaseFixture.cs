using AutoMapper;
using Domain.Interface;
using Infrastructure.Persistence.Context;
using Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Tests.Test.Objects;
using Z.EntityFramework.Extensions;

namespace Tests.Unit.Tests.Fixture
{
    public class TestDatabaseFixture : IDisposable
    {
        public readonly FutureSpaceContext Context;
        private readonly DbContextOptions<FutureSpaceContext> Options;
        public readonly ILaunchRepository Launch;

        public TestDatabaseFixture()
        {
            Options = new DbContextOptionsBuilder<FutureSpaceContext>()
                .UseInMemoryDatabase("futurespacecommanddbtest")
                .ConfigureWarnings(warn => warn.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;
            Context = new FutureSpaceContext(Options);

            Launch = new LaunchRepository(Context, new Mock<IMapper>().Object);
            EntityFrameworkManager.ContextFactory = context => Context; //SetUp context for zzz.EntityFramework Extension.

            if (Context.Launch.Any())
            {
                Context.Database.EnsureDeleted();
                Context.Database.EnsureCreated();
            }

            SeedDatabase();
        }

        private void SeedDatabase()
        {
            if (!Context.Launch.Any())
            {
                Context.Launch.AddRange(TestLaunchInMemoryObjects.Test1(), TestLaunchInMemoryObjects.Test2(), TestLaunchInMemoryObjects.Test3());
                Context.SaveChanges();

                DetachEntitiesEfChangeTracker();
            }
        }

        public void DetachEntitiesEfChangeTracker()
        {
            var entries = Context.ChangeTracker.Entries().ToList();
                foreach (var entry in entries)
                    entry.State = EntityState.Detached;
        }

        public Launch NewObjectForSaveTests()
        {
            return new Launch()
            {
                AtualizationDate = new DateTime(2024, 04, 21, 17, 54, 00),
                ImportedT = new DateTime(2024, 04, 21, 17, 54, 00),
                EntityStatus = "PUBLISHED",
                ApiGuid = new Guid("aa72a4aa-bf77-4de2-b21a-76cd8b6f2ad0"),
                Url = "https://ll.thespacedevs.com/2.2.0/launch/aa72a4aa-bf77-4de2-b21a-76cd8b6f2ad0/",
                LaunchLibraryId = null,
                Slug = "soyuz-u-yantar-4k1-8",
                Name = "Soyuz U | Yantar-4K1 8",
                Status = new Status()
                {
                    Name = "Launch Successful",
                    Abbrev = "Success",
                    Description = "The launch vehicle successfully inserted its payload(s) into the target orbit(s)."
                },
                Net = new DateTime(1983, 04, 26, 10, 00, 00),
                WindowEnd = new DateTime(1983, 04, 26, 10, 00, 00),
                WindowStart = new DateTime(1983, 04, 26, 10, 00, 00),
                Inhold = null,
                TbdTime = null,
                TbdDate = null,
                Probability = null,
                HoldReason = "",
                FailReason = null,
                Hashtag = null,
                LaunchServiceProvider = new LaunchServiceProvider()
                {
                    Url = "https://ll.thespacedevs.com/2.2.0/agencies/66/",
                    Name = "Soviet Space Program",
                    Type = "Government"
                },
                Rocket = new Rocket()
                {
                    Configuration = new Configuration()
                    {
                        LaunchLibraryId = null,
                        Url = "https://ll.thespacedevs.com/2.2.0/config/launcher/37/",
                        Name = "Soyuz U",
                        Family = "Soyuz",
                        FullName = "Soyuz U",
                        Variant = ""
                    }
                },
                Mission = new Mission()
                {
                    LaunchLibraryId = null,
                    Name = "Yantar-4K1 8",
                    Description = "Second generation high resolution film-return Yantar reconnaissance satellite",
                    Type = "Government/Top Secret",
                    Orbit = new Orbit()
                    {
                        Name = "Low Earth Orbit",
                        Abbrev = "LEO"
                    },
                    LaunchDesignator = null
                },
                Pad = new Pad()
                {
                    Url = "https://ll.thespacedevs.com/2.2.0/pad/20/",
                    AgencyId = null,
                    Name = "31/6",
                    InfoUrl = null,
                    WikiUrl = "https://en.wikipedia.org/wiki/Baikonur_Cosmodrome_Site_31",
                    MapUrl = "https://www.google.com/maps?q=45.996034,63.564003",
                    Latitude = 45.996034,
                    Longitude = 63.564003,
                    Location = new Location()
                    {
                        Url = "https://ll.thespacedevs.com/2.2.0/location/15/",
                        Name = "Baikonur Cosmodrome, Republic of Kazakhstan",
                        CountryCode = "KAZ",
                        MapImage = "https://spacelaunchnow-prod-east.nyc3.digitaloceanspaces.com/media/launch_images/location_15_20200803142517.jpg",
                        TotalLaunchCount = 1548,
                        TotalLandingCount = 0
                    },
                    MapImage = "https://spacelaunchnow-prod-east.nyc3.digitaloceanspaces.com/media/launch_images/pad_20_20200803143516.jpg",
                    TotalLaunchCount = 415
                },
                WebcastLive = false,
                Image = "https://spacelaunchnow-prod-east.nyc3.digitaloceanspaces.com/media/images/soyuz2520u_image_20190222031023.jpeg",
                Infographic = null,
                Programs = null
            };
        }

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    Context.Database.EnsureDeleted();
                    Context.Database.EnsureCreated();

                    Context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}