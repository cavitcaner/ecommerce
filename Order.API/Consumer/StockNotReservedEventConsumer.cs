using Common.Order;
using Common.Stock;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Order.API.Database;

namespace Order.API.Consumer
{
    public class StockNotReservedEventConsumer : IConsumer<StockNotReservedEvent>
    {
        private readonly OrderDbContext _context;

        public StockNotReservedEventConsumer(OrderDbContext orderDbContext)
        {
            _context = orderDbContext;
        }

        public async Task Consume(ConsumeContext<StockNotReservedEvent> context)
        {
            var order = await _context.Orders.FirstAsync(x => x.Id == context.Message.OrderId);

            order.Status = OrderStatus.Fail;
            order.Message = context.Message.Message;

            _context.Update(order);
            await _context.SaveChangesAsync();
        }
    }
}
