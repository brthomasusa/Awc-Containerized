using Awc.Services.Company.API.ViewModels;

namespace Awc.Services.Company.API.Services
{
    public interface ICompanyService
    {
        Task<Result<CompanyViewModel>> GetCompanyViewModel(int id);    
    }
}