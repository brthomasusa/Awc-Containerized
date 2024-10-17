using Awc.Dapr.Services.Company.API.Application.Abstractions.Messaging;
using Awc.Dapr.Services.Company.API.ViewModels;
using Awc.Dapr.Services.Company.API.Services;

namespace Awc.Dapr.Services.Company.API.Application.Features.GetCompanyById
{
    public sealed class GetCompanyByIdQueryHandler
    (
        ICompanyService companyService,
        ILogger<GetCompanyByIdQueryHandler> logger   
    ): IQueryHandler<GetCompanyByIdQuery, CompanyViewModel>
    {
        private readonly ICompanyService _companyService = companyService;
        private readonly ILogger<GetCompanyByIdQueryHandler> _logger = logger;

        public async Task<Result<CompanyViewModel>> Handle
        (
            GetCompanyByIdQuery query, 
            CancellationToken cancellationToken
        )
        {
            try
            {
                Result<CompanyViewModel> getCompany = await _companyService.GetCompanyViewModel(query.CompanyId);

                if (getCompany.IsFailure)
                {
                    return Result<CompanyViewModel>.Failure<CompanyViewModel>(
                        new Error("CompanyService.GetCompanyById", getCompany.Error.Message)
                    );                
                }

                return getCompany.Value; 
            }
            catch (Exception ex)
            {
                string errMsg = Helpers.GetExceptionMessage(ex);
                _logger.LogError(ex, "{Message}", errMsg);

                return Result<CompanyViewModel>.Failure<CompanyViewModel>(
                    new Error("GetCompanyByIdQueryHandler.Handle", Helpers.GetExceptionMessage(ex))
                );
            }                          
        }


    }
}