using SimpleStore.Core.Data;
using SimpleStore.Core.Entities.Carts;
using SimpleStore.Core.Entities.Customers;
using SimpleStore.Core.Entities.Orders;
using SimpleStore.Core.Services.Catalog;
using SimpleStore.Core.Services.Shipping;

namespace SimpleStore.Core.Services.Orders
{
    public interface IOrderProcessingService
    {
        Task<PlaceOrderResult> PlaceOrder(Customer customer, Cart cart);
    }

    public class OrderProcessingService : IOrderProcessingService
    {
        private readonly StoreDbContext _context;
        private readonly IOrderService _orderService;
        private readonly IOrderCalculationService _orderCalculationService;
        private readonly ICatalogProductService _catalogProductService;
        private readonly IShippingService _shippingService;

        public OrderProcessingService(
            StoreDbContext context,
            IOrderService orderService,
            IOrderCalculationService
            orderCalculationService,
            ICatalogProductService catalogProductService,
            IShippingService shippingService)
        {
            _context = context;
            _orderService = orderService;
            _orderCalculationService = orderCalculationService;
            _catalogProductService = catalogProductService;
            _shippingService = shippingService;
        }

        public async Task<PlaceOrderResult> PlaceOrder(Customer customer, Cart cart)
        {
            var result = new PlaceOrderResult
            {
                Succeeded = true
            };

            var shippingOption = cart.ShippingOptions.FirstOrDefault(x => x.Selected);

            if (shippingOption == null)
                throw new Exception("Nenhum método de envio selecionado.");

            var order = new Order
            {
                CustomerId = customer.Id,
                Status = OrderStatus.Pending,
                ProductItems = new List<OrderProductItem>(),
                ShippingMethod = shippingOption.Name
            };

            // Add items to order
            foreach (var item in cart.Items)
            {
                var orderItem = new OrderProductItem
                {
                    CatalogProductId = item.CatalogItemId,
                    CatalogProduct = item.CatalogItem,
                    Price = item.CatalogItem.Price,
                    Quantity = item.Quantity
                };

                order.ProductItems.Add(orderItem);
            }

            // Calculate order values
            order.ShippingValue = await _orderCalculationService.GetShippingValue(cart);
            order.Subtotal = await _orderCalculationService.GetSubtotal(cart);
            order.Total = await _orderCalculationService.GetTotal(cart);

            // Begin database transaction
            await _context.BeginTransaction();

            // Save Order
            await _orderService.Insert(order);

            // Create shipment
            var shipments = await _shippingService.CreateShipments(order);
            order.Shipments = shipments;

            // Update Product stock quantity
            foreach (var item in cart.Items)
            {
                await _catalogProductService.UpdateProductStock(item.CatalogItem, item.Quantity * -1);
            }

            // Commit changes
            try
            {
                await _context.Commit();
            }
            catch (Exception ex)
            {
                await _context.Rollback();

                result.Succeeded = false;
                result.Message = "Não foi possível criar o pedido.";
                result.Errors ??= new List<string>();
                result.Errors.Add(ex.Message);
            }

            return result;
        }
    }
}