using System.Net.Http.Json;
using Microsoft.AspNetCore.WebUtilities;

namespace Company.FunctionalTests.Endpoints
{
    public class GetEmployeesTest(ApiWebApplicationFactory fixture) : IntegrationTestBase(fixture)
    {
        [Fact]
        public async Task GetEmployees_EmployeesByLastName_ValidCriteria_ShouldSucceed()
        {
            var queryParams = new Dictionary<string, string?>
            {
                ["searchField"] = "LastName",
                ["searchCriteria"] = "Du",
                ["orderBy"] = "LastName",
                ["pageNumber"] = "0",
                ["pageSize"] = "0",
                ["skip"] = "0",
                ["take"] = "10"
            };

            List<EmployeeListItemViewModel>? response = await _client
                .GetFromJsonAsync<List<EmployeeListItemViewModel>>(QueryHelpers.AddQueryString($"{_urlRoot}employees", queryParams));

            int count = response!.Count;
            Assert.Equal(1, count);
        }

        [Fact]
        public async Task GetEmployeesByLastNameTest_EmployeesByLastName_ValidCriteria_ShouldSucceed()
        {
            var queryParams = new Dictionary<string, string?>
            {
                ["searchField"] = "LastName",
                ["searchCriteria"] = "Du",
                ["orderBy"] = "LastName",
                ["pageNumber"] = "0",
                ["pageSize"] = "0",
                ["skip"] = "0",
                ["take"] = "10"
            };

            List<EmployeeListItemViewModel>? response = await _client
                .GetFromJsonAsync<List<EmployeeListItemViewModel>>(QueryHelpers.AddQueryString($"{_urlRoot}employeestest", queryParams));

            int count = response!.Count;
            Assert.Equal(1, count);
        }        
    }
}