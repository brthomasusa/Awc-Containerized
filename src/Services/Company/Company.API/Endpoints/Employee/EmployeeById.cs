using Awc.Services.Company.API.Application.Features.GetEmployeeById;
using Awc.Services.Company.API.Extentions;

namespace Awc.Services.Company.API.Endpoints.Employee
{
    public class EmployeeById : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("employees/{employeeId:int}", GetEmployeeId); 
        }

        public static async Task<IResult> GetEmployeeId(int employeeId, ISender sender, ILogger<EmployeeById> logger)
        {  
            Result<EmployeeDetailViewModel>? result = null;

            try
            {
                result = await sender.Send(new GetEmployeeByIdQuery(EmployeeId: employeeId));
                        
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