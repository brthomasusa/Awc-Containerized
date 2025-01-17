namespace Awc.Services.Company.API.Application.Features.GetDepartmentMembers
{
    public sealed record GetDepartmentMembersQuery(int DepartmentId, int Skip, int Take) : IQuery<PagedList<DepartmentMemberViewModel>>;
}