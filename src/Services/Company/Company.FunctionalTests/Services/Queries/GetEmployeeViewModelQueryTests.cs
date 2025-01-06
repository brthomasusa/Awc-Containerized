using Dapper;
using Awc.Services.Company.API.ViewModels;
using Awc.Services.Company.API.Services.Queries;

namespace Company.FunctionalTests.Services.Queries
{
    public class GetEmployeeViewModelQueryTests : TestBase
    {
        [Fact]
        public async Task DoQuery_GetEmployeeViewModelQuery_ValidId_ShouldSucceed()
        {
            // Arrange
            int employeeId = 1;

            // Act
            Result<EmployeeDetailViewModel> result = await GetEmployeeViewModelQuery.DoQuery(_dapperCtx, employeeId);

            // Assert            
            Assert.True(result.IsSuccess);
        } 

        [Fact]
        public async Task DoQuery_GetEmployeeViewModelQuery_InvalidId_ShouldFail()
        {
            // Arrange
            int employeeId = -1;

            // Act
            Result<EmployeeDetailViewModel> result = await GetEmployeeViewModelQuery.DoQuery(_dapperCtx, employeeId);

            // Assert            
            Assert.True(result.IsFailure);
        } 

        [Fact]
        public async Task DoQuery_GetDepartmentHistoryViewModelQuery_ValidId_ShouldSucceed()
        {
            // Arrange
            int employeeId = 16;

            // Act
            Result<List<DepartmentHistoryViewModel>> result = await GetDepartmentHistoryViewModelQuery.DoQuery(_dapperCtx, employeeId);

            // Assert            
            Assert.True(result.IsSuccess);
            int departments = result.Value.Count;
            Assert.Equal(2, departments);
        }

        [Fact]
        public async Task DoQuery_GetDepartmentHistoryViewModelQuery_InvalidId_ShouldReturnZeroResults()
        {
            // Arrange
            int employeeId = -16;

            // Act
            Result<List<DepartmentHistoryViewModel>>  result = await GetDepartmentHistoryViewModelQuery.DoQuery(_dapperCtx, employeeId);

            // Assert            
            Assert.True(result.IsSuccess);
            int departments = result.Value.Count;
            Assert.Equal(0, departments);
        }  

        [Fact]
        public async Task DoQuery_GetPayHistoryViewModelQuery_ValidId_ShouldSucceed()
        {
            // Arrange
            int employeeId = 16;

            // Act
            Result<List<PayHistoryViewModel>> result = await GetPayHistoryViewModelQuery.DoQuery(_dapperCtx, employeeId);

            // Assert            
            Assert.True(result.IsSuccess);
            int histories = result.Value.Count;
            Assert.Equal(3, histories);
        }  

        [Fact]
        public async Task DoQuery_GetPayHistoryViewModelQuery_InvalidId_ShouldReturnZeroResults()
        {
            // Arrange
            int employeeId = -16;

            // Act
            Result<List<PayHistoryViewModel>> result = await GetPayHistoryViewModelQuery.DoQuery(_dapperCtx, employeeId);

            // Assert            
            Assert.True(result.IsSuccess);
            int histories = result.Value.Count;
            Assert.Equal(0, histories);
        }                                   
    }
}