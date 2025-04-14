using System.Text.Json;
using Microsoft.AspNetCore.Components;
using WebUI.Exceptions;
using WebUI.Services.Repositories.Product;
using WebUI.Store.Features.Products.ViewProductListItems;

namespace WebUI.Pages.Features.Products.ViewProduct
{
    public partial class ProductListPage : Fluxor.Blazor.Web.Components.FluxorComponent
    {
        [Inject] private IProductService? ProductService { get; set; }
        [Inject] public DialogService? DialogService { get; set; }
        [Inject] private NotificationService? NotificationService { get; set; }
        [Inject] private NavigationManager? Navigation { get; set; }
        [Inject] private IJSRuntime? JSRuntime { get; set; }
        private DocumentPage<ProductListItemViewModel>? _products;
        private IList<ProductListItemViewModel>? _selectedProduct;
        private string _productNameFilter = string.Empty;
        private readonly IEnumerable<int> pageSizeOptions = [5, 10, 15, 20];
        private ProductDialogSettings? _settings;
        private bool isLoading;

        private ProductDialogSettings Settings
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


        protected async override Task OnInitializedAsync()
        {
            try
            {
                await base.OnInitializedAsync();

                _products = await ProductService!.GetProductsFilteredByNameAsync("Name", _productNameFilter, string.Empty, 0, 20);
                _selectedProduct = [_products.Data.FirstOrDefault()!];

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

        private async Task GetProductListItemViewModel(LoadDataArgs args)
        {
            try
            {
                isLoading = true;
                await Task.Yield();

                if (args.Filters is not null)
                {
                    List<FilterDescriptor> descriptors = [.. args.Filters];
                    FilterDescriptor? filterDescriptor
                        = descriptors.Find(x => !string.IsNullOrEmpty(x.Property) && !string.IsNullOrEmpty(x.FilterValue.ToString()));

                    if (filterDescriptor is not null)
                    {
                        _productNameFilter = filterDescriptor.FilterValue.ToString()!;
                    }
                    else
                    {
                        _productNameFilter = string.Empty;
                    }
                }
                else
                {
                    _productNameFilter = string.Empty;
                }
                _products = await ProductService!.GetProductsFilteredByNameAsync("Name", _productNameFilter, string.Empty, args.Skip ?? default, args.Top ?? default);
                _selectedProduct = [_products.Data.FirstOrDefault()!];

                isLoading = false;
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

        private async void ViewProductDetailViewModel(ProductListItemViewModel model)
        {
            await ViewProductDetails(model.ProductID, model.Name!);
        }

        private async Task OnRowDoubleClicked(DataGridRowMouseEventArgs<ProductListItemViewModel> args)
        {
            await ViewProductDetails(args.Data.ProductID, args.Data.Name!);
        }

        private async Task ViewProductDetails(int productId, string productName)
        {
            await DialogService!.OpenAsync<ViewProductDetailDialogPage>(productName,
                new Dictionary<string, object>() { { "ProductID", productId } },
                new DialogOptions()
                {
                    Resizable = true,
                    Draggable = true,
                    Resize = OnResize,
                    Drag = OnDrag,
                    Width = Settings != null ? Settings.Width : "1000px",
                    Height = Settings != null ? Settings.Height : "675px",
                    Left = Settings?.Left,
                    Top = Settings?.Top
                });
        }

        private void OnDrag(System.Drawing.Point point)
        {
            _ = JSRuntime!.InvokeVoidAsync("eval", $"console.log('Dialog drag. Left:{point.X}, Top:{point.Y}')");

            Settings ??= new ProductDialogSettings();

            Settings.Left = $"{point.X}px";
            Settings.Top = $"{point.Y}px";

            InvokeAsync(SaveStateAsync);
        }

        private void OnResize(System.Drawing.Size size)
        {
            JSRuntime!.InvokeVoidAsync("eval", $"console.log('Dialog resize. Width:{size.Width}, Height:{size.Height}')");

            Settings ??= new ProductDialogSettings();

            Settings.Width = $"{size.Width}px";
            Settings.Height = $"{size.Height}px";

            InvokeAsync(SaveStateAsync);
        }

        private async Task SaveStateAsync()
        {
            await Task.CompletedTask;

            await JSRuntime!.InvokeVoidAsync("window.localStorage.setItem",
                                             "ProductDetailViewModelDialogSettings",
                                             JsonSerializer.Serialize<ProductDialogSettings>(Settings));
        }

        private async Task LoadStateAsync()
        {
            await Task.CompletedTask;

            var result = await JSRuntime!.InvokeAsync<string>("window.localStorage.getItem",
                                                              "ProductDetailViewModelDialogSettings");

            if (!string.IsNullOrEmpty(result))
            {
                _settings = JsonSerializer.Deserialize<ProductDialogSettings>(result);
            }
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