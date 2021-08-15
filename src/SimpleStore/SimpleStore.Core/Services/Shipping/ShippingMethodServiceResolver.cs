using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleStore.Core.Services.Shipping
{
    public interface IShippingMethodServiceResolver
    {
        IShippingMethodService GetByName(string name);
    }

    public class ShippingMethodServiceResolver : IShippingMethodServiceResolver
    {
        private readonly IServiceProvider _serviceProvider;

        public ShippingMethodServiceResolver(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IShippingMethodService GetByName(string name)
        {
            return _serviceProvider.GetServices<IShippingMethodService>()?.SingleOrDefault(x => x.Name == name);
        }
    }
}
