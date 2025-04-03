using Awc.Services.Product.Product.API.Application.Abstractions;

namespace Awc.Services.Product.Product.API.Application.Features.Products.GetProductListItemsByName
{
    public sealed record GetProductListItemsByNameQuery(StringSearchCriteria SearchCriteria) : IQuery<PagedList<ProductListItemViewModel>>;
}