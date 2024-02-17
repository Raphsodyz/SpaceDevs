using Data.Context;
using Data.Interface;
using Data.Repository;
using Microsoft.EntityFrameworkCore;
using Tests.Test.Objects;

namespace Tests.Database
{
    public class TestDatabaseFixture : IDisposable
    {
        public readonly FutureSpaceContext Context;
        public readonly ILaunchRepository Launch;

        public TestDatabaseFixture()
        {
            var options = new DbContextOptionsBuilder<FutureSpaceContext>()
                .UseInMemoryDatabase("futurespacedbtest")
                .Options;

            Context = new FutureSpaceContext(options);
            Launch = new LaunchRepository(Context);

            if(Context.Launch.Any())
            {
                Context.Database.EnsureDeleted();
                Context.Database.EnsureCreated();
            }
            
            SeedDatabase();
        }

        private void SeedDatabase()
        {
            if(!Context.Launch.Any())
            {
                Context.Launch.AddRange(TestLaunchInMemoryObjects.Test1(), TestLaunchInMemoryObjects.Test2(), TestLaunchInMemoryObjects.Test3());
                Context.SaveChanges();
            }
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