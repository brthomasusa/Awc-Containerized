using WebUI.Models;
using WebUI.Models.ProductApi;

namespace WebUI.Services.Repositories.Product
{
    public interface IProductService
    {
        Task<DocumentPage<ProductListItemViewModel>> GetProductsFilteredByNameAsync
        (
            string searchField,
            string searchCriteria,
            string orderBy,
            int skip,
            int take
        );

        Task<ProductDetailViewModel> GetProductByIdAync(int productId);
    }
}