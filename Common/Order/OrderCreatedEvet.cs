using Common.Payment;

namespace Events.Order
{
    public class OrderCreatedEvent
    {
        public Guid OrderId { get; set; }
        public string CustomerId { get; set; }
        public PaymentMessage Payment { get; set; }
        public List<OrderItemMessage> orderItems { get; set; } = new List<OrderItemMessage>();
    }
}
