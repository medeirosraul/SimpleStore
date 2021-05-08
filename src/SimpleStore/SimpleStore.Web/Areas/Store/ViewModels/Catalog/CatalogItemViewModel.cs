namespace SimpleStore.Web.Areas.Store.ViewModels.Catalog
{
    public class CatalogItemViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public CatalogItemPriceViewModel Price { get; set; }
        public string Picture { get; set; }
    }
}