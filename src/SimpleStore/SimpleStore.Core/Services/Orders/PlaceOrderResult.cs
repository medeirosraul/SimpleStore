using SimpleStore.Core.Entities.Orders;

namespace SimpleStore.Core.Services.Orders
{
    public class PlaceOrderResult
    {
        public bool Succeeded { get; set; }
        public string Message { get; set; }
        public ICollection<string> Errors { get; set; }
        public Order Order { get; set; }
    }
}
