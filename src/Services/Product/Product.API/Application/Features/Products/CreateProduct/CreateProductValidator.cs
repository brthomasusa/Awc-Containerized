namespace Awc.Services.Product.Product.API.Application.Features.Products.CreateProduct
{
    public class CreateProductValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductValidator()
        {
            RuleFor(product => product.ProductID)
                                      .Equal(0)
                                      .WithMessage("ID for new product should be zero.");
        }
    }
}