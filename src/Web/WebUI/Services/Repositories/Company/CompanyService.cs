#pragma warning disable CS8603

using System.Net.Http.Json;
using Microsoft.AspNetCore.WebUtilities;
using WebUI.Exceptions;
using WebUI.Models;
using WebUI.Models.CompanyApi;

namespace WebUI.Services.Repositories.Company
{
    public sealed class CompanyService(HttpClient httpClient) : ICompanyService
    {
        private readonly HttpClient _httpClient = httpClient;

        public async Task<EmployeeDetailViewModel> GetEmployeeByIdAsync(int employeeId)
        {
            var response = await _httpClient.GetAsync($"api/employee/getbyid/{employeeId}");

            if (response.IsSuccessStatusCode) // if response status code is 2XX
            {
                return await response.Content.ReadFromJsonAsync<EmployeeDetailViewModel>();
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                var error = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
                throw new ApiResponseException(error!);
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

        public async Task<DocumentPage<EmployeeListItemViewModel>> GetEmployeesFilteredByNameAsync(string lastName, int pageNumber, int pageSize)
        {
            var queryParams = new Dictionary<string, string?>
            {
                ["lastName"] = lastName,
                ["pageNumber"] = pageNumber.ToString(),
                ["pageSize"] = pageSize.ToString()
            };

            var response = await _httpClient.GetAsync(QueryHelpers.AddQueryString("api/employee/getbyname", queryParams));

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
            var response = await _httpClient.GetAsync($"api/company/{companyId}");

            if (response.IsSuccessStatusCode) // if response status code is 2XX
            {
                return await response.Content.ReadFromJsonAsync<CompanyViewModel>();
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                var error = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
                throw new ApiResponseException(error!);
            }
            else
            {
                // Throw exception for other failure responses 
                throw new Exception("Opps! Something went wrong");
            }
        }
    }
}