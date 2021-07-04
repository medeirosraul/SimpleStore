namespace SimpleStore.Web.Areas.Store.ViewModels.Catalog
{
    public class CatalogProductViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public decimal OldPrice { get; set; }
        public string PriceString { get; set; }
        public string OldPriceString { get; set; }
        public string Picture { get; set; }
    }
}