namespace Awc.Services.Company.API.Application.Features.GetCompanyById
{
    public sealed class GetCompanyByIdQueryHandler
    (
        ICompanyService companyService,
        ILogger<GetCompanyByIdQueryHandler> logger,
        ICacheService cacheService,
        IDatabaseRetryService databaseRetryService
    ) : IQueryHandler<GetCompanyByIdQuery, CompanyViewModel>
    {
        private readonly ICompanyService _companyService = companyService;
        private readonly ILogger<GetCompanyByIdQueryHandler> _logger = logger;
        private readonly ICacheService _cacheService = cacheService;
        private readonly IDatabaseRetryService _databaseRetryService = databaseRetryService;
        private static readonly TimeSpan _cacheExpiration = TimeSpan.FromDays(1);

        public async Task<Result<CompanyViewModel>> Handle
        (
            GetCompanyByIdQuery query,
            CancellationToken cancellationToken
        )
        {
            try
            {
                var cacheKey = $"company:{query.CompanyId}";
                var cacheData = await _cacheService.GetCacheValueAsync<CompanyViewModel>(cacheKey);

                if (cacheData is not null)
                {
                    return cacheData;
                }

                Result<CompanyViewModel>? getCompany = null;

                await _databaseRetryService.ExecuteWithRetryAsync(async () =>
                {
                    getCompany = await _companyService.GetCompanyViewModel(query.CompanyId);
                });

                if (getCompany!.IsFailure)
                {
                    return Result<CompanyViewModel>.Failure<CompanyViewModel>(
                        new Error("CompanyService.GetCompanyById", getCompany.Error.Message)
                    );
                }

                await _cacheService.SetCacheValueAsync<CompanyViewModel>(cacheKey, getCompany.Value, _cacheExpiration);

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