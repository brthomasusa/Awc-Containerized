namespace Awc.Services.Company.API.Application.Features.GetDepartmentMembers
{
    public sealed record GetDepartmentMembersQuery(int DepartmentId, string LastName, int Skip, int Take) 
        : IQuery<PagedList<DepartmentMemberViewModel>>;
}