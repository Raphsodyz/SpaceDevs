using Data.Context;
using Data.Interface;
using Data.Repository;
using Microsoft.EntityFrameworkCore;
using Tests.Test.Objects;

namespace Tests.Database
{
    public class TestDatabaseFixture : IDisposable
    {
        public readonly FutureSpaceContext _context;
        public readonly ILaunchRepository _launchRepository;

        public TestDatabaseFixture()
        {
            var options = new DbContextOptionsBuilder<FutureSpaceContext>()
                .UseInMemoryDatabase("futurespacedbtest")
                .Options;

            _context = new FutureSpaceContext(options);
            _launchRepository = new LaunchRepository(_context);

            SeedDatabase();
        }

        public void SeedDatabase()
        {
            if(!_context.Launch.Any())
            {
                _context.Launch.AddRange(TestLaunchObjects.Test1(), TestLaunchObjects.Test2(), TestLaunchObjects.Test3());
                _context.SaveChanges();
            }
        }

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Database.EnsureDeleted();
                    _context.Database.EnsureCreated();

                    _context.Dispose();
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

    [CollectionDefinition("Database Launch")]
    public class DatabaseCollection : ICollectionFixture<TestDatabaseFixture>
    {

    }
}