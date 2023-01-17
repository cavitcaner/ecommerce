using Common.Order;

namespace Common.Payment
{
    public class PaymentSuccessEvent
    {
        public Guid OrderId { get; set; }
        public string CustomerEmail { get; set; }
        public List<OrderItemMessage> OrderItems { get; set; }
    }
}
