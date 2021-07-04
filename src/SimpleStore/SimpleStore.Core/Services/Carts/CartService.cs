using Microsoft.EntityFrameworkCore;
using SimpleStore.Core.Data;
using SimpleStore.Core.Entities.Carts;
using SimpleStore.Core.Services.Catalog;
using SimpleStore.Framework.Contexts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleStore.Core.Services.Carts
{
    public interface ICartService : IStoreBaseService<Cart>
    {
        Task<Cart> GetByCustomerId(string customerId, bool tracking = false);

        Task AddCartItem(string cartId, string itemId, int quantity);
        Task RemoveCartItem(string cartId, string itemId, int quantity);

        Task UpdateShippingOptions(string cartId, ICollection<CartShippingOption> options);

        Task UpdateSelectedShippingOption(string cartId, string optionId);

        Task ClearShippingOptions(string cartId);
    }

    public class CartService : StoreBaseService<Cart>, ICartService
    {
        private readonly IStoreBaseService<CartItem>  _cartItemService;
        private readonly IStoreBaseService<CartShippingOption> _cartShippingOptionService;

        private readonly ICatalogProductService _catalogItemService;

        public CartService(StoreDbContext context, IStoreContext storeContext, IStoreBaseService<CartItem> cartItemService, ICatalogProductService catalogItemService, IStoreBaseService<CartShippingOption> cartShippingOptionService) : base(context, storeContext)
        {
            _cartItemService = cartItemService;
            _catalogItemService = catalogItemService;
            _cartShippingOptionService = cartShippingOptionService;
        }

        public override async Task<Cart> GetById(string id, bool tracking = false)
        {
            var query = PrepareQuery();
            query = query.Include(x => x.Items.Where(y => !y.Deleted));
            query = query.Include(x => x.ShippingOptions);

            return await query.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Cart> GetByCustomerId(string customerId, bool tracking = false)
        {
            var query = PrepareQuery(tracking);
            query = query.Include(x => x.Items.Where(y => !y.Deleted))
                .ThenInclude(x => x.CatalogItem);
            query = query.Include(x => x.ShippingOptions);

            var cart = await query.FirstOrDefaultAsync(x => x.CustomerId == customerId);

            if (cart != null)
                return cart;

            // If cart is null, create a cart for customer
            cart = new Cart
            {
                CustomerId = customerId
            };

            var count = await Insert(cart);
            if (count <= 0)
                throw new Exception("Cart can't be created.");

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

        public async Task UpdateShippingOptions(string cartId, ICollection<CartShippingOption> options)
        {
            // Get cart
            var cart = await GetById(cartId);

            // Delete current options
            if (cart.ShippingOptions != null && cart.ShippingOptions.Count > 0)
            {
                await _cartShippingOptionService.Delete(cart.ShippingOptions);
            }

            // Save new options
            foreach (var option in options)
            {
                option.CartId = cartId;
            }

            await _cartShippingOptionService.Insert(options);
        }

        public async Task UpdateSelectedShippingOption(string cartId, string optionId)
        {
            // Get cart
            var cart = await GetById(cartId);

            // Get last selected option
            var lastSelectedOption = cart.ShippingOptions?.FirstOrDefault(x => x.Selected);

            // Return if option already selected
            if (lastSelectedOption != null && lastSelectedOption.Id == optionId)
                return;

            // Get current option
            var option = cart.ShippingOptions?.FirstOrDefault(x => x.Id == optionId);

            if (option == null)
                throw new InvalidOperationException("Option doesn't exists.");

            option.Selected = true;

            if (lastSelectedOption != null)
            {
                lastSelectedOption.Selected = false;
                await _cartShippingOptionService.Update(lastSelectedOption);
            }

            // Update database
            await _cartShippingOptionService.Update(option);
        }

        public async Task ClearShippingOptions(string cartId)
        {
            // Get cart
            var cart = await GetById(cartId);

            if (cart.ShippingOptions != null && cart.ShippingOptions.Count > 0)
                await _cartShippingOptionService.Delete(cart.ShippingOptions);
        }
    }
}