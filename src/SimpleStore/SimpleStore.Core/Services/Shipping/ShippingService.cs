using SimpleStore.Core.Entities.Carts;
using SimpleStore.Core.Entities.Shipping;
using SimpleStore.Core.Services.Carts;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleStore.Core.Services.Shipping
{
    public interface IShippingService
    {
        Task<ICollection<ShippingOption>> CalculateOptions(Cart cart, string zipcode);
    }

    public class ShippingService : IShippingService
    {
        private readonly IShippingMethodServiceResolver _shippingMethodServiceResolver;

        public ShippingService(IShippingMethodServiceResolver shippingMethodServiceResolver)
        {
            _shippingMethodServiceResolver = shippingMethodServiceResolver;
        }

        public async Task<ICollection<ShippingOption>> CalculateOptions(Cart cart, string zipcode)
        {
            // Get new estimatives
            var shippingMethodService = _shippingMethodServiceResolver.GetByName("MelhorEnvio");
            var options = await shippingMethodService.GetShippingOptions(cart, zipcode);

            return options;
        }
    }
}
