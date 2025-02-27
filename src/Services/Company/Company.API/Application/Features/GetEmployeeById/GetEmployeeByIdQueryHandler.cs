namespace Awc.Services.Company.API.Application.Features.GetEmployeeById
{
    public sealed class GetEmployeeByIdQueryHandler
    (
        IEmployeeService service,
        IDatabaseRetryService databaseRetryService
    ) : IQueryHandler<GetEmployeeByIdQuery, EmployeeDetailViewModel>
    {
        private readonly IEmployeeService _service = service;
        private readonly IDatabaseRetryService _databaseRetryService = databaseRetryService;
        private static readonly TimeSpan _cacheExpiration = TimeSpan.FromDays(1);

        public async Task<Result<EmployeeDetailViewModel>> Handle
        (
            GetEmployeeByIdQuery request,
            CancellationToken cancellationToken
        )
        {
            // var cacheKey = $"employee:{request.EmployeeId}";
            // var cacheData = await _cacheService.GetCacheValueAsync<EmployeeDetailViewModel>(cacheKey);

            // if (cacheData is not null)
            // {
            //     return cacheData;
            // }

            Result<EmployeeDetailViewModel>? result = null;

            await _databaseRetryService.ExecuteWithRetryAsync(async () =>
            {
                result = await _service.GetEmployeeViewModelWithChildren(request.EmployeeId);
            });

            if (result!.IsFailure)
            {
                return Result<EmployeeDetailViewModel>.Failure<EmployeeDetailViewModel>(
                    new Error("GetEmployeeByIdQueryHandler.Handle", result.Error.Message)
                );
            }

            // var expireyTime = DateTimeOffset.Now.AddSeconds(60);
            // await _cacheService.SetCacheValueAsync<EmployeeDetailViewModel>(cacheKey, result.Value, _cacheExpiration);

            return result.Value;
        }
    }
}