using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleStore.Core.Services.Payments.BankTransfer
{
    public class BankTransferPaymentService : IPaymentService
    {
        public string Identificator => "BankTransfer";
        public string Name => "Transferência Bancária";
        public string ImageUrl => "";
    }
}
