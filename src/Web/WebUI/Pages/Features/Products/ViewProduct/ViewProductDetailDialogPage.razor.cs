using System.Text.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;
using WebUI.Exceptions;
using WebUI.Models;
using WebUI.Models.ProductApi;
using WebUI.Services.Repositories.Product;
using WebUI.Utilities;

namespace WebUI.Pages.Features.Products.ViewProduct
{
    public partial class ViewProductDetailDialogPage
    {
        [Parameter(CaptureUnmatchedValues = true)]
        public IReadOnlyDictionary<string, dynamic>? Attributes { get; set; }
        [Parameter] public dynamic? ProductID { get; set; }
        [Parameter] public bool ShowClose { get; set; } = true;
        [Inject] private IProductService? ProductService { get; set; }
        [Inject] private NotificationService? NotificationService { get; set; }
        [Inject] private NavigationManager? Navigation { get; set; }
        [Inject] private DialogService? DialogService { get; set; }
        private ProductDetailViewModel? _product;
        private string? _makeFlag;
        private string? _finishGoodsFlag;
        private int _weight = 0;
        private string? _sellStartDate;
        private string? _sellEndDate;
        private string? _discontinuedDate;

        protected async override Task OnInitializedAsync()
        {
            try
            {
                _product = await ProductService!.GetProductByIdAync(ProductID);

                _makeFlag = ConvertBooleanToString(_product.MakeFlag);
                _finishGoodsFlag = ConvertBooleanToString(_product.FinishedGoodsFlag);
                _sellStartDate = ConvertDateToString(_product.SellStartDate);
                _sellEndDate = ConvertDateToString(_product.SellEndDate);
                _discontinuedDate = ConvertDateToString(_product.DiscontinuedDate);

                await base.OnInitializedAsync();
            }
            catch (ApiResponseException ex)
            {
                ShowErrorNotification.ShowError(
                    NotificationService!,
                    ex.Message
                );

                Navigation?.NavigateTo("/Pages/Features/Products/ViewProduct/ProductListPage");
            }
            catch (Exception ex)
            {
                ShowErrorNotification.ShowError(
                    NotificationService!,
                    ex.Message
                );

                Navigation?.NavigateTo("/Pages/Features/Products/ViewProduct/ProductListPage");
            }
        }

        private static string ConvertBooleanToString(bool value)
            => value ? "Yes" : "No";

        private static string ConvertDateToString(DateTime date)
        {
            if (date == default)
                return string.Empty;

            return date.ToShortDateString();
        }
    }
}