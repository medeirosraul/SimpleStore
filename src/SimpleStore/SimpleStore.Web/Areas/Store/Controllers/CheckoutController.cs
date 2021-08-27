using Microsoft.AspNetCore.Mvc;
using SimpleStore.Core.Entities.Customers;
using SimpleStore.Core.Services.Carts;
using SimpleStore.Core.Services.Customers;
using SimpleStore.Core.Services.Orders;
using SimpleStore.Core.Services.Payments;
using SimpleStore.Core.Services.Shipping;
using SimpleStore.Framework.Contexts;
using SimpleStore.Web.Areas.Store.ViewModelProviders;
using SimpleStore.Web.Areas.Store.ViewModels.Checkout;
using SimpleStore.Web.Areas.Store.ViewModels.MyAccount;
using SimpleStore.Web.Areas.Store.ViewModels.Payment;

namespace SimpleStore.Web.Areas.Store.Controllers
{
    [Area("Store")]
    public class CheckoutController : Controller
    {
        private readonly ICustomerContext _customerContext;
        private readonly ICustomerAddressService _customerAddressService;
        private readonly ICartService _cartService;
        private readonly ICartViewModelProvider _cartViewModelProvider;
        private readonly IPaymentServiceResolver _paymentServiceResolver;
        private readonly IShippingService _shippingService;
        private readonly IOrderProcessingService _orderProcessingService;

        public CheckoutController(
            ICartViewModelProvider cartViewModelProvider,
            ICustomerContext customerContext,
            ICartService cartService,
            IPaymentServiceResolver paymentServiceResolver,
            IShippingService shippingService,
            ICustomerAddressService customerAddressService,
            IOrderProcessingService orderProcessingService)
        {
            _cartViewModelProvider = cartViewModelProvider;
            _customerContext = customerContext;
            _cartService = cartService;
            _paymentServiceResolver = paymentServiceResolver;
            _shippingService = shippingService;
            _customerAddressService = customerAddressService;
            _orderProcessingService = orderProcessingService;
        }

        public IActionResult Index()
        {
            // Redirect to First Step
            return RedirectToAction(nameof(ShippingAddress));
        }

        [HttpGet]
        public async Task<IActionResult> ShippingAddress()
        {
            var viewmodel = await CreateCheckoutModel();
            return View(viewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ShippingAddress([FromForm] string addressId, [FromForm] CustomerAddressViewModel newAddress)
        {
            var customer = _customerContext.CurrentCustomer;
            var cart = await _cartService.GetByCustomerId(_customerContext.CurrentCustomer.Id);
            var model = await CreateCheckoutModel();
            model.NewAddress = newAddress;

            // If existent address
            if (!string.IsNullOrEmpty(addressId))
            {
                var existingAddress = customer.Addresses.FirstOrDefault(x => x.Id == addressId);

                if (existingAddress == null)
                {
                    ModelState.AddModelError("InvalidAddressError", "O endereço selecionado é inválido. Selecione outro endereço ou cadastre um novo.");
                    return RedirectToAction(nameof(ShippingAddress));
                }

                await _customerContext.UpdateShippingAddress(addressId);
                await _cartService.ClearShippingOptions(cart.Id);

                return RedirectToAction(nameof(ShippingMethod));
            }

            // If new address
            if (!ModelState.IsValid)
                return View(model);

            var address = new CustomerAddress
            {
                CustomerId = customer.Id,
                Responsible = newAddress.Responsible,
                ZipCode = newAddress.ZipCode,
                Address = newAddress.Address,
                Number = newAddress.Number,
                Complement = newAddress.Complement,
                Neighborhood = newAddress.Neighborhood,
                City = newAddress.City,
                State = newAddress.State,
                Country = newAddress.Country
            };

            var count = await _customerAddressService.InsertOrUpdate(address);
            if (count <= 0)
            {
                ModelState.AddModelError("Error", "Erro ao adicionar novo endereço. Tente novamente.");
                View(model);
            }

            await _customerContext.UpdateShippingAddress(address.Id);
            await _cartService.ClearShippingOptions(cart.Id);

            return RedirectToAction(nameof(ShippingMethod));
        }

        [HttpGet]
        public async Task<IActionResult> ShippingMethod()
        {
            var customer = _customerContext.CurrentCustomer;
            var cart = await _cartService.GetByCustomerId(_customerContext.CurrentCustomer.Id);
            var address = _customerContext.CurrentCustomer.Addresses.FirstOrDefault(x => x.IsShippingAddress);

            if (address == null)
                RedirectToAction(nameof(ShippingAddress));

            await _cartService.CalculateShippingOptions(cart.Id, address.ZipCode);

            var viewmodel = await CreateCheckoutModel();
            return View(viewmodel);
        }

        [HttpPost]
        public async Task<IActionResult> ShippingMethod(string shippingOption)
        {
            var cart = await _cartService.GetByCustomerId(_customerContext.CurrentCustomer.Id);
            await _cartService.UpdateSelectedShippingOption(cart, shippingOption);

            return RedirectToAction(nameof(Confirmation));
        }

        [HttpGet]
        public async Task<IActionResult> Confirmation()
        {
            var model = await CreateCheckoutModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Confirmation(bool confirm)
        {
            if (!confirm)
                return RedirectToAction(nameof(Confirmation));

            var customer = _customerContext.CurrentCustomer;
            var cart = await _cartService.GetByCustomerId(customer.Id);

            var placeOrderResult = await _orderProcessingService.PlaceOrder(customer, cart);

            if (placeOrderResult.Succeeded)
                await _cartService.ClearCart(cart);

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> PaymentMethod()
        {
            var viewmodel = await CreateCheckoutModel();
            return View(viewmodel);
        }

        [HttpPost]
        public async Task<IActionResult> PaymentMethod(string paymentMethod)
        {
            var viewmodel = await CreateCheckoutModel();
            return View(viewmodel);
        }

        private async Task<CheckoutViewModel> CreateCheckoutModel()
        {
            var customer = _customerContext.CurrentCustomer;
            var cart = await _cartService.GetByCustomerId(_customerContext.CurrentCustomer.Id);
            var paymentServices = _paymentServiceResolver.GetActivePaymentMethods();
            var checkoutViewModel = new CheckoutViewModel
            {
                PaymentMethods = new List<PaymentMethodViewModel>(),
                CustomerAddresses = new List<CustomerAddressViewModel>()
            };

            checkoutViewModel.Cart = await _cartViewModelProvider.CreateCartViewModel(cart);

            // Customer addresses
            if (customer.Addresses != null && customer.Addresses.Count > 0)
            {
                foreach (var address in customer.Addresses)
                {
                    var addressViewModel = new CustomerAddressViewModel
                    {
                        Id = address.Id,
                        IsShippingAddress = address.IsShippingAddress,
                        Responsible = address.Responsible,
                        ZipCode = address.ZipCode,
                        Address = address.Address,
                        Number = address.Number,
                        Complement = address.Complement,
                        Neighborhood = address.Neighborhood,
                        City = address.City,
                        State = address.State,
                        Country = address.Country
                    };

                    checkoutViewModel.CustomerAddresses.Add(addressViewModel);
                }
            }

            // Payment methods
            foreach (var p in paymentServices)
            {
                checkoutViewModel.PaymentMethods.Add(new PaymentMethodViewModel
                {
                    Identificator = p.Identificator,
                    Name = p.Name
                });
            }

            return checkoutViewModel;
        }
    }
}
