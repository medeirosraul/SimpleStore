using SimpleStore.Web.Areas.Store.ViewModels.Catalog;
using System.Collections.Generic;

namespace SimpleStore.Web.Areas.Store.ViewModels.Cart
{
    public class CartViewModel
    {
        public string Id { get; set; }
        public ICollection<CartItemViewModel> Items {get;set;}
        public ICollection<CartShippingOptionViewModel> ShippingOptions { get; set; }
        public string ShippingZipCode { get; set; }
        public decimal Subtotal { get; set; }
        public string SubtotalString { get; set; }
        public decimal ShippingValue { get; set; }
        public string ShippingValueString { get; set; }
        public decimal Total { get; set; }
        public string TotalString { get; set; }
    }

    public class CartItemViewModel
    {
        public string Id { get; set; }
        public CatalogProductViewModel CatalogItem { get; set; }
        public int Quantity { get; set; }
    }

    public class CartShippingOptionViewModel
    {
        public string Id { get; set; }
        public string Method { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Value { get; set; }
        public string ValueString { get; set; }
        public bool Selected { get; set; }
    }
}