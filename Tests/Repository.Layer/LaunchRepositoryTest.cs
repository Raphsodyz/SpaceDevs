using Tests.Database;

namespace Tests.Repository.Layer
{
    public class LaunchRepositoryTest : IClassFixture<TestDatabaseFixture>
    {
        private readonly TestDatabaseFixture _fixture;
        public LaunchRepositoryTest(TestDatabaseFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task GenericRepository_GetAll_NoParameters()
        {
            // Arrange & Act
            var result = await _fixture.Launch.GetAll();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<Launch>>(result);
            Assert.Equal(3, result.Count);
        }
    }
}