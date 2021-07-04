using SimpleStore.Core.Entities.Carts;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleStore.Core.Services.Orders
{
    public interface IOrderCalculationService
    {
        Task<decimal> GetSubtotal(Cart cart);
        Task<decimal> GetSubtotal(ICollection<CartItem> items);
        Task<decimal> GetShippingValue(Cart cart);
        Task<decimal> GetTotal(Cart cart);
    }

    public class OrderCalculationService : IOrderCalculationService
    {
        public OrderCalculationService()
        {
            
        }

        public async Task<decimal> GetSubtotal(Cart cart)
        {
            return await GetSubtotal(cart.Items);
        }

        public async Task<decimal> GetSubtotal(ICollection<CartItem> items)
        {
            var subtotal = 0m;

            foreach (var item in items)
            {
                subtotal += item.CatalogItem.Price * item.Quantity;
            }

            return subtotal;
        }

        public async Task<decimal> GetShippingValue(Cart cart)
        {
            var shipping = cart.ShippingOptions.FirstOrDefault(x => x.Selected);
            if (shipping == null) return 0m;

            return shipping.Value;
        }

        public async Task<decimal> GetTotal(Cart cart)
        {
            var subtotal = await GetSubtotal(cart);
            var shippingTax = await GetShippingValue(cart);

            return subtotal + shippingTax;
        }
    }
}