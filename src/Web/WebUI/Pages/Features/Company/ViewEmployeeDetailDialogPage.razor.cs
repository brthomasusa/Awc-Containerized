using System.Text.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;
using WebUI.Exceptions;
using WebUI.Models;
using WebUI.Models.CompanyApi;
using WebUI.Services.Repositories.Company;
using WebUI.Utilities;

namespace WebUI.Pages.Features.Company
{
    public partial class ViewEmployeeDetailDialogPage
    {
        [Parameter(CaptureUnmatchedValues = true)]
        public IReadOnlyDictionary<string, dynamic>? Attributes { get; set; }
        [Parameter] public dynamic? EmployeeID { get; set; }
        [Parameter] public bool ShowClose { get; set; } = true;
        [Inject] private ICompanyService? CompanyService { get; set; }
        [Inject] private NotificationService? NotificationService { get; set; }
        [Inject] private NavigationManager? Navigation { get; set; }
        [Inject] private DialogService? DialogService { get; set; }
        private EmployeeDetailViewModel? _employee;

        protected async override Task OnInitializedAsync()
        {
            try
            {
                _employee = await CompanyService!.GetEmployeeByIdAsync(EmployeeID);
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