

namespace WebUI.Store.Features.Products.ViewProductListItems
{
    public class ProductListItemsGridState
    {
        public bool Initialized { get; set; }
        public int CurrentPage { get; set; }
        public DocumentPage<ProductListItemViewModel>? ProductListItems { get; set; }
        public IList<ProductListItemViewModel>? SelectedProduct { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}