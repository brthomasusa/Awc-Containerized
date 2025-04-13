#pragma warning disable CS9124, CS8603

using System.Text.Json;
using Microsoft.AspNetCore.WebUtilities;
using WebUI.Exceptions;

namespace WebUI.Services.Repositories.Product
{
    public sealed class ProductService(HttpClient httpClient) : IProductService
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly JsonSerializerOptions _options = new() { PropertyNameCaseInsensitive = true };

        public async Task<DocumentPage<ProductListItemViewModel>> GetProductsFilteredByNameAsync
        (
            string searchField,
            string searchCriteria,
            string orderBy,
            int skip,
            int take
        )
        {
            var queryParams = new Dictionary<string, string?>
            {
                ["searchField"] = searchField,
                ["searchCriteria"] = searchCriteria,
                ["orderBy"] = orderBy,
                ["skip"] = skip.ToString(),
                ["take"] = take.ToString()
            };

            var response = await _httpClient.GetAsync(QueryHelpers.AddQueryString("products", queryParams));

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<DocumentPage<ProductListItemViewModel>>();
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

        public async Task<ProductDetailViewModel> GetProductByIdAync(int productId)
        {
            var response = await _httpClient.GetAsync($"products/{productId}");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<ProductDetailViewModel>();
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
    }
}