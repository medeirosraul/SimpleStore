using Microsoft.EntityFrameworkCore;
using SimpleStore.Core.Data;
using SimpleStore.Core.Entities.Carts;
using SimpleStore.Core.Entities.Shipping;
using SimpleStore.Core.Services.Catalog;
using SimpleStore.Core.Services.Shipping;
using SimpleStore.Framework.Contexts;

namespace SimpleStore.Core.Services.Carts
{
    public interface ICartService : IStoreBaseService<Cart>
    {
        Task<Cart> GetByCustomerId(string customerId, bool tracking = false);

        Task AddCartItem(Cart cart, string itemId, int quantity);

        Task RemoveCartItem(Cart cart, string itemId, int quantity);

        Task ClearCart(Cart cart);

        Task<ICollection<ShippingOption>> CalculateShippingOptions(string cartId, string zipcode);

        Task UpdateShippingOptions(string cartId, ICollection<CartShippingOption> options);

        Task UpdateSelectedShippingOption(Cart cart, string optionId);

        Task UpdateSelectedPaymentMethod(Cart cart, string identificator);

        Task ClearShippingOptions(string cartId);
    }

    public class CartService : StoreBaseService<Cart>, ICartService
    {
        private readonly ICustomerContext _customerContext;
        private readonly IStoreBaseService<CartItem> _cartItemService;
        private readonly IStoreBaseService<CartShippingOption> _cartShippingOptionService;

        private readonly ICatalogProductService _catalogItemService;
        private readonly IShippingService _shippingService;

        public CartService(StoreDbContext context,
            IStoreContext storeContext,
            IStoreBaseService<CartItem> cartItemService,
            ICatalogProductService catalogItemService,
            IStoreBaseService<CartShippingOption> cartShippingOptionService,
            IShippingService shippingService,
            ICustomerContext customerContext) : base(context, storeContext)
        {
            _cartItemService = cartItemService;
            _catalogItemService = catalogItemService;
            _cartShippingOptionService = cartShippingOptionService;
            _shippingService = shippingService;
            _customerContext = customerContext;
        }

        public override async Task<Cart> GetById(string id, bool tracking = false)
        {
            var query = PrepareQuery();
            query = query.Include(x => x.Items.Where(y => !y.Deleted))
                .ThenInclude(x => x.CatalogItem);
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

        public async Task AddCartItem(Cart cart, string itemId, int quantity)
        {
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
            cart.Items ??= new List<CartItem>();

            cart.Items.Add(cartItem);
        }

        public async Task RemoveCartItem(Cart cart, string itemId, int quantity)
        {
            // Get Cart Item
            var item = cart.Items?.FirstOrDefault(x => x.CatalogItemId == itemId);

            // If doesn't exist, return
            if (item == null)
                return;

            // Set item as deleted and update
            item.Deleted = true;

            await _cartItemService.Update(item);

            cart.Items.Remove(item);
        }

        public async Task ClearCart(Cart cart)
        {
            await _cartItemService.Delete(cart.Items);
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

        public async Task UpdateSelectedShippingOption(Cart cart, string optionId)
        {
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

        public async Task<ICollection<ShippingOption>> CalculateShippingOptions(string cartId, string zipcode)
        {
            // Get cart
            var cart = await GetById(cartId);
            cart.ShippingZipCode = zipcode;

            await Update(cart);
            await ClearShippingOptions(cart.Id);

            var options = await _shippingService.CalculateOptions(cart, zipcode);

            if (options != null && options.Count > 0)
            {
                var cartShippingOptions = new List<CartShippingOption>();
                foreach (var option in options)
                {
                    cartShippingOptions.Add(new CartShippingOption
                    {
                        Name = option.Name,
                        Description = option.Description,
                        Value = option.Value,
                        Method = "MelhorEnvio"
                    });
                }

                // Pre-select minor value
                cartShippingOptions.First().Selected = true;
                await UpdateShippingOptions(cart.Id, cartShippingOptions);
            }

            return options;
        }

        public Task UpdateSelectedPaymentMethod(Cart cart, string identificator)
        {
            throw new NotImplementedException();
        }


    }
}