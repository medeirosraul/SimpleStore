using SimpleStore.Core.Entities.Carts;
using SimpleStore.Core.Services.Prices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleStore.Core.Services.Orders
{
    public interface IOrderCalculationService
    {
        Task<decimal> GetSubtotal(Cart cart);
        Task<decimal> GetSubtotal(ICollection<CartItem> items);
    }

    public class OrderCalculationService : IOrderCalculationService
    {
        private IPriceProvider _priceProvider;

        public OrderCalculationService(IPriceProvider priceProvider)
        {
            _priceProvider = priceProvider;
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
                var price = await _priceProvider.GetCatalogItemPrice(item.CatalogItem);

                subtotal += price.Value * item.Quantity;
            }

            return subtotal;
        }
    }
}