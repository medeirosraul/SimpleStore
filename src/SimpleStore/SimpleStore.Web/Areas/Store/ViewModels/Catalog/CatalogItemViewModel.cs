namespace SimpleStore.Web.Areas.Store.ViewModels.Catalog
{
    public class CatalogItemViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public decimal PriceOldValue { get; set; }
        public string PriceOldValueString { get; set; }
        public decimal PriceValue { get; set; }
        public string PriceValueString { get; set; }
        public string Picture { get; set; }
    }
}