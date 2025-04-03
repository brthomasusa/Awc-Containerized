using Awc.Services.Product.Product.API.Application.Features.Products.GetProductListItemsByName;
using Awc.Services.Product.Product.API.Application.Abstractions;
using Awc.Services.Product.Product.API.Extensions;

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

                return result.IsSuccess ? Results.Ok(result.Value) :
                                          result.ToInternalServerErrorProblemDetails(result.Error.Message);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "{Message}", Helpers.GetInnerExceptionMessage(ex));
                return result!.ToInternalServerErrorProblemDetails(ex.Message);
            }
        }
    }
}