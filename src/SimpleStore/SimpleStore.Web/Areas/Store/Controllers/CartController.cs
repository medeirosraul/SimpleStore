using Microsoft.AspNetCore.Mvc;
using SimpleStore.Core.Entities.Carts;
using SimpleStore.Core.Services.Carts;
using SimpleStore.Core.Services.Catalog;
using SimpleStore.Core.Services.Monetaries;
using SimpleStore.Core.Services.Orders;
using SimpleStore.Core.Services.Pictures;
using SimpleStore.Core.Services.Products;
using SimpleStore.Core.Services.Shipping;
using SimpleStore.Framework.Contexts;
using SimpleStore.Web.Areas.Store.ViewModels.Cart;
using SimpleStore.Web.Areas.Store.ViewModels.Catalog;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleStore.Web.Areas.Store.Controllers
{
    [Area("Store")]
    public class CartController : Controller
    {
        private readonly IStoreContext _storeContext;
        private readonly ICustomerContext _customerContext;
        private readonly ICartService _cartService;
        private readonly ICatalogProductService _catalogItemService;
        private readonly IMonetaryService _monetaryService;
        private readonly IPictureProvider _pictureProvider;
        private readonly IOrderCalculationService _orderCalculationService;
        private readonly IShippingMethodServiceResolver _shippingMethodServiceResolver;

        public CartController(IStoreContext storeContext, ICustomerContext customerContext, ICartService cartService, ICatalogProductService catalogItemService, IPictureProvider pictureProvider, IMonetaryService monetaryService, IOrderCalculationService orderCalculationService, IShippingMethodServiceResolver shippingMethodServiceResolver)
        {
            _storeContext = storeContext;
            _customerContext = customerContext;
            _cartService = cartService;
            _catalogItemService = catalogItemService;
            _pictureProvider = pictureProvider;
            _monetaryService = monetaryService;
            _orderCalculationService = orderCalculationService;
            _shippingMethodServiceResolver = shippingMethodServiceResolver;
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

        public async Task<IActionResult> EstimateShipping([FromForm] string zipcode)
        {
            // Clear current options
            var cart = await _cartService.GetByCustomerId(_customerContext.CurrentCustomer.Id);
            cart.ShippingZipCode = zipcode;
            await _cartService.Update(cart);
            await _cartService.ClearShippingOptions(cart.Id);

            // Get new estimatives
            var shippingMethodService = _shippingMethodServiceResolver.GetByName("MelhorEnvio");
            var options = await shippingMethodService.GetShippingOptions(cart, zipcode);

            if (options != null && options.Count > 0)
            {
                var cartShippingOptions = new List<CartShippingOption>();
                foreach (var option in options)
                {
                    cartShippingOptions.Add(new CartShippingOption
                    {
                        Name = option.Name,
                        Description = option.Description,
                        Value = option.Value,
                        Method = "MelhorEnvio"
                    });
                }

                // Pre-select minor value
                cartShippingOptions.First().Selected = true;

                await _cartService.UpdateShippingOptions(cart.Id, cartShippingOptions);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> SelectShippingOption([FromForm] string shippingOption)
        {
            var cart = await _cartService.GetByCustomerId(_customerContext.CurrentCustomer.Id);
            await _cartService.UpdateSelectedShippingOption(cart.Id, shippingOption);

            return RedirectToAction(nameof(Index));
        }

        private async Task<CartViewModel> CreateCartViewModel(Cart cart)
        {
            // Cart
            var cartViewModel = new CartViewModel
            {
                Id = cart.Id,
                ShippingZipCode = cart.ShippingZipCode,
                Items = new List<CartItemViewModel>(),
                ShippingOptions = new List<CartShippingOptionViewModel>()
            };

            // Items
            foreach (var item in cart.Items)
            {
                var catalogItem = await _catalogItemService.GetById(item.CatalogItemId);
                var catalogItemViewModel = new CatalogProductViewModel
                {
                    Id = catalogItem.Id,
                    Name = catalogItem.Name,
                    Picture = _pictureProvider.GetCatalogProductPictureUrl(catalogItem, 200),
                    Price = catalogItem.OldPrice,
                    OldPrice = catalogItem.Price,
                    OldPriceString = _monetaryService.GetValueString(catalogItem.OldPrice),
                    PriceString = _monetaryService.GetValueString(catalogItem.Price)
                };

                var cartItemViewModel = new CartItemViewModel
                {
                    Id = item.Id,
                    Quantity = item.Quantity,
                    CatalogItem = catalogItemViewModel
                };

                cartViewModel.Items.Add(cartItemViewModel);
            }

            // Shipping Options
            if (cart.ShippingOptions != null)
            {
                foreach (var option in cart.ShippingOptions)
                {
                    var optionViewModel = new CartShippingOptionViewModel
                    {
                        Id = option.Id,
                        Method = option.Method,
                        Name = option.Name,
                        Description = option.Description,
                        Value = option.Value,
                        ValueString = _monetaryService.GetValueString(option.Value),
                        Selected = option.Selected
                    };

                    cartViewModel.ShippingOptions.Add(optionViewModel);
                }

                cartViewModel.ShippingOptions = cartViewModel.ShippingOptions.OrderBy(x => x.Value).ToList();
            }

            // Values
            cartViewModel.Subtotal = await _orderCalculationService.GetSubtotal(cart);
            cartViewModel.ShippingValue = await _orderCalculationService.GetShippingValue(cart);
            cartViewModel.Total = await _orderCalculationService.GetTotal(cart);

            cartViewModel.SubtotalString = _monetaryService.GetValueString(cartViewModel.Subtotal);
            cartViewModel.ShippingValueString = _monetaryService.GetValueString(cartViewModel.ShippingValue);
            cartViewModel.TotalString = _monetaryService.GetValueString(cartViewModel.Total);

            return cartViewModel;
        }
    }
}
