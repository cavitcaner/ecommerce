using Common;
using Common.Order;
using Common.Stock;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Stock.API.Database;

namespace Stock.API.Consumer
{
    public class OrderCreatedEventConsumer : IConsumer<OrderCreatedEvent>
    {
        private readonly StockDbContext _context;
        private readonly ISendEndpointProvider _sendEndpointProvider;
        private readonly IPublishEndpoint _publishEndpoint;

        public OrderCreatedEventConsumer(StockDbContext context, ISendEndpointProvider sendEndpointProvider, IPublishEndpoint publishEndpoint)
        {
            _context = context;
            _sendEndpointProvider = sendEndpointProvider;
            _publishEndpoint = publishEndpoint;
        }

        public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
        {
            //TODO: buraya distributed lock yazılabilir. 

            var allInStock = true;

            var orderItemIdList = context.Message.orderItems.Select(x => x.ProductId).ToList();
            var stocks = await _context.Stocks.Where(x => orderItemIdList.Contains(x.ProductId)).ToListAsync();
            
            foreach (var item in context.Message.orderItems)
            {
                if (stocks.Any(x => x.ProductId == item.ProductId && x.UnitInStock >= item.Quantity) == false)
                {
                    allInStock = false;
                    break;
                }
            }

            if (allInStock)
            {
                foreach (var item in context.Message.orderItems)
                {
                    var stock = stocks.First(x => x.ProductId == item.ProductId);
                    item.ProductName = stock.ProductName;
                    stock.UnitInStock -= item.Quantity;
                }

                await _context.SaveChangesAsync();

                var sendEndpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri($"queue:{QueueConst.StockReservedEventQueueName}"));

                await sendEndpoint.Send(new StockReservedEvent()
                {
                    CustomerEmail = context.Message.CustomerEmail,
                    CustomerId = context.Message.CustomerId,
                    OrderId = context.Message.OrderId,
                    OrderItems = context.Message.orderItems,
                    Payment = context.Message.Payment
                });
            }
            else
            {
                await _publishEndpoint.Publish(new StockNotReservedEvent
                {
                    Message = "Yeterli stok bulunamadı.",
                    OrderId = context.Message.OrderId
                });
            }

        }
    }
}
