using SimpleStore.Core.Services.Payments;
using SimpleStore.Web.Areas.Store.ViewModels.Cart;
using SimpleStore.Web.Areas.Store.ViewModels.MyAccount;
using SimpleStore.Web.Areas.Store.ViewModels.Payment;
using System.Collections.Generic;

namespace SimpleStore.Web.Areas.Store.ViewModels.Checkout
{
    public class CheckoutViewModel
    {
        public CartViewModel Cart { get; set; }

        public CustomerAddressViewModel NewAddress { get; set; }
        public ICollection<PaymentMethodViewModel> PaymentMethods{ get; set; }
        public ICollection<CustomerAddressViewModel> CustomerAddresses { get; set; }
    }
}