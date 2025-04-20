using System.Net.Http.Json;
using Microsoft.AspNetCore.WebUtilities;
using Awc.Services.Product.Product.API.Application.Abstractions;

namespace Product.FunctionalTests.Endpoints
{
    public class GetProductListItemViewModelsTest(ApiWebApplicationFactory fixture) : IntegrationTestBase(fixture)
    {
        [Fact]
        public async Task GetProductListItemsByName_GetProductListItemViewModelsByName_ValidCriteria_ShouldSucceed()
        {
            var queryParams = new Dictionary<string, string?>
            {
                ["searchField"] = "Name",
                ["searchCriteria"] = "tube",
                ["orderBy"] = "Name",
                ["skip"] = "0",
                ["take"] = "10"
            };


            PagedList<ProductListItemViewModel>? response = await _client
                .GetFromJsonAsync<PagedList<ProductListItemViewModel>>(QueryHelpers.AddQueryString($"{_urlRoot}products", queryParams));

            int count = response!.Data.Count;
            Assert.Equal(1, count);
        }
    }
}