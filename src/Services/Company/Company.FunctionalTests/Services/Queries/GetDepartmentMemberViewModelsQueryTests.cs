namespace Company.FunctionalTests.Services.Queries
{
    public class GetDepartmentMemberViewModelsQueryTests : TestBase
    {
        [Fact]
        public async Task DoQuery_GetDepartmentMemberViewModelsQuery_ValidDeptId_and_LastName_ShouldReturn2()
        {
            // Arrange
            int departmentId = 7;
            string lastName = "Du";
            int skip = 0;
            int take = 10;

            // Act
            Result<PagedList<DepartmentMemberViewModel>> result =
                await GetDepartmentMemberViewModelsQuery.DoQuery(_dapperCtx, departmentId, lastName, skip, take);

            // Assert
            Assert.True(result.IsSuccess);
            int members = result.Value.Data.Count;
            Assert.Equal(2, members);
        }
 
         [Fact]
        public async Task DoQuery_GetDepartmentMemberViewModelsQuery_ValidDeptId_and_EmptyLastName_ShouldReturn179()
        {
            // Arrange
            int departmentId = 7;
            string lastName = string.Empty;
            int skip = 0;
            int take = 180;

            // Act
            Result<PagedList<DepartmentMemberViewModel>> result =
                await GetDepartmentMemberViewModelsQuery.DoQuery(_dapperCtx, departmentId, lastName, skip, take);

            // Assert
            Assert.True(result.IsSuccess);
            int members = result.Value.Data.Count;
            Assert.Equal(179, members);
        }

         [Fact]
        public async Task DoQuery_GetDepartmentMemberViewModelsQuery_InvalidDeptId_ShouldReturn0()
        {
            // Arrange
            int departmentId = 0;
            string lastName = "Du";
            int skip = 0;
            int take = 10;

            // Act
            Result<PagedList<DepartmentMemberViewModel>> result =
                await GetDepartmentMemberViewModelsQuery.DoQuery(_dapperCtx, departmentId, lastName, skip, take);

            // Assert
            Assert.True(result.IsSuccess);
            int members = result.Value.Data.Count;
            Assert.Equal(0, members);
        }
    }
}