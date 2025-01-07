using WebUI.Models;
using WebUI.Models.CompanyApi;

namespace WebUI.Services.Repositories.Company
{
    public interface ICompanyService
    {
        Task<EmployeeDetailViewModel> GetEmployeeByIdAsync(int employeeId);
        Task<DocumentPage<EmployeeListItemViewModel>> GetEmployeesFilteredByNameAsync(string lastName, int pageNumber, int pageSize);
        Task<CompanyViewModel> GetCompanyByIdAsync(int companyId);
    }
}