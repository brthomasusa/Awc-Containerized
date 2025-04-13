namespace WebUI.Store.Features.Products.ViewProductListItems
{
    public class ViewProductListItemFeature : Feature<ProductListItemsState>
    {
        public override string GetName() => "ProductListItems";

        protected override ProductListItemsState GetInitialState()
        {
            return new ProductListItemsState
            {
                Initialized = false,
                Loading = false,
                ErrorMessage = string.Empty,
                ProductListItems = new DocumentPage<ProductListItemViewModel>() { MetaData = new(0, 0, 0), Data = [] },
                SelectedProduct = [],
                ProductNameFilter = string.Empty,
                LastUpdated = default
            };
        }
    }
}