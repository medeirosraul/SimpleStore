namespace SimpleStore.Web.Areas.Store.ViewModels.Catalog
{
    public class CatalogProductViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public CatalogProductPriceViewModel Price { get; set; }
        public string Picture { get; set; }
    }
}