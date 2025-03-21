#pragma warning disable CS9124

using System.Text.Json;
using System.Net.Http.Json;
using Microsoft.AspNetCore.WebUtilities;
using WebUI.Exceptions;
using WebUI.Models;
using WebUI.Models.ProductApi;
using WebUI.Utilities;
using System.Linq;

namespace WebUI.Services.Repositories.Product
{
    public sealed class ProductService(HttpClient httpClient) : IProductService
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly JsonSerializerOptions _options = new() { PropertyNameCaseInsensitive = true };

        public Task<DocumentPage<ProductListItemViewModel>> GetProductsFilteredByNameAsync
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

            throw new NotImplementedException();
        }

        public async Task<IQueryable<ProductListItemViewModel>> GetProductsListItemsAync()
        {
            var products = await _httpClient.GetFromJsonAsync<List<ProductListItemViewModel>>("sample-data/ProductListItemsByName.json");
            return products!.AsQueryable();
        }

        public async Task<ProductDetailViewModel> GetProductByIdAync(int productId)
        {
            List<ProductDetailViewModel>? products = await _httpClient.GetFromJsonAsync<List<ProductDetailViewModel>>("sample-data/ProductDetailViewModel.json");

            var product = products!.FirstOrDefault(p => p.ProductID == productId);

            return product!;
        }
    }
}