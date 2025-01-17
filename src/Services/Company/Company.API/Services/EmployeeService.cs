using Awc.Services.Company.API.Application.Features.GetEmployees;

namespace Awc.Services.Company.API.Services
{
    public sealed class EmployeeService(DapperContext dapperContext, ILogger<EmployeeService> logger) : IEmployeeService
    {
        private readonly DapperContext _dapperContext = dapperContext;
        private readonly ILogger<EmployeeService> _logger = logger;

        public async Task<Result<EmployeeDetailViewModel>> GetEmployeeViewModelWithChildren(int employeeId)
        {
            Result<List<DepartmentHistoryViewModel>> getDepartments = await GetDepartmentHistoryViewModelQuery.DoQuery(_dapperContext, employeeId);

            if (getDepartments.IsFailure)
            {
                return Result<EmployeeDetailViewModel>.Failure<EmployeeDetailViewModel>(
                    new Error("EmployeeService.GetEmployeeViewModelWithChildren", getDepartments.Error.Message)
                );
            }

            Result<List<PayHistoryViewModel>> getPayHistories = await GetPayHistoryViewModelQuery.DoQuery(_dapperContext, employeeId);

            if (getPayHistories.IsFailure)
            {
                return Result<EmployeeDetailViewModel>.Failure<EmployeeDetailViewModel>(
                    new Error("EmployeeService.GetEmployeeViewModelWithChildren", getPayHistories.Error.Message)

                );
            }

            Result<EmployeeDetailViewModel> getEmployee = await GetEmployeeViewModelQuery.DoQuery(_dapperContext, employeeId);

            if (getEmployee.IsFailure)
            {
                return Result<EmployeeDetailViewModel>.Failure<EmployeeDetailViewModel>(
                    new Error("EmployeeService.GetEmployeeViewModelWithChildren", getEmployee.Error.Message)
                );
            }

            EmployeeDetailViewModel employee = getEmployee.Value;
            employee.DepartmentHistories = getDepartments.Value;
            employee.PayHistories = getPayHistories.Value;

            return employee;
        }

        public async Task<Result<PagedList<EmployeeListItemViewModel>>> GetEmployeeListItems(StringSearchCriteria criteria)
        {
            Result<PagedList<EmployeeListItemViewModel>> result =
                await GetEmployeeListItemsQuery.DoQuery(_dapperContext, criteria);

            if (result.IsFailure)
            {
                return Result<PagedList<EmployeeListItemViewModel>>.Failure<PagedList<EmployeeListItemViewModel>>(
                    new Error("EmployeeService.GetEmployeeListItems", result.Error.Message)
                );
            }

            return result;
        }
    }
}