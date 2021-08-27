using Microsoft.EntityFrameworkCore;
using SimpleStore.Core.Data;
using SimpleStore.Core.Entities.Orders;
using SimpleStore.Framework.Contexts;

namespace SimpleStore.Core.Services.Orders
{
    public interface IOrderService : IStoreBaseService<Order>
    {
        Task<Order> GetById(string id, bool includeProductItems = false, bool includePayments = false, bool includeShipments = false, bool tracking = false);
    }

    public class OrderService : StoreBaseService<Order>, IOrderService
    {
        public OrderService(StoreDbContext context, IStoreContext storeContext) : base(context, storeContext)
        {

        }

        public Task<Order> GetById(string id, bool includeProductItems = false, bool includePayments = false, bool includeShipments = false, bool tracking = false)
        {
            var query = PrepareQuery();

            if (includeProductItems)
                query = query.Include(x => x.ProductItems)
                    .ThenInclude(x => x.CatalogProduct);

            if (includePayments)
                query = query.Include(x => x.Payments);

            if (includeShipments)
                query = query.Include(x => x.Shipments)
                    .ThenInclude(x => x.Items);

            return query.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
