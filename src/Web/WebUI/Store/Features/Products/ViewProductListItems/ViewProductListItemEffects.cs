using WebUI.Services.Repositories.Product;

namespace WebUI.Store.Features.Products.ViewProductListItems
{
    public class ViewProductListItemEffects
    (
        IProductService? productService,
        NotificationService notificationService
    ) : Effect<GetProductListItemsAction>
    {
        private readonly IProductService? _productService = productService;
        private readonly NotificationService? _notificationService = notificationService;

        public override async Task HandleAsync
        (
            GetProductListItemsAction action,
            IDispatcher dispatcher
        )
        {
            try
            {
                dispatcher.Dispatch(new SetLoadingFlagAction());

                DocumentPage<ProductListItemViewModel> products =
                    await _productService!.GetProductsFilteredByNameAsync(action.StringSearchCriteria.SearchField!,
                                                                          action.StringSearchCriteria.SearchCriteria!,
                                                                          action.StringSearchCriteria.OrderBy!,
                                                                          action.StringSearchCriteria.Skip,
                                                                          action.StringSearchCriteria.Take);

                List<ProductListItemViewModel>? selectedProduct = [products.Data.FirstOrDefault()!];

                dispatcher.Dispatch(new GetProductListItemsSuccessAction(products, selectedProduct));
            }
            catch (Exception ex)
            {
                ShowErrorNotification.ShowError(
                    _notificationService!,
                    Helpers.GetExceptionMessage(ex)
                );

                dispatcher.Dispatch(new GetProductListItemsFailureAction(Helpers.GetExceptionMessage(ex)));
            }
        }
    }
}