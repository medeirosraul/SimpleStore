using SimpleStore.Core.Entities.Carts;
using SimpleStore.Core.Entities.Orders;
using SimpleStore.Core.Entities.Shipping;
using SimpleStore.Framework.Contexts;

namespace SimpleStore.Core.Services.Shipping
{
    public interface IShippingService
    {
        Task<ICollection<ShippingOption>> CalculateOptions(Cart cart, string zipcode);
        Task<ICollection<Shipment>> CreateShipments(Order order);
    }

    public class ShippingService : IShippingService
    {
        private readonly ICustomerContext _customerContext;
        private readonly IShippingMethodServiceResolver _shippingMethodServiceResolver;
        private readonly IStoreBaseService<Shipment> _shipmentService;

        public ShippingService(ICustomerContext customerContext, IShippingMethodServiceResolver shippingMethodServiceResolver, IStoreBaseService<Shipment> shipmentService)
        {
            _customerContext = customerContext;
            _shippingMethodServiceResolver = shippingMethodServiceResolver;
            _shipmentService = shipmentService;
        }

        public async Task<ICollection<ShippingOption>> CalculateOptions(Cart cart, string zipcode)
        {
            // Get new estimatives
            var shippingMethodService = _shippingMethodServiceResolver.GetByName("MelhorEnvio");
            var options = await shippingMethodService.GetShippingOptions(cart, zipcode);

            return options;
        }

        public async Task<ICollection<Shipment>> CreateShipments(Order order)
        {
            var customer = _customerContext.CurrentCustomer;
            var address = customer.Addresses.FirstOrDefault(x => x.IsShippingAddress);
            var shippingMethod = order.ShippingMethod;

            if (address == null)
                throw new Exception("Nenhum endereço selecionado para entrega.");

            if (string.IsNullOrEmpty(shippingMethod))
                throw new Exception("Nenhuma forma de envio selecionada.");

            var shipment = new Shipment
            {
                OrderId = order.Id,
                ShippingMethod = shippingMethod,
                Responsible = address.Responsible,
                ZipCode = address.ZipCode,
                Address = address.Address,
                Number = address.Number,
                Complement = address.Complement,
                Neighborhood = address.Neighborhood,
                City = address.City,
                State = address.State,
                Country = address.Country,
                Status = ShipmentStatus.Pending,
                Items = new List<ShipmentItem>()
            };

            foreach (var item in order.ProductItems)
            {
                var shipmentItem = new ShipmentItem
                {
                    OrderProductItemId = item.Id
                };

                shipment.Items.Add(shipmentItem);
            }

            await _shipmentService.Insert(shipment);

            return new List<Shipment>()
            {
                shipment
            };
        }
    }
}
