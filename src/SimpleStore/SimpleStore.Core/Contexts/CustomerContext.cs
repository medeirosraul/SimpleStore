using SimpleStore.Core.Entities.Customers;
using SimpleStore.Framework.Contexts;

namespace SimpleStore.Core.Contexts
{
    public class CustomerContext : ICustomerContext
    {
        public Customer CurrentCustomer { get; set; }

        public CustomerContext()
        {

        }
    }
}