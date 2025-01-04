using Awc.Services.Company.API.ViewModels;

namespace Awc.Services.Company.API.Services
{
    public interface IEmployeeService
    {
        Task<Result<EmployeeViewModel>> GetEmployeeViewModelWithChildren(int employeeIdid);   
    }
}