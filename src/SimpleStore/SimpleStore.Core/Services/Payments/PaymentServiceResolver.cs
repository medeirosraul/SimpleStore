using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleStore.Core.Services.Payments
{
    public interface IPaymentServiceResolver
    {
        IPaymentService GetByIdentificator(string identificator);

        IEnumerable<IPaymentService> GetActivePaymentMethods();
    }

    public class PaymentServiceResolver : IPaymentServiceResolver
    {
        private readonly IServiceProvider _serviceProvider;

        public PaymentServiceResolver(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IPaymentService GetByIdentificator(string identificator)
        {
            return _serviceProvider.GetServices<IPaymentService>()?.SingleOrDefault(x => x.Identificator == identificator);
        }

        public IEnumerable<IPaymentService> GetActivePaymentMethods()
        {
            return _serviceProvider.GetServices<IPaymentService>();
        }
    }
}
