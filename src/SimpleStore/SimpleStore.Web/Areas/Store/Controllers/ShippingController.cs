using Microsoft.AspNetCore.Mvc;
using SimpleStore.Core.Entities.Carts;
using SimpleStore.Core.Services.Carts;
using SimpleStore.Core.Services.Shipping;
using SimpleStore.Framework.Contexts;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleStore.Web.Areas.Store.Controllers
{
    [Area("Store")]
    public class ShippingController : Controller
    {
        private readonly ICustomerContext _customerContext;
        private readonly ICartService _cartService;
        private readonly IShippingService _shippingService;

        public ShippingController(ICustomerContext customerContext, ICartService cartService, IShippingService shippingService)
        {
            _customerContext = customerContext;
            _cartService = cartService;
            _shippingService = shippingService;
        }

        [HttpPost]
        public async Task<IActionResult> Calculate([FromForm] string zipcode, [FromForm] string callback)
        {
            var cart = await _cartService.GetByCustomerId(_customerContext.CurrentCustomer.Id);

            await _shippingService.CalculateOptions(cart, zipcode);

            callback = string.IsNullOrEmpty(callback) ? "/Cart" : callback;
            return Redirect(callback);
        }
    }
}