namespace Awc.Services.Company.API.Application.Features.GetDepartmentMembers
{
    public sealed class GetDepartmentMembersQueryHandler
    (
        ICompanyService service,
        IDatabaseRetryService databaseRetryService
    ) : IQueryHandler<GetDepartmentMembersQuery, PagedList<DepartmentMemberViewModel>>
    {
        private readonly ICompanyService _service = service;
        private readonly IDatabaseRetryService _databaseRetryService = databaseRetryService;

        public async Task<Result<PagedList<DepartmentMemberViewModel>>> Handle
        (
            GetDepartmentMembersQuery request,
            CancellationToken cancellationToken
        )
        {
            Result<PagedList<DepartmentMemberViewModel>>? result = null;

            await _databaseRetryService.ExecuteWithRetryAsync(async () =>
            {
                result = await _service.GetDepartmentMemberViewModels(request.DepartmentId,
                                                                      request.LastName,
                                                                      request.Skip,
                                                                      request.Take);
            });


            if (result!.IsFailure)
            {
                return Result<PagedList<DepartmentMemberViewModel>>.Failure<PagedList<DepartmentMemberViewModel>>(
                    new Error("GetDepartmentMembersQueryHandler.Handle", result.Error.Message)
                );
            }

            return result.Value;
        }
    }
}