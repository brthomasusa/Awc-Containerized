namespace WebUI.Store.Features.Products.ViewProductListItems
{
    public record GetProductListItemsAction(StringSearchCriteria StringSearchCriteria);
    public record GetProductListItemsSuccessAction(DocumentPage<ProductListItemViewModel> ProductListItems, List<ProductListItemViewModel> SelectedProducts);
    public record GetProductListItemsFailureAction(string ErrorMessage);
}