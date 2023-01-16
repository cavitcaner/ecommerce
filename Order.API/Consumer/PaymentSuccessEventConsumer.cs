using Common.Order;
using Common.Payment;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Order.API.Database;

namespace Order.API.Consumer
{
    public class PaymentSuccessEventConsumer : IConsumer<PaymentSuccessEvent>
    {
        private readonly OrderDbContext _context;

        public PaymentSuccessEventConsumer(OrderDbContext orderDbContext)
        {
            _context = orderDbContext;
        }

        public async Task Consume(ConsumeContext<PaymentSuccessEvent> context)
        {
            var order = await _context.Orders.FirstAsync(x => x.Id == context.Message.OrderId);

            order.Status = OrderStatus.Success;
            order.Message = "Sipariş başarılı bir şekilde oluşturulmuştur.";

            _context.Update(order);
            await _context.SaveChangesAsync();
        }
    }
}
