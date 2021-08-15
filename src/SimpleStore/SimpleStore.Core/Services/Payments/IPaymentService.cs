using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleStore.Core.Services.Payments
{
    public interface IPaymentService
    {
        string Identificator { get; }

        string Name { get; }

        string ImageUrl { get; }
    }
}
