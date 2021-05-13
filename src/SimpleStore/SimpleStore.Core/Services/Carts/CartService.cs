using SimpleStore.Core.Data;
using SimpleStore.Core.Entities.Carts;
using SimpleStore.Framework.Contexts;

namespace SimpleStore.Core.Services.Carts
{
    public interface ICartService : IStoreBaseService<Cart>
    {

    }

    public class CartService : StoreBaseService<Cart>, ICartService
    {
        private readonly ICartItemService _cartItemService;

        public CartService(StoreDbContext context, IStoreContext storeContext, ICartItemService cartItemService) : base(context, storeContext)
        {
            _cartItemService = cartItemService;
        }
    }
}
