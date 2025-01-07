using Microsoft.AspNetCore.Components;

namespace WebUI.Layout
{
    public partial class CompanyToolbar
    {
        [Inject] private NavigationManager? Navigation { get; set; }

        private void NavigateToViewCompanyDetails()
            => Navigation?.NavigateTo("/companydata/company");

        private void NavigateToViewEmployeeList()
            => Navigation?.NavigateTo("/companydata/employeelist");
    }
}