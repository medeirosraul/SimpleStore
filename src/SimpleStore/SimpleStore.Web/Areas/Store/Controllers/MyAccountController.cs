using Microsoft.AspNetCore.Mvc;
using SimpleStore.Core.Entities.Customers;
using SimpleStore.Core.Services.Customers;
using SimpleStore.Framework.Contexts;
using SimpleStore.Web.Areas.Store.ViewModels.MyAccount;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleStore.Web.Areas.Store.Controllers
{
    [Area("Store")]
    [Route("[controller]")]
    public class MyAccountController : Controller
    {
        private readonly IStoreContext _storeContext;
        private readonly ICustomerContext _customerContext;
        private readonly ICustomerService _customerService;
        private readonly ICustomerAddressService _customerAddressService;

        public MyAccountController(IStoreContext storeContext, ICustomerContext customerContext, ICustomerService customerService, ICustomerAddressService customerAddressService)
        {
            _storeContext = storeContext;
            _customerContext = customerContext;
            _customerService = customerService;
            _customerAddressService = customerAddressService;
        }

        public async Task<IActionResult> Index()
        {
            var customer = _customerContext.CurrentCustomer;

            var customerViewModel = new CustomerViewModel
            {
                Name = customer.Name,
                PhoneNumber = customer.PhoneNumber,
                DocumentNumber = customer.DocumentNumber
            };

            if (customer.Addresses != null && customer.Addresses.Count > 0)
            {
                customerViewModel.Addresses = new List<CustomerAddressViewModel>();
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

                    customerViewModel.Addresses.Add(addressViewModel);
                }
            }

            return View(customerViewModel);
        }

        [HttpPost("Customer")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PostCustommer([FromForm]CustomerViewModel customerViewModel)
        {
            var customer = _customerContext.CurrentCustomer;

            customer.Addresses = null;
            customer.Cart = null;
            customer.Name = customerViewModel.Name;
            customer.PhoneNumber = customerViewModel.PhoneNumber;
            customer.DocumentNumber = customerViewModel.DocumentNumber;

            await _customerService.Update(customer);
            await _customerContext.SetCurrentCustomer();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet("Address/Edit/{id?}")]
        public async Task<IActionResult> AddressEdit(string id)
        {
            var address = await _customerAddressService.GetById(id);

            if (address == null) 
                return View();

            if (address.CustomerId != _customerContext.CurrentCustomer.Id)
                return View();

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

            return View(addressViewModel);
        }

        [HttpPost("Address/Edit/{id?}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddressEdit(string id, [FromForm] CustomerAddressViewModel addressViewModel)
        {
            CustomerAddress address = null;

            if (!string.IsNullOrEmpty(addressViewModel.Id))
            {
                address = await _customerAddressService.GetById(id);

                if (address.CustomerId != _customerContext.CurrentCustomer.Id)
                    address = null;
            }

            address ??= new CustomerAddress 
            {
                CustomerId = _customerContext.CurrentCustomer.Id
            };

            address.Responsible = addressViewModel.Responsible;
            address.ZipCode = addressViewModel.ZipCode;
            address.Address = addressViewModel.Address;
            address.Number = addressViewModel.Number;
            address.Complement = addressViewModel.Complement;
            address.Neighborhood = addressViewModel.Neighborhood;
            address.City = addressViewModel.City;
            address.State = addressViewModel.State;
            address.Country = addressViewModel.Country;

            await _customerAddressService.InsertOrUpdate(address);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet("Address/Remove/{id}")]
        public async Task<IActionResult> AddressRemoveConfirmation(string id)
        {
            var address = await _customerAddressService.GetById(id);

            if (address == null)
                return RedirectToAction(nameof(Index));

            if (address.CustomerId != _customerContext.CurrentCustomer.Id)
                return RedirectToAction(nameof(Index));

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

            return View(addressViewModel);
        }

        [HttpPost("Address/Remove")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddressRemove([FromForm] string id)
        {
            var address = await _customerAddressService.GetById(id);

            if (address == null)
                return RedirectToAction(nameof(Index));

            if (address.CustomerId != _customerContext.CurrentCustomer.Id)
                return RedirectToAction(nameof(Index));

            await _customerAddressService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}