using Awc.Services.Company.API.Application.Abstractions.Messaging;
using Awc.Services.Company.API.ViewModels;
using Awc.Services.Company.API.Services;

namespace Awc.Services.Company.API.Application.Features.GetCompanyById
{
    public sealed class GetCompanyByIdQueryHandler
    (
        ICompanyService companyService,
        ILogger<GetCompanyByIdQueryHandler> logger,
        ICacheService cacheService
    ) : IQueryHandler<GetCompanyByIdQuery, CompanyViewModel>
    {
        private readonly ICompanyService _companyService = companyService;
        private readonly ILogger<GetCompanyByIdQueryHandler> _logger = logger;
        private readonly ICacheService _cacheService = cacheService;

        public async Task<Result<CompanyViewModel>> Handle
        (
            GetCompanyByIdQuery query,
            CancellationToken cancellationToken
        )
        {
            try
            {
                var cacheData = _cacheService.GetData<CompanyViewModel>($"company{query.CompanyId}");

                if (cacheData is not null)
                {
                    return cacheData;
                }

                Result<CompanyViewModel> getCompany = await _companyService.GetCompanyViewModel(query.CompanyId);

                if (getCompany.IsFailure)
                {
                    return Result<CompanyViewModel>.Failure<CompanyViewModel>(
                        new Error("CompanyService.GetCompanyById", getCompany.Error.Message)
                    );
                }

                var expireyTime = DateTimeOffset.Now.AddSeconds(60);
                _cacheService.SetData<CompanyViewModel>($"colour{getCompany.Value.CompanyID}", getCompany.Value, expireyTime);

                return getCompany.Value;
            }
            catch (Exception ex)
            {
                string errMsg = Helpers.GetInnerExceptionMessage(ex);
                _logger.LogError(ex, "{Message}", errMsg);

                return Result<CompanyViewModel>.Failure<CompanyViewModel>(
                    new Error("GetCompanyByIdQueryHandler.Handle", Helpers.GetInnerExceptionMessage(ex))
                );
            }
        }


    }
}