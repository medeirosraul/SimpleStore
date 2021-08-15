using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleStore.Core.Services.Payments.BankTransfer
{
    public class MercadoPagoPaymentService : IPaymentService
    {
        public string Identificator => "MercadoPago";
        public string Name => "Mercado Pago";
        public string ImageUrl => "";
    }
}
