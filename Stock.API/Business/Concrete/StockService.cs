using Common;
using Common.Order;
using Common.Payment;
using Common.Stock;
using Common.Stock.DTO;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Stock.API.Database;

namespace Stock.API.Business.Abstract
{
    public class StockService : IStockService
    {
        private readonly StockDbContext _context;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ISendEndpointProvider _sendEndpointProvider;

        public StockService(StockDbContext context, ISendEndpointProvider sendEndpointProvider, IPublishEndpoint publishEndpoint)
        {
            _context = context;
            _sendEndpointProvider = sendEndpointProvider;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<bool> CheckAndDecreaseStockAsync(OrderCreatedEvent orderRequest)
        {
            //TODO: buraya distributed lock yazılabilir. 

            var allInStock = true;

            var orderItemIdList = orderRequest.orderItems.Select(x => x.ProductId).ToList();
            var stocks = await _context.Stocks.Where(x => orderItemIdList.Contains(x.ProductId)).ToListAsync();

            foreach (var item in orderRequest.orderItems)
            {
                if (stocks.Any(x => x.ProductId == item.ProductId && x.UnitInStock >= item.Quantity) == false)
                {
                    allInStock = false;
                    break;
                }
            }

            if (allInStock)
            {
                foreach (var item in orderRequest.orderItems)
                {
                    var stock = stocks.First(x => x.ProductId == item.ProductId);
                    item.ProductName = stock.ProductName;
                    stock.UnitInStock -= item.Quantity;
                }

                await _context.SaveChangesAsync();

                var sendEndpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri($"queue:{QueueConst.StockReservedEventQueueName}"));

                await sendEndpoint.Send(new StockReservedEvent()
                {
                    CustomerEmail = orderRequest.CustomerEmail,
                    CustomerId = orderRequest.CustomerId,
                    OrderId = orderRequest.OrderId,
                    OrderItems = orderRequest.orderItems,
                    Payment = orderRequest.Payment
                });
            }
            else
            {
                await _publishEndpoint.Publish(new StockNotReservedEvent
                {
                    Message = "Yeterli stok bulunamadı.",
                    OrderId = orderRequest.OrderId
                });
            }


            return allInStock;
        }

        public async Task<List<StockDto>> GetAllStocksAsync()
        {
            var stocks = await _context.Stocks.Select(x => new StockDto
            {
                StockCode = x.Id,
                ProductId = x.ProductId,
                ProductName = x.ProductName,
                UnitPrice = x.UnitPrice,
                UnitInStock = x.UnitInStock
            }).ToListAsync();

            return stocks;
        }

        public async Task StockIncreaseAsync(PaymentFailedEvent message)
        {
            var orderItemIdList = message.OrderItems.Select(x => x.ProductId).ToList();
            var stocks = await _context.Stocks.Where(x => orderItemIdList.Contains(x.ProductId)).ToListAsync();

            foreach (var item in message.OrderItems)
            {
                var stock = stocks.First(x => x.ProductId == item.ProductId);
                item.ProductName = stock.ProductName;
                stock.UnitInStock += item.Quantity;
            }

            await _context.SaveChangesAsync();

            await _publishEndpoint.Publish(new StockReversedByMessageEvent
            {
                OrderId = message.OrderId,
                Message = message.Message
            });
        }
    }
}
