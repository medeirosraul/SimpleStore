using Microsoft.AspNetCore.Mvc;
using SimpleStore.Core.Entities.Carts;
using SimpleStore.Core.Services.Carts;
using SimpleStore.Core.Services.Monetaries;
using SimpleStore.Core.Services.Orders;
using SimpleStore.Core.Services.Pictures;
using SimpleStore.Core.Services.Prices;
using SimpleStore.Core.Services.Products;
using SimpleStore.Framework.Contexts;
using SimpleStore.Web.Areas.Store.ViewModels.Cart;
using SimpleStore.Web.Areas.Store.ViewModels.Catalog;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleStore.Web.Areas.Store.Controllers
{
    [Area("Store")]
    public class CartController : Controller
    {
        private readonly IStoreContext _storeContext;
        private readonly ICustomerContext _customerContext;
        private readonly ICartService _cartService;
        private readonly ICatalogItemService _catalogItemService;
        private readonly IMonetaryService _monetaryService;
        private readonly IPriceProvider _priceProvider;
        private readonly IPictureProvider _pictureProvider;
        private readonly IOrderCalculationService _orderCalculationService;

        public CartController(IStoreContext storeContext, ICustomerContext customerContext, ICartService cartService, ICatalogItemService catalogItemService, IPriceProvider priceProvider, IPictureProvider pictureProvider, IMonetaryService monetaryService, IOrderCalculationService orderCalculationService)
        {
            _storeContext = storeContext;
            _customerContext = customerContext;
            _cartService = cartService;
            _catalogItemService = catalogItemService;
            _priceProvider = priceProvider;
            _pictureProvider = pictureProvider;
            _monetaryService = monetaryService;
            _orderCalculationService = orderCalculationService;
        }

        public async Task<IActionResult> Index()
        {
            var cart = await _cartService.GetByCustomerId(_customerContext.CurrentCustomer.Id);
            var cartViewModel = await CreateCartViewModel(cart);

            return View(cartViewModel);
        }

        public async Task<IActionResult> AddItem([FromForm]string itemId, [FromForm] int quantity)
        {
            var cart = await _cartService.GetByCustomerId(_customerContext.CurrentCustomer.Id);
            await _cartService.AddCartItem(cart.Id, itemId, quantity);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> RemoveItem([FromForm] string itemId, [FromForm] int quantity)
        {
            var cart = await _cartService.GetByCustomerId(_customerContext.CurrentCustomer.Id);
            await _cartService.RemoveCartItem(cart.Id, itemId, quantity);

            return RedirectToAction(nameof(Index)); ;
        }

        private async Task<CartViewModel> CreateCartViewModel(Cart cart)
        {
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
                    PriceOldValueString = _monetaryService.GetValueString(price.OldValue),
                    PriceValueString = _monetaryService.GetValueString(price.Value)
                };

                var cartItemViewModel = new CartItemViewModel
                {
                    Id = item.Id,
                    Quantity = item.Quantity,
                    CatalogItem = catalogItemViewModel
                };

                cartViewModel.Items.Add(cartItemViewModel);
            }

            cartViewModel.Subtotal = await _orderCalculationService.GetSubtotal(cart);
            cartViewModel.SubtotalString = _monetaryService.GetValueString(cartViewModel.Subtotal);

            return cartViewModel;
        }
    }
}
