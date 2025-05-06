using Awc.Services.Product.Product.API.Application.Features.Products.GetProductListItemsByName;
using Awc.Services.Product.Product.API.Application.Abstractions;
using Awc.Services.Product.Product.API.Extensions;
using Serilog.Context;

namespace Awc.Services.Product.Product.API.Endpoints
{
    public class GetProductListItems : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("products", GetProductListItemsByName);
        }

        public static async Task<IResult> GetProductListItemsByName
        (
            [FromQuery] string SearchField,
            [FromQuery] string SearchCriteria,
            [FromQuery] string OrderBy,
            [FromQuery] int Skip,
            [FromQuery] int Take,
            ISender sender,
            ILogger<GetProductListItems> logger
        )
        {
            using (LogContext.PushProperty("EndpointName", nameof(GetProductListItemsByName)))
            {
                Result<PagedList<ProductListItemViewModel>>? result = null;

                try
                {
                    StringSearchCriteria criteria = new
                    (
                        SearchField,
                        SearchCriteria,
                        string.IsNullOrEmpty(OrderBy) ? "[Name]" : OrderBy,
                        Skip,
                        Take
                    );

                    result = await sender.Send(new GetProductListItemsByNameQuery(SearchCriteria: criteria));

                    if (result.IsSuccess)
                    {
                        if (result.Value.Data.Count > 0)
                        {
                            logger.LogInformation("Returning {ProductCount} of {TotalRecords} products found  where search field '{SearchField}' was equal to '{SearchCriteria}'.",
                                result.Value.Data.Count, result.Value.MetaData!.TotalRecords, criteria.SearchField, criteria.SearchCriteria);
                        }
                        else
                        {
                            logger.LogWarning("No products found where search field '{SearchField}' was equal to '{SearchCriteria}'.", criteria.SearchField, criteria.SearchCriteria);
                        }

                        return Results.Ok(result.Value);
                    }
                    else
                    {
                        logger.LogError("An error occurred: {Message}", result.Error.Message);
                        return result.ToInternalServerErrorProblemDetails(result.Error.Message);
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