using Microsoft.AspNetCore.Mvc;
using SimpleStore.Core.Entities.Customers;
using SimpleStore.Core.Services.Carts;
using SimpleStore.Core.Services.Customers;
using SimpleStore.Core.Services.Payments;
using SimpleStore.Core.Services.Shipping;
using SimpleStore.Framework.Contexts;
using SimpleStore.Web.Areas.Store.ViewModelProviders;
using SimpleStore.Web.Areas.Store.ViewModels.Checkout;
using SimpleStore.Web.Areas.Store.ViewModels.MyAccount;
using SimpleStore.Web.Areas.Store.ViewModels.Payment;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public CheckoutController(
            ICartViewModelProvider cartViewModelProvider,
            ICustomerContext customerContext,
            ICartService cartService,
            IPaymentServiceResolver paymentServiceResolver,
            IShippingService shippingService, 
            ICustomerAddressService customerAddressService)
        {
            _cartViewModelProvider = cartViewModelProvider;
            _customerContext = customerContext;
            _cartService = cartService;
            _paymentServiceResolver = paymentServiceResolver;
            _shippingService = shippingService;
            _customerAddressService = customerAddressService;
        }

        public IActionResult Index()
        {
            // Redirect to First Step
            return RedirectToAction(nameof(ShippingAddress));
        }

        [HttpGet]
        public async Task<IActionResult> ShippingAddress()
        {
            var viewmodel = await CreateCheckoutViewModel();
            return View(viewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ShippingAddress([FromForm] string addressId, [FromForm] CustomerAddressViewModel newAddress)
        {
            var customer = _customerContext.CurrentCustomer;
            var cart = await _cartService.GetByCustomerId(_customerContext.CurrentCustomer.Id);
            var viewModel = await CreateCheckoutViewModel();

            viewModel.NewAddress = newAddress;

            // If existent address
            if (!string.IsNullOrEmpty(addressId))
            {
                var existingAddress = customer.Addresses.FirstOrDefault(x => x.Id == addressId);

                if (existingAddress == null)
                {
                    ModelState.AddModelError("InvalidAddressError", "O endereço selecionado é inválido. Selecione outro endereço ou cadastre um novo.");
                    return RedirectToAction(nameof(ShippingAddress));
                }

                await _cartService.UpdateSelectedShippingAddress(cart.Id, existingAddress.Id);
                return RedirectToAction(nameof(ShippingMethod));
            }

            // If new address
            if (!ModelState.IsValid)
                return View(viewModel);

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
                View();
            }

            await _cartService.UpdateSelectedShippingAddress(cart.Id, address.Id);

            return RedirectToAction(nameof(ShippingMethod));
        }

        [HttpGet]
        public async Task<IActionResult> ShippingMethod()
        {
            var viewmodel = await CreateCheckoutViewModel();
            return View(viewmodel);
        }

        [HttpPost]
        public async Task<IActionResult> ShippingMethod(string shippingOption)
        {
            var cart = await _cartService.GetByCustomerId(_customerContext.CurrentCustomer.Id);
            await _cartService.UpdateSelectedShippingOption(cart, shippingOption);

            return RedirectToAction(nameof(PaymentMethod));
        }

        [HttpGet]
        public async Task<IActionResult> PaymentMethod()
        {
            var viewmodel = await CreateCheckoutViewModel();
            return View(viewmodel);
        }

        [HttpPost]
        public async Task<IActionResult> PaymentMethod(string paymentMethod)
        {
            var viewmodel = await CreateCheckoutViewModel();
            return View(viewmodel);
        }

        private async Task<CheckoutViewModel> CreateCheckoutViewModel()
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
