using System.Linq.Expressions;
using Tests.Database;
using Tests.Test.Objects;

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

        [Fact]
        public async Task GenericRepository_GetAll_WithWhereClause()
        {
            //Arrange
            List<Expression<Func<Launch, bool>>> qryByStatus = new ()
            { l => l.IdStatus == TestLaunchInMemoryObjects.Test1().IdStatus };

            // Act
            var result = await _fixture.Launch.GetAll(filters: qryByStatus);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<Launch>>(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GenericRepository_GetAll_WithLimitClause()
        {
            //Arrange & Act
            var result = await _fixture.Launch.GetAll(howMany: 1);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<Launch>>(result);
            Assert.Equal(1, result.Count);
        }

        [Fact]
        public async Task GenericRepository_GetAll_WithOrderByClause()
        {
            //Arrange & Act
            var result = await _fixture.Launch.GetAll(orderBy: q => q.OrderBy(l => l.Name));

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<Launch>>(result);
            Assert.Equal(3, result.Count);
            Assert.Equal("Kosmos 11K63 | DS-U2-M 2", result[0].Name);
            Assert.Equal("Soyuz U | Zenit-4MKM 36", result[1].Name);
            Assert.Equal("Thor Delta L | Pioneer E", result[2].Name);
        }

        [Fact]
        public async Task GenericRepository_GetAll_WithInnerJoinClause()
        {
            //Arrange & Act
            var result = await _fixture.Launch.GetAll(includedProperties: "Status");

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<Launch>>(result);
            Assert.Equal(3, result.Count);
            Assert.NotNull(result[0].Status);
            Assert.NotNull(result[1].Status);
            Assert.NotNull(result[2].Status);
        }

        [Fact]
        public async Task GenericRepository_GetAllSelectedColumns_WithSelectAndWhereClause()
        {
            //Arrange
            List<Expression<Func<Launch, bool>>> qryByStatus = new ()
            { l => l.IdStatus == TestLaunchInMemoryObjects.Test1().IdStatus };

            //Act
            var result = await _fixture.Launch.GetAllSelectedColumns(filters: qryByStatus, selectColumns: l => l.Name);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<string>>(result);
            Assert.Equal(2, result.Count());
            Assert.Equal("Kosmos 11K63 | DS-U2-M 2", result.ElementAt(0));
            Assert.Equal("Soyuz U | Zenit-4MKM 36", result.ElementAt(1));
        }

        [Fact]
        public async Task GenericRepository_GetAllSelectedColumns_WithSelectWhereAndOrderBy()
        {
            //Arrange
            List<Expression<Func<Launch, bool>>> qryByStatus = new ()
            { l => l.IdStatus == TestLaunchInMemoryObjects.Test1().IdStatus };

            //Act
            var result = await _fixture.Launch.GetAllSelectedColumns(
                filters: qryByStatus,
                selectColumns: l => l.Name,
                orderBy: q => q.OrderByDescending(l => l.Name));

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<string>>(result);
            Assert.Equal(2, result.Count());
            Assert.Equal("Kosmos 11K63 | DS-U2-M 2", result.ElementAt(1));
            Assert.Equal("Soyuz U | Zenit-4MKM 36", result.ElementAt(0));
        }
    }
}