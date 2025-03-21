using System.Text.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;
using WebUI.Exceptions;
using WebUI.Models;
using WebUI.Models.ProductApi;
using WebUI.Services.Repositories.Product;
using WebUI.Utilities;

namespace WebUI.Pages.Features.Products
{
    public partial class ProductListPage
    {
        [Inject] private IProductService? ProductService { get; set; }
        [Inject] public DialogService? DialogService { get; set; }
        [Inject] private NotificationService? NotificationService { get; set; }
        [Inject] private NavigationManager? Navigation { get; set; }
        [Inject] private IJSRuntime? JSRuntime { get; set; }
        private IQueryable<ProductListItemViewModel>? _products;
        private IList<ProductListItemViewModel>? _selectedProduct;
        private string _productNameFilter = string.Empty;
        private readonly IEnumerable<int> pageSizeOptions = [5, 10, 15, 20];
        // private bool isLoading;
        // private ProductDialogSettings? _settings;


        protected async override Task OnInitializedAsync()
        {
            try
            {
                _products ??= await ProductService!.GetProductsListItemsAync();
                _selectedProduct = [_products.FirstOrDefault()!];
            }
            catch (ApiResponseException ex)
            {
                ShowErrorNotification.ShowError(
                    NotificationService!,
                    ex.Message
                );

                Navigation?.NavigateTo("/");
            }
            catch (Exception ex)
            {
                ShowErrorNotification.ShowError(
                    NotificationService!,
                    ex.Message
                );

                Navigation?.NavigateTo("/");
            }
        }

        protected void OnRowDoubleClicked(DataGridRowMouseEventArgs<ProductListItemViewModel> args)
        {
            Console.WriteLine($"ProductID -> {args.Data.ProductID}");
        }

    }


    public sealed class ProductDialogSettings
    {
        public string? Left { get; set; }
        public string? Top { get; set; }
        public string? Width { get; set; }
        public string? Height { get; set; }
    }
}