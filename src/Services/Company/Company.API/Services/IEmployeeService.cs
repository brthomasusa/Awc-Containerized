using Awc.Services.Company.API.Application.Features.GetEmployees;

namespace Awc.Services.Company.API.Services
{
    public interface IEmployeeService
    {
        Task<Result<EmployeeDetailViewModel>> GetEmployeeViewModelWithChildren(int employeeIdid);
        Task<Result<PagedList<EmployeeListItemViewModel>>> GetEmployeeListItems(StringSearchCriteria criteria);
    }
}