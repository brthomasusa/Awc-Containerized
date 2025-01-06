using Awc.Services.Company.API.Application.Features.GetEmployees;
using Awc.Services.Company.API.Services.Queries;

namespace Company.FunctionalTests.Services.Queries
{
    public class GetEmployeeListItemsQueryTests : TestBase
    {
        [Fact]
        public async Task DoQuery_GetEmployeeListItemsQuery_ShouldSucceed()
        {
            StringSearchCriteria criteria = new("[LastName]", "Du", "[LastName]", 1, 10, 0, 10);
            Result<PagedList<EmployeeListItemViewModel>> result =
                await GetEmployeeListItemsQuery.DoQuery(_dapperCtx, criteria);

            Assert.True(result.IsSuccess);
            int employees = result.Value.Count;
            Assert.Equal(1, employees);
        } 

        [Fact]
        public async Task DoQuery_GetEmployeeListItemsQuery_EmptySearchString_ShouldSucceed()
        {
            StringSearchCriteria criteria = new("[LastName]", string.Empty, "[LastName]", 1, 10, 0, 10);
            Result<PagedList<EmployeeListItemViewModel>> result =
                await GetEmployeeListItemsQuery.DoQuery(_dapperCtx, criteria);

            Assert.True(result.IsSuccess);
            int employees = result.Value.Count;
            Assert.Equal(7, employees);
        }                
    }
}