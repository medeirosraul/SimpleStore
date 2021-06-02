using Microsoft.EntityFrameworkCore;
using SimpleStore.Core.Data;
using SimpleStore.Core.Entities.Carts;
using SimpleStore.Core.Services.Products;
using SimpleStore.Framework.Contexts;
using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace SimpleStore.Core.Services.Carts
{
    public interface ICartService : IStoreBaseService<Cart>
    {
        Task<Cart> GetByCustomerId(string customerId, bool tracking = false);
        Task AddCartItem(string cartId, string itemId, int quantity);
        Task RemoveCartItem(string cartId, string itemId, int quantity);
    }

    public class CartService : StoreBaseService<Cart>, ICartService
    {
        private readonly ICartItemService _cartItemService;
        private readonly ICatalogItemService _catalogItemService;

        public CartService(StoreDbContext context, IStoreContext storeContext, ICartItemService cartItemService, ICatalogItemService catalogItemService) : base(context, storeContext)
        {
            _cartItemService = cartItemService;
            _catalogItemService = catalogItemService;
        }

        public override async Task<Cart> GetById(string id, bool tracking = false)
        {
            var query = PrepareQuery();
            query = query.Include(x => x.Items.Where(y => !y.Deleted));

            return await query.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Cart> GetByCustomerId(string customerId, bool tracking = false)
        {
            var query = PrepareQuery(tracking);
            query = query.Include(x => x.Items.Where(y => !y.Deleted)).ThenInclude(x => x.CatalogItem);

            var cart = await query.FirstOrDefaultAsync(x => x.CustomerId == customerId);

            if (cart != null)
                return cart;

            // If cart is null, create cart for customer
            cart = new Cart
            {
                CustomerId = customerId
            };

            var count = await Insert(cart);
            if (count <= 0)
                throw new InvalidOperationException("Cart can't be created.");

            return cart;
        }

        public async Task AddCartItem(string cartId, string itemId, int quantity)
        {
            // Get cart
            var cart = await GetById(cartId);

            // Get catalog item
            var item = await _catalogItemService.GetById(itemId);

            // Verify if item exists in cart
            var cartItem = cart.Items?.FirstOrDefault(x => x.CatalogItemId == itemId);

            // If doesn't exists, create one
            if (cartItem == null)
            {
                cartItem ??= new CartItem
                {
                    CartId = cart.Id,
                    CatalogItemId = item.Id,
                    Quantity = quantity
                };
            }
            else
            {
                cartItem.Quantity += quantity;
            }

            // Insert or Update Cart Item
            await _cartItemService.InsertOrUpdate(cartItem);
        }

        public async Task RemoveCartItem(string cartId, string itemId, int quantity)
        {
            // Get Cart
            var cart = await GetById(cartId);

            // Get Cart Item
            var item = cart.Items?.FirstOrDefault(x => x.CatalogItemId == itemId);

            // If doesn't exists, return
            if (item == null)
                return;

            // Set item as deleted and update
            item.Deleted = true;

            await _cartItemService.Update(item);
        }
    }
}
