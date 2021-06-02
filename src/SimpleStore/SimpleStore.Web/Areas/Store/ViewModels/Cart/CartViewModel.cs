using SimpleStore.Web.Areas.Store.ViewModels.Catalog;
using System.Collections.Generic;

namespace SimpleStore.Web.Areas.Store.ViewModels.Cart
{
    public class CartViewModel
    {
        public string Id { get; set; }
        public ICollection<CartItemViewModel> Items {get;set;}
        public decimal Subtotal { get; set; }
        public string SubtotalString { get; set; }
    }

    public class CartItemViewModel
    {
        public string Id { get; set; }
        public CatalogItemViewModel CatalogItem { get; set; }
        public int Quantity { get; set; }
    }
}