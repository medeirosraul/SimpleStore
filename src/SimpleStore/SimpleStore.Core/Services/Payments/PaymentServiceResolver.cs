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
        IPaymentMethod GetByIdentificator(string identificator);

        IEnumerable<IPaymentMethod> GetActivePaymentMethods();
    }

    public class PaymentServiceResolver : IPaymentServiceResolver
    {
        private readonly IServiceProvider _serviceProvider;

        public PaymentServiceResolver(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IPaymentMethod GetByIdentificator(string identificator)
        {
            return _serviceProvider.GetServices<IPaymentMethod>()?.SingleOrDefault(x => x.Identificator == identificator);
        }

        public IEnumerable<IPaymentMethod> GetActivePaymentMethods()
        {
            return _serviceProvider.GetServices<IPaymentMethod>();
        }
    }
}
