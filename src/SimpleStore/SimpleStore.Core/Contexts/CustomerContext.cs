using Microsoft.AspNetCore.Http;
using Serilog;
using SimpleStore.Core.Entities.Customers;
using SimpleStore.Core.Services.Customers;
using SimpleStore.Framework.Contexts;
using System.Security.Claims;

namespace SimpleStore.Core.Contexts
{
    public class CustomerContext : ICustomerContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IStoreContext _storeContext;
        private readonly ICustomerService _customerService;

        private Customer _currentCustomer;
        public Customer CurrentCustomer => _currentCustomer;

        public CustomerContext(IHttpContextAccessor httpContextAccessor, IStoreContext storeContext, ICustomerService customerService)
        {
            _httpContextAccessor = httpContextAccessor;
            _storeContext = storeContext;
            _customerService = customerService;

            Task.Run(async () => await SetCurrentCustomer()).Wait();
        }

        public async Task SetCurrentCustomer()
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var customerId = _httpContextAccessor.HttpContext.Request.Cookies[$".SimpleStore.Customer"];

            // Verify if user is logged in
            if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                // Verify if logged user contains a customer entity associated
                var loadedCustomer = await _customerService.GetByUser(userId);
                if (loadedCustomer != null)
                {
                    _currentCustomer = loadedCustomer;
                    return;
                }

                // If user does not contains a customer associated, verify cookie
                if (!string.IsNullOrEmpty(customerId))
                {
                    loadedCustomer = await _customerService.GetById(customerId);
                    if (loadedCustomer != null && string.IsNullOrEmpty(loadedCustomer.UserId))
                    {
                        loadedCustomer.UserId = userId;
                        await _customerService.Update(loadedCustomer);
                        _currentCustomer = loadedCustomer;
                        return;
                    }
                    else
                    {
                        customerId = string.Empty;
                    }
                }

                // Return
            }

            // If customer cookie exists, get customer by Id
            if (!string.IsNullOrWhiteSpace(customerId))
            {
                var loadedCustomer = await _customerService.GetById(customerId);
                if (loadedCustomer != null && string.IsNullOrWhiteSpace(loadedCustomer.UserId))
                {
                    _currentCustomer = loadedCustomer;
                    return;
                }
            }

            // If customer cookie doesn't exists, create a customer and a cookie with its Id
            var customer = new Customer();
            var count = await _customerService.Insert(customer);

            if (count <= 0)
            {
                Log.Error("Current Customer can't be created.");
                return;
            }

            var cookieOptions = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(30)
            };

            _httpContextAccessor.HttpContext.Response.Cookies.Append($".SimpleStore.Customer", customer.Id, cookieOptions);
        }

        public Task UpdateShippingAddress(string addressId)
        {
            return _customerService.UpdateShippingAddress(CurrentCustomer, addressId);
        }

        public Task UpdatePaymentMethod(string paymentMethodIdentificator)
        {
            throw new NotImplementedException();
        }
    }
}