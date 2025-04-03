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
                ["skip"] = "0",
                ["take"] = "10"
            };

            PagedList<EmployeeListItemViewModel>? response = await _client
                .GetFromJsonAsync<PagedList<EmployeeListItemViewModel>>(QueryHelpers.AddQueryString($"{_urlRoot}employees", queryParams));

            int count = response!.Data.Count;
            Assert.Equal(3, count);
        }


        [Fact]
        public async Task GetEmployeesByLastNameTest_EmployeesByLastName_EmptySearchCriteria_ShouldReturnSevenRecords()
        {
            var queryParams = new Dictionary<string, string?>
            {
                ["searchField"] = "LastName",
                ["searchCriteria"] = string.Empty,
                ["orderBy"] = "LastName",
                ["skip"] = "0",
                ["take"] = "10"
            };

            PagedList<EmployeeListItemViewModel>? response = await _client
                .GetFromJsonAsync<PagedList<EmployeeListItemViewModel>>(QueryHelpers.AddQueryString($"{_urlRoot}employees", queryParams));

            int count = response!.Data.Count;
            Assert.Equal(10, count);
        }

        [Fact]
        public async Task GetEmployeesByLastNameTest_EmployeesByLastName_EmptySearchCriteria_ShouldReturnFourRecords()
        {
            var queryParams = new Dictionary<string, string?>
            {
                ["searchField"] = "LastName",
                ["searchCriteria"] = string.Empty,
                ["orderBy"] = "LastName",
                ["skip"] = "0",
                ["take"] = "4"
            };

            PagedList<EmployeeListItemViewModel>? response = await _client
                .GetFromJsonAsync<PagedList<EmployeeListItemViewModel>>(QueryHelpers.AddQueryString($"{_urlRoot}employees", queryParams));

            int count = response!.Data.Count;
            Assert.Equal(4, count);
        }
    }
}