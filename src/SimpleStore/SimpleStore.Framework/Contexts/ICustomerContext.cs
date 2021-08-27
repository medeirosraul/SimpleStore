using SimpleStore.Core.Entities.Customers;

namespace SimpleStore.Framework.Contexts
{
    public interface ICustomerContext
    {
        public Customer CurrentCustomer { get; }

        public Task SetCurrentCustomer();

        public Task UpdateShippingAddress(string addressId);

        public Task UpdatePaymentMethod(string paymentMethodIdentificator);
    }
}
