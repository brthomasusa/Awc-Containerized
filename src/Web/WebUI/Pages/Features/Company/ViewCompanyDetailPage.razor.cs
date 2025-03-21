using Microsoft.AspNetCore.Components;
using Radzen;
using WebUI.Components;
using WebUI.Exceptions;
using WebUI.Models.CompanyApi;
using WebUI.Services.Repositories.Company;
using System.Text.Json;
using Microsoft.JSInterop;

namespace WebUI.Pages.Features.Company
{
    public partial class ViewCompanyDetailPage
    {
        [Inject] private ICompanyService? CompanyService { get; set; }
        [Inject] private NavigationManager? Navigation { get; set; }
        [Inject] private DialogService? DialogService { get; set; }
        [Inject] private IJSRuntime? JSRuntime { get; set; }

        private CompanyViewModel? _company;
        private string _fullMailAddress = string.Empty;
        private string _fullDeliveryAddress = string.Empty;
        private List<DepartmentViewModel>? _departments;
        private List<ShiftViewModel>? _shifts;
        private const int CompanyID = 1;
        private bool isDepartmentDataLoading;
        private bool isShiftDataLoading;
        private DeptMemberDialogSettings? _settings;

        [CascadingParameter] public ErrorHandler? ErrorHandler { get; set; }

        protected async override Task OnInitializedAsync()
        {
            if (_company is null)
            {
                await GetCompanyAsync();
            }
        }

        private async Task GetCompanyAsync()
        {
            try
            {
                _company = await CompanyService!.GetCompanyByIdAsync(CompanyID);

                _fullMailAddress = _company.FullMailAddress;
                _fullDeliveryAddress = _company.FullDeliveryAddress;
                _departments = _company.Departments;
                _shifts = _company.Shifts;
            }
            catch (ApiResponseException ex)
            {
                ErrorHandler!.HandleExceptionAsync(ex);
                Navigation?.NavigateTo("/");
            }
            catch (Exception ex)
            {
                ErrorHandler!.HandleExceptionAsync(ex);
                Navigation?.NavigateTo("/");
            }
        }

        private async Task LoadDepartmentData(LoadDataArgs args)
        {
            isDepartmentDataLoading = true;

            if (_company is null)
            {
                await GetCompanyAsync();
            }

            isDepartmentDataLoading = false;
        }

        private async Task LoadShiftData(LoadDataArgs args)
        {
            isShiftDataLoading = true;

            if (_company is null)
            {
                await GetCompanyAsync();
            }

            isShiftDataLoading = false;
        }

        private async void ViewDepartmentMembers(DepartmentViewModel model)
        {
            await LoadStateAsync();

            await DialogService!.OpenAsync<DepartmentMemberListDialogPage>($"Department: {model.Name}",
                new Dictionary<string, object>() { { "DepartmentID", (int)model.DepartmentID } },
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

            Settings ??= new DeptMemberDialogSettings();

            Settings.Left = $"{point.X}px";
            Settings.Top = $"{point.Y}px";

            InvokeAsync(SaveStateAsync);
        }

        private void OnResize(System.Drawing.Size size)
        {
            JSRuntime!.InvokeVoidAsync("eval", $"console.log('Dialog resize. Width:{size.Width}, Height:{size.Height}')");

            Settings ??= new DeptMemberDialogSettings();

            Settings.Width = $"{size.Width}px";
            Settings.Height = $"{size.Height}px";

            InvokeAsync(SaveStateAsync);
        }

        private async Task SaveStateAsync()
        {
            await Task.CompletedTask;

            await JSRuntime!.InvokeVoidAsync("window.localStorage.setItem",
                                             "DeptMemberDialogSettings",
                                             JsonSerializer.Serialize<DeptMemberDialogSettings>(Settings));
        }

        private async Task LoadStateAsync()
        {
            await Task.CompletedTask;

            var result = await JSRuntime!.InvokeAsync<string>("window.localStorage.getItem",
                                                              "DeptMemberDialogSettings");

            if (!string.IsNullOrEmpty(result))
            {
                _settings = JsonSerializer.Deserialize<DeptMemberDialogSettings>(result);
            }
        }

        public DeptMemberDialogSettings Settings
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

        public sealed class DeptMemberDialogSettings
        {
            public string? Left { get; set; }
            public string? Top { get; set; }
            public string? Width { get; set; }
            public string? Height { get; set; }
        }
    }
}