#pragma warning disable CS8603

using System.Text.Json;
using System.Net.Http.Json;
using Microsoft.AspNetCore.WebUtilities;
using WebUI.Exceptions;
using WebUI.Models;
using WebUI.Models.CompanyApi;
using WebUI.Utilities;

namespace WebUI.Services.Repositories.Company
{
    public sealed class CompanyService(HttpClient httpClient) : ICompanyService
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly JsonSerializerOptions _options = new() { PropertyNameCaseInsensitive = true };

        public async Task<EmployeeDetailViewModel> GetEmployeeByIdAsync(int employeeId)
        {
            var response = await _httpClient.GetAsync($"employees/{employeeId}");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<EmployeeDetailViewModel>();
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                var error = await response.Content.ReadFromJsonAsync<ProblemDetailResponse>();
                throw new ApiResponseException(new ApiErrorResponse(error!.Detail!));
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                var error = await response.Content.ReadFromJsonAsync<ProblemDetailResponse>();
                throw new Exception(error!.Detail);
            }
            else
            {
                throw new Exception("Opps! Something went wrong");
            }
        }

        public async Task<DocumentPage<EmployeeListItemViewModel>> GetEmployeesFilteredByNameAsync
        (
            string searchField,
            string searchCriteria,
            string orderBy,
            int pageNumber,
            int pageSize,
            int skip,
            int take
        )
        {
            var queryParams = new Dictionary<string, string?>
            {
                ["searchField"] = searchField,
                ["searchCriteria"] = searchCriteria,
                ["orderBy"] = orderBy,
                ["pageNumber"] = pageNumber.ToString(),
                ["pageSize"] = pageSize.ToString(),
                ["skip"] = skip.ToString(),
                ["take"] = take.ToString()
            };

            var response = await _httpClient.GetAsync(QueryHelpers.AddQueryString("employees", queryParams));

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<DocumentPage<EmployeeListItemViewModel>>();
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                var error = await response.Content.ReadFromJsonAsync<ProblemDetailResponse>();
                throw new Exception(error!.Detail);
            }
            else
            {
                // Throw exception for other failure responses 
                throw new Exception("Opps! Something went wrong");
            }
        }

        public async Task<CompanyViewModel> GetCompanyByIdAsync(int companyId)
        {
            var response = await _httpClient.GetAsync($"companies/{companyId}");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<CompanyViewModel>();
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                var errorResponse = await response.Content.ReadFromJsonAsync<ProblemDetailResponse>();
                throw new ApiResponseException(new ApiErrorResponse(errorResponse!.Detail!));
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                string errorMsg = $"We are sorry, the api server was unable to process this request due to an internal error.!";
                throw new Exception(errorMsg);
            }
            else
            {
                throw new Exception("Opps! Something went wrong");
            }
        }
    }
}