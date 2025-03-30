using Awc.Services.Product.Product.API.Infrastructure;
using Awc.Services.Product.Product.API.Infrastructure.Queries;
using Awc.Services.Product.Product.API.ViewModels;

namespace Awc.Services.Product.Product.API.Services
{
    public sealed class ProductService(DapperContext dapperContext, ILogger<ProductService> logger) : IProductService
    {
        private readonly DapperContext _dapperContext = dapperContext;
        private readonly ILogger<ProductService> _logger = logger;

        public Task<Result<ProductDetailViewModel>> GetPrductDetailViewModelById(int productId)
        {
            throw new NotImplementedException();
        }
    }
}