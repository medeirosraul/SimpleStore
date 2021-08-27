using Microsoft.EntityFrameworkCore;
using SimpleStore.Core.Data;
using SimpleStore.Core.Entities.Customers;
using SimpleStore.Framework.Contexts;

namespace SimpleStore.Core.Services.Customers
{
    public interface ICustomerService : IStoreBaseService<Customer>
    {
        Task<Customer> GetByUser(string userId);
        Task UpdateShippingAddress(Customer customer, string addressId);
        Task UpdateSelectedPaymentMethod(Customer customer, string paymentMethod);
    }

    public class CustomerService : StoreBaseService<Customer>, ICustomerService
    {
        private readonly IStoreBaseService<CustomerAddress> _customerAddressService;

        public CustomerService(
            StoreDbContext context,
            IStoreContext storeContext, IStoreBaseService<CustomerAddress> customerAddressService) : base(context, storeContext)
        {
            _customerAddressService = customerAddressService;
        }

        public async Task<Customer> GetByUser(string userId)
        {
            var query = PrepareQuery();
            query = query.Include(x => x.Addresses.Where(address => !address.Deleted));
            query = query.Where(p => p.UserId == userId);

            return await query.FirstOrDefaultAsync();
        }

        public async Task UpdateShippingAddress(Customer customer, string addressId)
        {
            foreach (var address in customer.Addresses)
            {
                address.IsShippingAddress = false;

                if (address.Id == addressId)
                    address.IsShippingAddress = true;
            }

            await _customerAddressService.Update(customer.Addresses);
        }

        public async Task UpdateSelectedPaymentMethod(Customer customer, string paymentMethod)
        {
            customer.SelectedPaymentMethod = paymentMethod;
            await Update(customer);
        }
    }
}