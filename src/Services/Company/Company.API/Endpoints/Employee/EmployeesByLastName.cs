using Awc.Services.Company.API.Application.Features.GetEmployees;
using Awc.Services.Company.API.Extentions;

namespace Awc.Services.Company.API.Endpoints.Employee
{
    public class EmployeesByLastName : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("employees", GetEmployees); 
        }

        public static async Task<IResult> GetEmployees
        (
            QueryParameters.FilterByFieldNameParameters parameters, 
            ISender sender, 
            ILogger<EmployeesByLastName> logger
        )
        {  
            Result<PagedList<EmployeeListItemViewModel>>? result = null;

            try
            {
                StringSearchCriteria criteria = new
                (
                    parameters.SearchField,
                    parameters.SearchCriteria,
                    parameters.OrderBy,
                    parameters.PageNumber,
                    parameters.PageSize,
                    parameters.Skip,
                    parameters.Take
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