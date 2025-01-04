using Awc.Services.Company.API.ViewModels;
using Awc.Services.Company.API.Services.Queries;

namespace Awc.Services.Company.API.Services
{
    public sealed class EmployeeService(DapperContext dapperContext, ILogger<EmployeeService> logger) : IEmployeeService
    {
        private readonly DapperContext _dapperContext = dapperContext;
        private readonly ILogger<EmployeeService> _logger = logger;

        public async Task<Result<EmployeeViewModel>> GetEmployeeViewModelWithChildren(int employeeId)
        {
            Result<List<DepartmentHistoryViewModel>> getDepartments = await GetDepartmentHistoryViewModelQuery.DoQuery(_dapperContext, employeeId);
            if (getDepartments.IsFailure)
            {
                return Result<EmployeeViewModel>.Failure<EmployeeViewModel>(
                    new Error("EmployeeService.GetEmployeeViewModelWithChildren", getDepartments.Error.Message)
                );
            }

            Result<List<PayHistoryViewModel>> getPayHistories = await GetPayHistoryViewModelQuery.DoQuery(_dapperContext, employeeId);
            if (getPayHistories.IsFailure)
            {
                return Result<EmployeeViewModel>.Failure<EmployeeViewModel>(
                    new Error("EmployeeService.GetEmployeeViewModelWithChildren", getPayHistories.Error.Message)

                );                
            }

            Result<EmployeeViewModel> getEmployee = await GetEmployeeViewModelQuery.DoQuery(_dapperContext, employeeId);
            if (getEmployee.IsFailure)
            {
                return Result<EmployeeViewModel>.Failure<EmployeeViewModel>(
                    new Error("EmployeeService.GetEmployeeViewModelWithChildren", getEmployee.Error.Message)
                );
            }

            EmployeeViewModel employee = getEmployee.Value;
            employee.DepartmentHistories = getDepartments.Value;
            employee.PayHistories = getPayHistories.Value;

            return employee;
        }                
    }
}