namespace WebUI.Store.Features.Products.ViewProductListItems
{
    public static class ViewProductListItemReducers
    {
        [ReducerMethod(typeof(SetLoadingFlagAction))]
        public static ProductListItemsState OnGetProductListItemsAction
        (
            ProductListItemsState state
        )
        {
            return state with
            {
                Loading = true
            };
        }

        [ReducerMethod]
        public static ProductListItemsState OnGGetProductListItemsSuccessAction
        (
            ProductListItemsState state,
            GetProductListItemsSuccessAction action
        )
        {
            return state with
            {
                ProductListItems = action.ProductListItems,
                SelectedProduct = action.SelectedProducts,
                Loading = false,
                Initialized = true,
                LastUpdated = DateTime.Now
            };
        }

        [ReducerMethod]
        public static ProductListItemsState OnGetProductListItemsFailureAction
        (
            ProductListItemsState state,
            GetProductListItemsFailureAction action
        )
        {
            return state with
            {
                ErrorMessage = action.ErrorMessage,
                Loading = false,
                Initialized = false
            };
        }
    }
}