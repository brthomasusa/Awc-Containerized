using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using WebUI.Components;
using WebUI.Exceptions;
using WebUI.Models.CompanyApi;
using WebUI.Services.Repositories.Company;

namespace WebUI.Pages.Company
{
    public partial class Company : ComponentBase
    {
        [Inject] public ICompanyService? CompanyService { get; set; }
        [Inject] public IDialogService? DialogService { get; set; }
        [Inject] private NavigationManager? Navigation { get; set; }

        private CompanyViewModel _company = new();
        private string _fullMailAddress = string.Empty;
        private string _fullDeliveryAddress = string.Empty;
        private IQueryable<DepartmentViewModel>? _departments;
        private IQueryable<ShiftViewModel>? _shifts;
        private readonly PaginationState pagination = new() { ItemsPerPage = 4 };
        private const int CompanyID = 1;
        [CascadingParameter] public ErrorHandler? ErrorHandler { get; set; }


        protected async override Task OnInitializedAsync()
        {
            try
            {
                _company = await CompanyService!.GetCompanyByIdAsync(CompanyID);

                _fullMailAddress = _company.FullMailAddress;
                _fullDeliveryAddress = _company.FullDeliveryAddress;
                _departments = _company.Departments.AsQueryable();
                await pagination.SetTotalItemCountAsync(_departments!.Count());
                _shifts = _company.Shifts.AsQueryable();
            }
            catch (ApiResponseException ex)
            {
                var dialog = await DialogService!.ShowErrorAsync(ex.Message);
                await dialog.Result;
                Navigation?.NavigateTo("/companydata/companyhomepage");
            }
            catch (Exception ex)
            {
                await ErrorHandler!.HandleExceptionAsync(ex);
                Navigation?.NavigateTo("/companydata/companyhomepage");
            }
        }


        private static void OnBreakpointEnterHandler(GridItemSize size)
        {
            Console.WriteLine($"Page Size: {size}");
        }

        private string FullMailAddress
        {
            get => _fullMailAddress;
            set { }
        }

        private string FullDeliveryAddress
        {
            get => _fullDeliveryAddress;
            set { }
        }
    }
}