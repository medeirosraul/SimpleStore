using Microsoft.AspNetCore.Mvc;
using SimpleStore.Core.Entities.Carts;
using SimpleStore.Core.Services.Carts;
using SimpleStore.Core.Services.Catalog;
using SimpleStore.Core.Services.Monetaries;
using SimpleStore.Core.Services.Orders;
using SimpleStore.Core.Services.Pictures;
using SimpleStore.Core.Services.Shipping;
using SimpleStore.Framework.Contexts;
using SimpleStore.Web.Areas.Store.ViewModelProviders;

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
        private readonly ICartViewModelProvider _cartViewModelProvider;

        public CartController(IStoreContext storeContext, ICustomerContext customerContext, ICartService cartService, ICatalogProductService catalogItemService, IPictureProvider pictureProvider, IMonetaryService monetaryService, IOrderCalculationService orderCalculationService, IShippingMethodServiceResolver shippingMethodServiceResolver, ICartViewModelProvider cartViewModelProvider)
        {
            _storeContext = storeContext;
            _customerContext = customerContext;
            _cartService = cartService;
            _catalogItemService = catalogItemService;
            _pictureProvider = pictureProvider;
            _monetaryService = monetaryService;
            _orderCalculationService = orderCalculationService;
            _cartViewModelProvider = cartViewModelProvider;
        }

        public async Task<IActionResult> Index()
        {
            var cart = await _cartService.GetByCustomerId(_customerContext.CurrentCustomer.Id);
            var cartViewModel = await _cartViewModelProvider.CreateCartViewModel(cart);

            return View(cartViewModel);
        }

        public async Task<IActionResult> AddItem([FromForm]string itemId, [FromForm] int quantity)
        {
            var cart = await _cartService.GetByCustomerId(_customerContext.CurrentCustomer.Id);
            await _cartService.AddCartItem(cart, itemId, quantity);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> RemoveItem([FromForm] string itemId, [FromForm] int quantity)
        {
            var cart = await _cartService.GetByCustomerId(_customerContext.CurrentCustomer.Id);
            await _cartService.RemoveCartItem(cart, itemId, quantity);

            return RedirectToAction(nameof(Index)); ;
        }

        public async Task<IActionResult> EstimateShipping([FromForm] string zipcode)
        {
            var cart = await _cartService.GetByCustomerId(_customerContext.CurrentCustomer.Id);

            await _cartService.CalculateShippingOptions(cart.Id, zipcode);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> SelectShippingOption([FromForm] string shippingOption)
        {
            var cart = await _cartService.GetByCustomerId(_customerContext.CurrentCustomer.Id);
            await _cartService.UpdateSelectedShippingOption(cart, shippingOption);

            return RedirectToAction(nameof(Index));
        }
    }
}