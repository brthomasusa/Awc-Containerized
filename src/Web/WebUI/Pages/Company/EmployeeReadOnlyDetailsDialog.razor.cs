using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using WebUI.Models.CompanyApi;

namespace WebUI.Pages.Company
{
    public partial class EmployeeReadOnlyDetailsDialog : IDialogContentComponent<EmployeeDetailViewModel>
    {
        [Parameter] public EmployeeDetailViewModel Content { get; set; } = default!;

        [CascadingParameter] public FluentDialog? Dialog { get; set; }
    }
}