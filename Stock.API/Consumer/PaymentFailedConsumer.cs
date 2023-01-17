using Common;
using Common.Order;
using Common.Payment;
using MassTransit;
using MassTransit.Transports;
using Microsoft.EntityFrameworkCore;
using Stock.API.Database;

namespace Stock.API.Consumer
{
    public class PaymentFailedConsumer : IConsumer<PaymentFailedEvent>
    {
        private readonly StockDbContext _context;

        public PaymentFailedConsumer(StockDbContext context)
        {
            _context = context;
        }

        public async Task Consume(ConsumeContext<PaymentFailedEvent> context)
        {
            var orderItemIdList = context.Message.OrderItems.Select(x => x.ProductId).ToList();
            var stocks = await _context.Stocks.Where(x => orderItemIdList.Contains(x.ProductId)).ToListAsync();

            foreach (var item in context.Message.OrderItems)
            {
                var stock = stocks.First(x => x.ProductId == item.ProductId);
                item.ProductName = stock.ProductName;
                stock.UnitInStock += item.Quantity;
            }

            await _context.SaveChangesAsync();
        }
    }
}
