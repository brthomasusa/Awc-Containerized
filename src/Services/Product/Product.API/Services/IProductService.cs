using Awc.Services.Product.Product.API.ViewModels;

namespace Awc.Services.Product.Product.API.Services
{
    public interface IProductService
    {
        Task<Result<ProductDetailViewModel>> GetPrductDetailViewModelById(int productId);
    }
}