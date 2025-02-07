namespace Awc.Services.Company.API.Application.Features.GetEmployees
{
    public sealed class GetEmployeesQueryHandler
    (
        IEmployeeService service,
        IDatabaseRetryService databaseRetryService
    ) : IQueryHandler<GetEmployeesQuery, PagedList<EmployeeListItemViewModel>>
    {
        private readonly IEmployeeService _service = service;
        private readonly IDatabaseRetryService _databaseRetryService = databaseRetryService;

        public async Task<Result<PagedList<EmployeeListItemViewModel>>> Handle
        (
            GetEmployeesQuery request,
            CancellationToken cancellationToken
        )
        {
            Result<PagedList<EmployeeListItemViewModel>>? result = null;

            await _databaseRetryService.ExecuteWithRetryAsync(async () =>
            {
                result = await _service.GetEmployeeListItems(request.SearchCriteria);
            });

            if (result!.IsFailure)
            {
                return Result<PagedList<EmployeeListItemViewModel>>.Failure<PagedList<EmployeeListItemViewModel>>(
                    new Error("GetEmployeesQueryHandler.Handle", result.Error.Message)
                );
            }

            return result.Value;
        }
    }
}