using SimpleStore.Core.Entities.Carts;
using SimpleStore.Core.Entities.Shipping;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleStore.Core.Services.Shipping
{
    public interface IShippingMethodService
    {
        string Name { get; }

        Task<ICollection<ShippingOption>> GetShippingOptions(Cart cart, string zipcode);
    }
}
