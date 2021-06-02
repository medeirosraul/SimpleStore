using Microsoft.AspNetCore.Mvc;
using SimpleStore.Core.Services.Carts;
using SimpleStore.Core.Services.Pictures;
using SimpleStore.Core.Services.Prices;
using SimpleStore.Core.Services.Products;
using SimpleStore.Framework.Contexts;
using SimpleStore.Web.Areas.Store.ViewModels.Cart;
using SimpleStore.Web.Areas.Store.ViewModels.Catalog;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleStore.Web.Areas.Store.ViewComponents
{
    public class CartViewComponent : ViewComponent
    {
        private readonly IStoreContext _storeContext;
        private readonly ICustomerContext _customerContext;
        private readonly ICartService _cartService;
        private readonly ICatalogItemService _catalogItemService;
        private readonly IPictureProvider _pictureProvider;
        private readonly IPriceProvider _priceProvider;

        public CartViewComponent(IStoreContext storeContext, ICustomerContext customerContext, ICartService cartService, ICatalogItemService catalogItemService, IPriceProvider priceProvider, IPictureProvider pictureProvider)
        {
            _storeContext = storeContext;
            _customerContext = customerContext;
            _cartService = cartService;
            _catalogItemService = catalogItemService;
            _priceProvider = priceProvider;
            _pictureProvider = pictureProvider;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var cart = await _cartService.GetByCustomerId(_customerContext.CurrentCustomer.Id);
            var cartViewModel = new CartViewModel
            {
                Id = cart.Id,
                Items = new List<CartItemViewModel>()
            };

            foreach (var item in cart.Items)
            {
                var catalogItem = await _catalogItemService.GetById(item.CatalogItemId);
                var price = await _priceProvider.GetCatalogItemPrice(catalogItem);

                var catalogItemViewModel = new CatalogItemViewModel
                {
                    Id = catalogItem.Id,
                    Name = catalogItem.Name,
                    Picture = _pictureProvider.GetCatalogItemPictureUrl(catalogItem, 200),
                    PriceOldValue = price.OldValue,
                    PriceValue = price.Value,
                    PriceOldValueString = _priceProvider.GetPriceOldValueString(price),
                    PriceValueString = _priceProvider.GetPriceValueString(price)
                };

                var cartItemViewModel = new CartItemViewModel
                {
                    Id = item.Id,
                    Quantity = item.Quantity,
                    CatalogItem = catalogItemViewModel
                };

                cartViewModel.Items.Add(cartItemViewModel);
            }

            return View(cartViewModel);
        }
    }
}