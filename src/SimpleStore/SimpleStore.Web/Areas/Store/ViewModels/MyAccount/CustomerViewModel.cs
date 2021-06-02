using Microsoft.AspNetCore.Hosting.Server.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleStore.Web.Areas.Store.ViewModels.MyAccount
{
    public class CustomerViewModel
    {
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string DocumentNumber { get; set; }

        public ICollection<CustomerAddressViewModel> Addresses { get; set; }
    }
}
