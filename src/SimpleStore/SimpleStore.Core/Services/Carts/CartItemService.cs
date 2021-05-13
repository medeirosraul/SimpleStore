using SimpleStore.Core.Data;
using SimpleStore.Core.Entities.Carts;
using SimpleStore.Framework.Contexts;

namespace SimpleStore.Core.Services.Carts
{
    public interface ICartItemService : IStoreBaseService<CartItem>
    {

    }

    public class CartItemService : StoreBaseService<CartItem>, ICartItemService
    {
        public CartItemService(StoreDbContext context, IStoreContext storeContext) : base(context, storeContext)
        {

        }
    }
}