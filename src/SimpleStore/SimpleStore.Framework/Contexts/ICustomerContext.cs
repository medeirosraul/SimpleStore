using SimpleStore.Core.Entities.Customers;
using SimpleStore.Core.Entities.Stores;
using System.Threading.Tasks;

namespace SimpleStore.Framework.Contexts
{
    public interface ICustomerContext
    {
        public Customer CurrentCustomer { get; }

        public Task SetCurrentCustomer();
    }
}
