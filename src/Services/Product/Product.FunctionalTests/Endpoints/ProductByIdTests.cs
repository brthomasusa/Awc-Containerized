using System.Net.Http.Json;
using System.Text.Json;
using Awc.Services.Product.Product.API.Application.Abstractions;

namespace Product.FunctionalTests.Endpoints
{
    public class ProductByIdTests(ApiWebApplicationFactory fixture) : IntegrationTestBase(fixture)
    {
        [Fact]
        public async Task ProductById_GetProductById_ShouldSucceed()
        {
            const int productId = 399;
            using var response = await _client.GetAsync($"{_urlRoot}products/{productId}",
                                                        HttpCompletionOption.ResponseHeadersRead);

            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStreamAsync();
            var product = await JsonSerializer.DeserializeAsync<ProductDetailViewModel>(jsonResponse, _options);

            Assert.Equal("Head Tube", product!.Name);
        }
    }
}