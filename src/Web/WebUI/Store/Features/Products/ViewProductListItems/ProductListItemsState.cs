namespace WebUI.Store.Features.Products.ViewProductListItems
{
    public record ProductListItemsState
    {
        public bool Initialized { get; init; }
        public bool Loading { get; init; }
        public string? ErrorMessage { get; init; }
        public DocumentPage<ProductListItemViewModel>? ProductListItems { get; init; }
        public List<ProductListItemViewModel>? SelectedProduct { get; init; }
        public string? ProductNameFilter { get; init; }
        public DateTime LastUpdated { get; init; }
    }
}