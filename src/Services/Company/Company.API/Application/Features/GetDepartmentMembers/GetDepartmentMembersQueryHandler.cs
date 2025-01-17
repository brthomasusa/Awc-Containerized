namespace Awc.Services.Company.API.Application.Features.GetDepartmentMembers
{
    public sealed class GetDepartmentMembersQueryHandler(ICompanyService service) : IQueryHandler<GetDepartmentMembersQuery, PagedList<DepartmentMemberViewModel>>
    {
        private readonly ICompanyService _service = service;

        public async Task<Result<PagedList<DepartmentMemberViewModel>>> Handle
        (
            GetDepartmentMembersQuery request,
            CancellationToken cancellationToken
        )
        {
            Result<PagedList<DepartmentMemberViewModel>> result = await _service.GetDepartmentMemberViewModels(request.DepartmentId, request.Skip, request.Take);

            if (result.IsFailure)
            {
                return Result<PagedList<DepartmentMemberViewModel>>.Failure<PagedList<DepartmentMemberViewModel>>(
                    new Error("GetDepartmentMembersQueryHandler.Handle", result.Error.Message)
                );
            }

            return result.Value;            
        }          
    }
}