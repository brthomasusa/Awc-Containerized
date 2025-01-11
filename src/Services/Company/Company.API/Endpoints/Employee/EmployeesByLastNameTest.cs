using Awc.Services.Company.API.Application.Features.GetEmployees;
using Awc.Services.Company.API.Extentions;

namespace Awc.Services.Company.API.Endpoints.Employee
{
    public class EmployeesByLastNameTest : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("employeestest", GetEmployees);
        }

        public static async Task<IResult> GetEmployees
        (
            [FromQuery] string SearchField,
            [FromQuery] string SearchCriteria,
            [FromQuery] string OrderBy,
            [FromQuery] int PageNumber,
            [FromQuery] int PageSize,
            [FromQuery] int Skip,
            [FromQuery] int Take,
            ISender sender,
            ILogger<EmployeesByLastName> logger
        )
        {
            Result<PagedList<EmployeeListItemViewModel>>? result = null;

            try
            {
                StringSearchCriteria criteria = new
                (
                    SearchField,
                    SearchCriteria,
                    string.IsNullOrEmpty(OrderBy) ? "[LastName]" : OrderBy,
                    Skip,
                    Take
                );

                GetEmployeesQuery request = new(SearchCriteria: criteria);

                result = await sender.Send(request);

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