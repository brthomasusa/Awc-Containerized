using Awc.Services.Company.API.Application.Features.GetCompanyById;
using Awc.Services.Company.API.Extentions;
using Awc.Services.Company.API.ViewModels;

namespace Awc.Services.Company.API.Endpoints.Company
{
    public class CompanyById : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("companies/{companyId:int}", GetCompanyById); 
        }

        public static async Task<IResult> GetCompanyById(int companyId, ISender sender, ILogger<CompanyById> logger)
        {  
            Result<CompanyViewModel>? result = null;

            try
            {
                result = await sender.Send(new GetCompanyByIdQuery(CompanyId: companyId));
                        
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