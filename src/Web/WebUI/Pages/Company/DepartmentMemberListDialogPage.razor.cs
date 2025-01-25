using Microsoft.AspNetCore.Components;
using Radzen;
using WebUI.Exceptions;
using WebUI.Models;
using WebUI.Models.CompanyApi;
using WebUI.Services.Repositories.Company;
using WebUI.Utilities;

namespace WebUI.Pages.Company
{
    public partial class DepartmentMemberListDialogPage
    {
        [Inject] private ICompanyService? CompanyService { get; set; }
        [Inject] public DialogService? DialogService { get; set; }
        [Inject] private NotificationService? NotificationService { get; set; }
        [Inject] private NavigationManager? Navigation { get; set; }
        [Parameter] public int DepartmentID { get; set; }
        [Parameter] public bool ShowClose { get; set; } = true;
        private DocumentPage<DepartmentMemberViewModel>? _members;
        private string _lastNameFilter = string.Empty;
        private readonly IEnumerable<int> pageSizeOptions = new List<int>() { 5, 10, 15, 20 };
        private bool isLoading;

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();

            _members = await CompanyService!.GetDepartmentMembersAsync(DepartmentID, _lastNameFilter, 0, 20);
        }

        private async Task GetDepartmentMembers(LoadDataArgs args)
        {
            try
            {
                if (args.Filters is not null)
                {
                    List<FilterDescriptor> descriptors = args.Filters.ToList();
                    FilterDescriptor? filterDescriptor
                        = descriptors.Find(x => !string.IsNullOrEmpty(x.Property) && !string.IsNullOrEmpty(x.FilterValue.ToString()));

                    if (filterDescriptor is not null)
                    {
                        _lastNameFilter = filterDescriptor.FilterValue.ToString()!;
                    }
                    else
                    {
                        _lastNameFilter = string.Empty;
                    }
                }
                else
                {
                    _lastNameFilter = string.Empty;
                }

                isLoading = true;

                _members = await CompanyService!.GetDepartmentMembersAsync(DepartmentID, _lastNameFilter, args.Skip ?? default, args.Top ?? default);

                isLoading = false;

                await InvokeAsync(StateHasChanged);

            }
            catch (ApiResponseException ex)
            {
                ShowErrorNotification.ShowError(
                    NotificationService!,
                    ex.Message
                );

                Navigation?.NavigateTo("/Pages/Company/ViewCompanyDetailPage");
            }
            catch (Exception ex)
            {
                ShowErrorNotification.ShowError(
                    NotificationService!,
                    ex.Message
                );

                Navigation?.NavigateTo("/Pages/Company/ViewCompanyDetailPage");
            }
        }
    }
}