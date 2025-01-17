using Awc.Services.Company.API.Application.Features.GetDepartmentMembers;
using Awc.Services.Company.API.Extentions;

namespace Awc.Services.Company.API.Endpoints.Company
{
    public sealed class DepartmentMembers : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("companies/departmentmembers", GetDepartmentMembers);
        }

        public static async Task<IResult> GetDepartmentMembers
        (
            [FromQuery] int DepartmentId,
            [FromQuery] int Skip,
            [FromQuery] int Take,
            ISender sender,
            ILogger<DepartmentMembers> logger
        )
        {
            Result<PagedList<DepartmentMemberViewModel>>? result = null;

            try
            {
                GetDepartmentMembersQuery request = new(DepartmentId, Skip, Take);

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