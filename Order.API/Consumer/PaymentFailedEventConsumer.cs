using Common.Order;
using Common.Payment;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Order.API.Database;

namespace Order.API.Consumer
{
    public class PaymentFailedEventConsumer : IConsumer<PaymentFailedEvent>
    {
        private readonly OrderDbContext _context;

        public PaymentFailedEventConsumer(OrderDbContext orderDbContext)
        {
            _context = orderDbContext;
        }

        public async Task Consume(ConsumeContext<PaymentFailedEvent> context)
        {
            var order = await _context.Orders.FirstAsync(x => x.Id == context.Message.OrderId);

            order.Status = OrderStatus.Fail;
            order.Message = context.Message.Message;

            _context.Update(order);
            await _context.SaveChangesAsync();
        }
    }
}
