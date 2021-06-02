using SimpleStore.Core.Data;
using SimpleStore.Core.Entities.Customers;
using SimpleStore.Framework.Contexts;

namespace SimpleStore.Core.Services.Customers
{
    public interface ICustomerAddressService : IStoreBaseService<CustomerAddress>
    {

    }

    public class CustomerAddressService : StoreBaseService<CustomerAddress>, ICustomerAddressService
    {
        public CustomerAddressService(
            StoreDbContext context, 
            IStoreContext storeContext) : base(context, storeContext)
        {

        }
    }
}