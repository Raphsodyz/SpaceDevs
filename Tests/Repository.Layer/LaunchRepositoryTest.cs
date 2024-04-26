using System.Linq.Expressions;
using Tests.Fixture;
using Tests.Test.Objects;
using Z.EntityFramework.Extensions;

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
        public async Task GenericRepository_GetAll_WithJoinClause()
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
            var result = await _fixture.Launch.GetAllSelectedColumns(
                filters: qryByStatus,
                selectColumns: l => l.Name,
                buildObject: l => l);

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IEnumerable<string>>(result);
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
                buildObject: l => l,
                orderBy: q => q.OrderByDescending(l => l.Name));

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IEnumerable<string>>(result);
            Assert.Equal(2, result.Count());
            Assert.Equal("Kosmos 11K63 | DS-U2-M 2", result.ElementAt(1));
            Assert.Equal("Soyuz U | Zenit-4MKM 36", result.ElementAt(0));
        }

        [Fact]
        public async Task GenericRepository_GetAllSelectedColumns_WithSelectWhereAndJoinClause()
        {
            //Arrange
            List<Expression<Func<Launch, bool>>> qryByStatus = new ()
            { l => l.IdStatus == TestLaunchInMemoryObjects.Test1().IdStatus };

            //Act
            var result = await _fixture.Launch.GetAllSelectedColumns(
                filters: qryByStatus,
                selectColumns: l => new { l.Name, l.Status },
                buildObject: l => new { l.Name, l.Status },
                includedProperties: "Status");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.NotNull(result.ElementAt(0).Status);
            Assert.NotNull(result.ElementAt(1).Status);
        }

        [Fact]
        public async Task GenericRepository_GetAllSelectedColumns_WithSelectWhereAndLimitClause()
        {
            //Arrange
            List<Expression<Func<Launch, bool>>> qryByStatus = new ()
            { l => l.IdStatus == TestLaunchInMemoryObjects.Test1().IdStatus };

            //Act
            var result = await _fixture.Launch.GetAllSelectedColumns(
                filters: qryByStatus,
                selectColumns: l => l.Name,
                buildObject: l => l,
                howMany: 1);

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IEnumerable<string>>(result);
            Assert.Equal(1, result.Count());
        }

        [Fact]
        public async Task GenericRepository_GetAllPaged_WithPagesAndSize()
        {
            //Arrange & Act
            var result = await _fixture.Launch.GetAllPaged(page: 0, pageSize: 3);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Pagination<Launch>>(result);
            Assert.Equal(3, result.NumberOfEntities);
            Assert.Equal(1, result.NumberOfPages);
            Assert.Equal(0, result.CurrentPage);
            Assert.IsType<List<Launch>>(result.Entities);
            Assert.Equal(3, result.Entities.Count);
        }

        [Fact]
        public async Task GenericRepository_GetAllPaged_WithWhereClauseAndPageOverMaxResults()
        {
            //Arrange
            List<Expression<Func<Launch, bool>>> qyrTest = new()
            { l => l.IdStatus == TestLaunchInMemoryObjects.Test1().IdStatus };

            //Act
            var result = await _fixture.Launch.GetAllPaged(
                page: 0,
                pageSize: 10,
                filters: qyrTest
            );

            //Assert
            Assert.NotNull(result);
            Assert.IsType<Pagination<Launch>>(result);
            Assert.Equal(2, result.NumberOfEntities);
            Assert.Equal(1, result.NumberOfPages);
            Assert.Equal(0, result.CurrentPage);
            Assert.Equal(2, result.Entities.Count);
        }

        [Fact]
        public async Task GenericRepository_GetAllPaged_WithIncludeClause()
        {
            //Arrange & Act
            var result = await _fixture.Launch.GetAllPaged(
                page: 0,
                pageSize: 10,
                includedProperties: "Status");


            //Assert
            Assert.NotNull(result);
            Assert.IsType<Pagination<Launch>>(result);
            Assert.Equal(3, result.NumberOfEntities);
            Assert.Equal(1, result.NumberOfPages);
            Assert.Equal(0, result.CurrentPage);
            Assert.IsType<List<Launch>>(result.Entities);
            Assert.Equal(3, result.Entities.Count);
            Assert.NotNull(result.Entities.Select(e => e.Status));
        }

        [Fact]
        public async Task GenericRepository_EntityCount_CountAllEntities()
        {
            //Arrange & Act
            var result = await _fixture.Launch.EntityCount();

            //Assert
            Assert.NotNull(result);
            Assert.IsType<int>(result);
            Assert.Equal(3, result);
        }

        [Fact]
        public async Task GenericRepository_EntityCount_CountEntitiesWithFilter()
        {
            //Arrange
            Expression<Func<Launch, bool>> qryLaunchFilter = l => l.Id == new Guid("0022751c-b755-4d0c-a23a-294ce9c95c71");

            //Act
            var result = await _fixture.Launch.EntityCount(qryLaunchFilter);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<int>(result);
            Assert.Equal(1, result);
        }

        [Fact]
        public async Task GenericRepository_Get_GetSpecificPropWithIncludeAndWhere()
        {
            //Arrange
            Expression<Func<Launch, bool>> qryLaunchFilter = l => l.Id == new Guid("0022751c-b755-4d0c-a23a-294ce9c95c71");

            //Act
            var result = await _fixture.Launch.Get(qryLaunchFilter, "Status");

            //Assert
            Assert.NotNull(result);
            Assert.IsType<Launch>(result);
            Assert.NotNull(result.Status);
        }

        [Fact]
        public async Task GenericRepository_Get_GetAllPropsFirstFromTheClause()
        {
            //Arrange
            Expression<Func<Launch, bool>> qryFilter = l => l.EntityStatus == EStatus.PUBLISHED.GetDisplayName();

            //Act
            var result = await _fixture.Launch.Get(
                filter: qryFilter,
                includedProperties: "Status, LaunchServiceProvider, Rocket.Configuration, Mission.Orbit, Pad.Location");

            //Assert
            Assert.NotNull(result);
            Assert.IsType<Launch>(result);
            Assert.Equal(new Guid("000ebc80-d782-4dee-8606-1199d9074039"), result.Id); // first from the test launch object class.
            Assert.NotNull(result.Status);
            Assert.NotNull(result.LaunchServiceProvider);
            Assert.NotNull(result.Rocket);
            Assert.NotNull(result.Rocket.Configuration);
            Assert.NotNull(result.Mission);
            Assert.NotNull(result.Mission.Orbit);
            Assert.NotNull(result.Pad);
            Assert.NotNull(result.Pad.Location);
        }

        [Fact]
        public async Task GenericRepository_GetSelected_GetOneLaunchAndSelectHisId()
        {
            //Arrange
            Expression<Func<Launch, bool>> qryId = l => l.EntityStatus == EStatus.PUBLISHED.GetDisplayName()
                && l.Id == new Guid("000ebc80-d782-4dee-8606-1199d9074039");

            //Act
            var result = await _fixture.Launch.GetSelected(
                filter: qryId,
                selectColumns: l => l.Id,
                buildObject: l => l
            );

            //Assert
            Assert.NotNull(result);
            Assert.IsType<Guid>(result);
            Assert.Equal(TestLaunchInMemoryObjects.Test1().Id, result);
        }

        [Fact]
        public async Task GenericRepository_GetSelected_GetOneLaunchAndBuildDTO()
        {
            //Arrange
            Expression<Func<Launch, bool>> qryId = l => l.EntityStatus == EStatus.PUBLISHED.GetDisplayName()
                && l.Id == new Guid("000ebc80-d782-4dee-8606-1199d9074039");

            //Act
            var result = await _fixture.Launch.GetSelected(
                filter: qryId,
                selectColumns: l => new { l.Id, l.WindowStart },
                buildObject: l => new TestObjectsDTO.OnlyIdAndDateDTO(l.Id, (DateTime)l.WindowStart)
            );

            //Assert
            Assert.NotNull(result);
            Assert.IsType<TestObjectsDTO.OnlyIdAndDateDTO>(result);
            Assert.Equal(TestLaunchInMemoryObjects.Test1().Id, result.Id);
            Assert.Equal(TestLaunchInMemoryObjects.Test1().WindowStart, result.WindowStart);
        }

        [Fact]
        public async Task GenericRepository_GetSelected_GetOneLaunchWithIncludeAndBuildDTO()
        {
            //Arrange
            Expression<Func<Launch, bool>> qryId = l => l.EntityStatus == EStatus.PUBLISHED.GetDisplayName()
                && l.Id == new Guid("000ebc80-d782-4dee-8606-1199d9074039");
            
            //Act
            var result = await _fixture.Launch.GetSelected(
                filter: qryId,
                selectColumns: l => new { l.IdPad, l.Pad.Name },
                buildObject: l => new TestObjectsDTO.OnlyIdAndPadNameDTO(){ IdPad = (Guid)l.IdPad, PadName = l.Name },
                includedProperties: "Pad"
            );

            //Assert
            Assert.NotNull(result);
            Assert.IsType<TestObjectsDTO.OnlyIdAndPadNameDTO>(result);
            Assert.Equal(TestLaunchInMemoryObjects.Test1().IdPad, result.IdPad);
            Assert.Equal(TestLaunchInMemoryObjects.Test1().Pad.Name, result.PadName);
        }

        [Fact]
        public async Task GenericRepository_UpdateOnQuery_UpdateDatabaseObjectsWithoutBringToMemory()
        {
            //Arrange
            Expression<Func<Launch, bool>> filter = l => l.Id == new Guid("000ebc80-d782-4dee-8606-1199d9074039");
            Expression<Func<Launch, Launch>> updateStatusColumn = l => new Launch()
            { EntityStatus = EStatus.TRASH.GetDisplayName() };

            //Act
            await _fixture.Launch.UpdateOnQuery(filter, updateStatusColumn);

            //Assert
            var getAssertInMemoryObject = await _fixture.Launch.GetSelected(
                filter: l => l.Id == new Guid("000ebc80-d782-4dee-8606-1199d9074039"),
                selectColumns: l => l.EntityStatus,
                buildObject: l => l);

            Assert.NotNull(getAssertInMemoryObject);
            Assert.Equal(getAssertInMemoryObject, EStatus.TRASH.GetDisplayName());

            //Rolling back the change for the other tests...
            Expression<Func<Launch, Launch>> rollbackUpdate = l => new Launch()
            { EntityStatus = EStatus.PUBLISHED.GetDisplayName() };
            await _fixture.Launch.UpdateOnQuery(filter, rollbackUpdate);
        }

        [Fact]
        public async Task GenericRepository_Save_SaveANewObjectInMemory()
        {
            //Arrange && Act
            await _fixture.Launch.Save(_fixture.NewObjectForSaveTests());

            //Assert
            Assert.Equal(4, await _fixture.Launch.EntityCount());
            Assert.Equal(new Guid("f81f0bd0-d730-46a4-aa00-44ef7441fd92"),
                await _fixture.Launch.GetSelected(l => l.Id == new Guid("f81f0bd0-d730-46a4-aa00-44ef7441fd92"),
                    selectColumns: l => l.Id,
                    buildObject: l => l)
            );

            //Rolling back the change for the other tests...
            await _fixture.Launch.Delete(_fixture.NewObjectForSaveTests());
        }
    }
}