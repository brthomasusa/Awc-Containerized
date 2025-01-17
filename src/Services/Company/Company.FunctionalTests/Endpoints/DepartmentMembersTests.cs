using System.Net.Http.Json;
using Microsoft.AspNetCore.WebUtilities;

namespace Company.FunctionalTests.Endpoints
{
    public class DepartmentMembersTests(ApiWebApplicationFactory fixture) : IntegrationTestBase(fixture)
    {
        [Fact]
        public async Task GetDepartmentMembers_DepartmentMembers_ValidDepartmentID_ShouldReturn10Items()
        {
            var queryParams = new Dictionary<string, string?>
            {
                ["DepartmentID"] = "7",
                ["Skip"] = "0",
                ["Take"] = "10"
            };

            PagedList<DepartmentMemberViewModel>? response = await _client
                .GetFromJsonAsync<PagedList<DepartmentMemberViewModel>>(QueryHelpers.AddQueryString($"{_urlRoot}companies/departmentmembers", queryParams));

            int count = response!.Data.Count;
            Assert.Equal(10, count);
        }        
    }
}