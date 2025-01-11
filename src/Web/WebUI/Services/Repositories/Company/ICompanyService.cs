using WebUI.Models;
using WebUI.Models.CompanyApi;

namespace WebUI.Services.Repositories.Company
{
    public interface ICompanyService
    {
        Task<EmployeeDetailViewModel> GetEmployeeByIdAsync(int employeeId);
        Task<DocumentPage<EmployeeListItemViewModel>> GetEmployeesFilteredByNameAsync
        (
            string searchField,
            string searchCriteria,
            string orderBy,
            int skip,
            int take
        );
        Task<CompanyViewModel> GetCompanyByIdAsync(int companyId);
    }
}