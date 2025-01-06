using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.WebUtilities;

namespace Company.FunctionalTests.Endpoints
{
    public class EmployeeByIdTests(ApiWebApplicationFactory fixture) : IntegrationTestBase(fixture)
    {
        [Fact]
        public async Task Employee_GetEmployeeDetails_ShouldSucceed()
        {
            const int employeeId = 16;
            using var response = await _client.GetAsync($"{_urlRoot}employees/{employeeId}",
                                                        HttpCompletionOption.ResponseHeadersRead);

            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStreamAsync();
            var employee = await JsonSerializer.DeserializeAsync<EmployeeDetailViewModel>(jsonResponse, _options);

            Assert.Equal("David M Bradley", employee!.EmployeeName);
            Assert.NotEmpty(employee!.DepartmentHistories!);
            Assert.NotEmpty(employee!.PayHistories!);
        }        
    }
}