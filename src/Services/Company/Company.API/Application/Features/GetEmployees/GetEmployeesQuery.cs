using Awc.Services.Company.API.Application.Abstractions.Messaging;
using Awc.Services.Company.API.ViewModels;

namespace Awc.Services.Company.API.Application.Features.GetEmployees
{
    public sealed record GetEmployeesQuery(StringSearchCriteria SearchCriteria) : IQuery<PagedList<EmployeeListItemViewModel>>;
}