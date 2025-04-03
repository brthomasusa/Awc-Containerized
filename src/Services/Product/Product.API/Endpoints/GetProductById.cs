using Awc.Services.Product.Product.API.Application.Features.Products.GetProductViewModelById;
using Awc.Services.Product.Product.API.Extensions;

namespace Awc.Services.Product.Product.API.Endpoints
{
    public class ProductById : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("products/{productId:int}", GetProductById);
        }

        public static async Task<IResult> GetProductById(int productId, ISender sender, ILogger<ProductById> logger)
        {
            Result<ProductDetailViewModel>? result = null;

            try
            {
                result = await sender.Send(new GetProductViewModelByIdQuery(ProductId: productId));

                return result.IsSuccess ? Results.Ok(result.Value) : result.ToNotFoundProblemDetails();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "{Message}", Helpers.GetInnerExceptionMessage(ex));
                return result!.ToInternalServerErrorProblemDetails(ex.Message);
            }
        }
    }
}