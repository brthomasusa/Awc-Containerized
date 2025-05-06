using Awc.Services.Product.Product.API.Application.Features.Products.GetProductViewModelById;
using Awc.Services.Product.Product.API.Extensions;
using Serilog.Context;

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
            using (LogContext.PushProperty("EndpointName", nameof(GetProductById)))
            {
                Result<ProductDetailViewModel>? result = null;

                try
                {
                    result = await sender.Send(new GetProductViewModelByIdQuery(ProductId: productId));

                    if (result.IsSuccess)
                    {
                        logger.LogInformation("Returning product with ID: {ProductId} and name: {ProductName}.", result.Value.ProductID, result.Value.Name);
                        return Results.Ok(result.Value);
                    }
                    else
                    {
                        // logger.LogWarning("Product with ID: {ProductId} could not be found.", result.Value.ProductID);
                        return result.ToNotFoundProblemDetails();
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "The following error occurred: {Message}", Helpers.GetInnerExceptionMessage(ex));
                    return result!.ToInternalServerErrorProblemDetails(Helpers.GetInnerExceptionMessage(ex));
                }
            }
        }
    }
}