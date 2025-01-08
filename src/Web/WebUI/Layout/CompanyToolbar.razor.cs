using Microsoft.AspNetCore.Components;

namespace WebUI.Layout
{
    public partial class CompanyToolbar
    {
        [Inject] private NavigationManager? Navigation { get; set; }

        private void NavigateToViewCompanyDetails()
            => Navigation?.NavigateTo("/company/company");

        private void NavigateToViewEmployeeList()
            => Navigation?.NavigateTo("/company/employeelist");
    }
}