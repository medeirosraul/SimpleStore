using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleStore.Web.Areas.Store.ViewModels.Payment
{
    public class PaymentMethodViewModel
    {
        public string Identificator { get; set; }
        public string Name { get; set; }
        public bool Selected { get; set; }
    }
}
