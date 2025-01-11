using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using WebUI.Components;
using WebUI.Exceptions;
using WebUI.Models;
using WebUI.Models.CompanyApi;
using WebUI.Services.Repositories.Company;

namespace WebUI.Pages.Company
{
    public partial class EmployeeList
    {
        private string lastNameFilter = string.Empty;
        private readonly bool altcolor = true;
        private GridItemsProvider<EmployeeListItemViewModel> _employeeItemProvider = default!;
        private EmployeeDetailViewModel _employee = new();
        private readonly PaginationState pagination = new() { ItemsPerPage = 5 };
        private DocumentPage<EmployeeListItemViewModel>? _documentPage;
        [Inject] public ICompanyService? CompanyService { get; set; }
        [Inject] public IDialogService? DialogService { get; set; }
        [CascadingParameter] public ErrorHandler? ErrorHandler { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await GoToPageAsync(pagination.CurrentPageIndex);
        }

        private async Task GoToPageAsync(int pageIndex)
        {
            _employeeItemProvider = async req =>
            {
                string searchField = "LastName";
                string searchCriteria = lastNameFilter;
                string orderBy = "LastName";
                Console.WriteLine("PageIndex {0}, req.Count {1}", pageIndex, req.Count);
                _documentPage = await CompanyService!.GetEmployeesFilteredByNameAsync(searchField,
                                                                                      searchCriteria,
                                                                                      orderBy,
                                                                                      pageIndex + 1,
                                                                                      req.Count ?? 12);

                return GridItemsProviderResult.From<EmployeeListItemViewModel>(
                    items: _documentPage!.Data,
                    totalItemCount: _documentPage.MetaData!.TotalRecords);
            };

            pagination.TotalItemCountChanged += (_, __) => StateHasChanged();
            await pagination.SetCurrentPageIndexAsync(pageIndex);
        }

        private async Task OnLastNameFilterChanged(ChangeEventArgs args)
        {
            if (args.Value is string value)
            {
                lastNameFilter = value;
                await GoToPageAsync(0);
            }
        }

        private async Task OnLastNameFilterClear()
        {
            if (string.IsNullOrWhiteSpace(lastNameFilter))
            {
                lastNameFilter = string.Empty;
            }

            await GoToPageAsync(0);
        }

        private async void HandleRowFocus(int employeeId)
        {
            try
            {
                _employee = await CompanyService!.GetEmployeeByIdAsync(employeeId);
                _employee.BirthDateAsString = _employee.BirthDate.ToShortDateString();
                _employee.HireDateAsString = _employee.HireDate.ToShortDateString();

                await OpenDialogAsync();
            }
            catch (ApiResponseException ex)
            {
                var dialog = await DialogService!.ShowErrorAsync(ex.Message);
                await dialog.Result;
            }
            catch (Exception ex)
            {
                await ErrorHandler!.HandleExceptionAsync(ex);
            }
        }

        private async Task OpenDialogAsync()
        {
            DialogParameters parameters = new()
            {
                Title = $"Viewing details for {_employee.EmployeeName}",
                PrimaryAction = "Close",
                PrimaryActionEnabled = true,
                Width = "800px",
                TrapFocus = true,
                Modal = true,
                PreventScroll = false
            };

            IDialogReference dialog = await DialogService!.ShowDialogAsync<EmployeeReadOnlyDetailsDialog>(_employee, parameters);
            DialogResult? result = await dialog.Result;

            if (result.Data is not null)
            {
                EmployeeDetailViewModel? employee = result.Data as EmployeeDetailViewModel;
                Console.WriteLine($"Dialog closed by {employee!.EmployeeName} - Canceled: {result.Cancelled}");
            }
            else
            {
                Console.WriteLine($"Dialog closed - Canceled: {result.Cancelled}");
            }
        }
    }
}