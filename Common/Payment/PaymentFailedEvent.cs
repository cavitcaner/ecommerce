using Common.Order;
using Common.Order.Dto;

namespace Common.Payment
{
    public class PaymentFailedEvent
    {
        public Guid OrderId { get; set; }
        public List<OrderItemMessage> OrderItems { get; set; }
        public string Message { get; set; }
    }
}
