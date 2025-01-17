namespace Company.FunctionalTests.Services.Queries
{
    public class GetDepartmentMemberViewModelsQueryTests : TestBase
    {
        [Fact]
        public async Task DoQuery_GetDepartmentMemberViewModelsQuery_ValidDeptId_ShouldReturn10()
        {
            // Arrange
            int departmentId = 7;
            int skip = 0;
            int take = 10;

            // Act
            Result<PagedList<DepartmentMemberViewModel>> result =
                await GetDepartmentMemberViewModelsQuery.DoQuery(_dapperCtx, departmentId, skip, take);

            // Assert
            Assert.True(result.IsSuccess);
            int members = result.Value.Data.Count;
            Assert.Equal(10, members);
        }
 
         [Fact]
        public async Task DoQuery_GetDepartmentMemberViewModelsQuery_InvalidDeptId_ShouldReturn0()
        {
            // Arrange
            int departmentId = 0;
            int skip = 0;
            int take = 10;

            // Act
            Result<PagedList<DepartmentMemberViewModel>> result =
                await GetDepartmentMemberViewModelsQuery.DoQuery(_dapperCtx, departmentId, skip, take);

            // Assert
            Assert.True(result.IsSuccess);
            int members = result.Value.Data.Count;
            Assert.Equal(0, members);
        }
    }
}