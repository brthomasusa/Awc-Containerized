using Awc.Services.Product.Product.API.Application.Abstractions.Messaging;
using Awc.Services.Product.Product.API.ViewModels;

namespace Awc.Services.Product.Product.API.Application.Features.Products.GetProductViewModelById
{
    public sealed record GetProductViewModelByIdQuery(int ProductId) : IQuery<ProductDetailViewModel>;
}