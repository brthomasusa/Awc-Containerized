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
    public partial class ViewEmployeeListItemsPage
    {
        [Inject] private ICompanyService? CompanyService { get; set; }
        [Inject] public DialogService? DialogService { get; set; }
        [Inject] private NotificationService? NotificationService { get; set; }
        [Inject] private NavigationManager? Navigation { get; set; }
        [Inject] private IJSRuntime? JSRuntime { get; set; }

        private DocumentPage<EmployeeListItemViewModel>? _employees;
        private string _lastNameFilter = string.Empty;
        private readonly IEnumerable<int> pageSizeOptions = [5, 10, 15, 20];
        private bool isLoading;
        private EmployeeDialogSettings? _settings;

        protected async override Task OnInitializedAsync()
        {
            try
            {
                _employees ??= await CompanyService!.GetEmployeesFilteredByNameAsync("LastName", _lastNameFilter, string.Empty, 0, 20);
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

        private async Task GetEmployeeListItems(LoadDataArgs args)
        {
            try
            {
                Console.WriteLine($"ViewEmployeeListItemsPag.GetEmployeeListItems: args: {args.ToJson()}");
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

                _employees = await CompanyService!.GetEmployeesFilteredByNameAsync("LastName", _lastNameFilter, string.Empty, args.Skip ?? default, args.Top ?? default);

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

        private async void ViewEmployeeDetails(EmployeeListItemViewModel model)
        {
            await LoadStateAsync();

            await DialogService!.OpenAsync<ViewEmployeeDetailDialogPage>($"{model.FirstName} {model.MiddleName} {model.LastName}, {model.JobTitle}",
                new Dictionary<string, object>() { { "EmployeeID", (int)model.BusinessEntityID } },
                new DialogOptions()
                {
                    Resizable = true,
                    Draggable = true,
                    Resize = OnResize,
                    Drag = OnDrag,
                    Width = Settings != null ? Settings.Width : "700px",
                    Height = Settings != null ? Settings.Height : "512px",
                    Left = Settings?.Left,
                    Top = Settings?.Top
                });

            await SaveStateAsync();
        }

        private void OnDrag(System.Drawing.Point point)
        {
            _ = JSRuntime!.InvokeVoidAsync("eval", $"console.log('Dialog drag. Left:{point.X}, Top:{point.Y}')");

            Settings ??= new EmployeeDialogSettings();

            Settings.Left = $"{point.X}px";
            Settings.Top = $"{point.Y}px";

            InvokeAsync(SaveStateAsync);
        }

        private void OnResize(System.Drawing.Size size)
        {
            JSRuntime!.InvokeVoidAsync("eval", $"console.log('Dialog resize. Width:{size.Width}, Height:{size.Height}')");

            Settings ??= new EmployeeDialogSettings();

            Settings.Width = $"{size.Width}px";
            Settings.Height = $"{size.Height}px";

            InvokeAsync(SaveStateAsync);
        }

        private async Task SaveStateAsync()
        {
            await Task.CompletedTask;

            await JSRuntime!.InvokeVoidAsync("window.localStorage.setItem",
                                             "DeptMemberDialogSettings",
                                             JsonSerializer.Serialize<EmployeeDialogSettings>(Settings));
        }

        private async Task LoadStateAsync()
        {
            await Task.CompletedTask;

            var result = await JSRuntime!.InvokeAsync<string>("window.localStorage.getItem",
                                                              "DeptMemberDialogSettings");

            if (!string.IsNullOrEmpty(result))
            {
                _settings = JsonSerializer.Deserialize<EmployeeDialogSettings>(result);
            }
        }

        public EmployeeDialogSettings Settings
        {
            get { return _settings!; }
            set
            {
                if (_settings != value)
                {
                    _settings = value;
                    InvokeAsync(SaveStateAsync);
                }
            }
        }

        public sealed class EmployeeDialogSettings
        {
            public string? Left { get; set; }
            public string? Top { get; set; }
            public string? Width { get; set; }
            public string? Height { get; set; }
        }
    }
}