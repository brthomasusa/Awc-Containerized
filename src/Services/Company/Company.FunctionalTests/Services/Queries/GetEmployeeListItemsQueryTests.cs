using Awc.Services.Company.API.Application.Features.GetEmployees;
using Awc.Services.Company.API.Services.Queries;

namespace Company.FunctionalTests.Services.Queries
{
    public class GetEmployeeListItemsQueryTests : TestBase
    {
        [Fact]
        public async Task DoQuery_GetEmployeeListItemsQuery_ShouldSucceed()
        {
            StringSearchCriteria criteria = new("[LastName]", "Du", "[LastName], [FirstName], [MiddleName]", 0, 10);
            Result<PagedList<EmployeeListItemViewModel>> result =
                await GetEmployeeListItemsQuery.DoQuery(_dapperCtx, criteria);

            Assert.True(result.IsSuccess);
            int employees = result.Value.Data.Count;
            Assert.Equal(3, employees);
        }

        [Fact]
        public async Task DoQuery_GetEmployeeListItemsQuery_EmptySearchString_ShouldReturn7Rows()
        {
            StringSearchCriteria criteria = new("[LastName]", string.Empty, "[LastName]", 0, 10);
            Result<PagedList<EmployeeListItemViewModel>> result =
                await GetEmployeeListItemsQuery.DoQuery(_dapperCtx, criteria);

            Assert.True(result.IsSuccess);
            int employees = result.Value.Data.Count;
            Assert.Equal(10, employees);
        }

        [Fact]
        public async Task DoQuery_GetEmployeeListItemsQuery_EmptySearchString_ShouldReturn5Rows()
        {
            StringSearchCriteria criteria = new("[LastName]", string.Empty, "[LastName]", 0, 5);
            Result<PagedList<EmployeeListItemViewModel>> result =
                await GetEmployeeListItemsQuery.DoQuery(_dapperCtx, criteria);

            Assert.True(result.IsSuccess);
            int employees = result.Value.Data.Count;
            Assert.Equal(5, employees);
        }

    }
}