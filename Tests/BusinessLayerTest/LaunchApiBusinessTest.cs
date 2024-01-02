using Tests.Database;

namespace Tests.BusinessLayerTest
{
    [Collection("Database Launch")]
    public class LaunchApiBusinessTest
    {
        private readonly TestDatabaseFixture _context;
        public LaunchApiBusinessTest(TestDatabaseFixture context)
        {
            _context = context;
        }
    }
}