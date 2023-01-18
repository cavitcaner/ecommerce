using Common.Order;
using Common.Payment;
using Common.Stock.DTO;
using Microsoft.EntityFrameworkCore;
using Stock.API.Database;

namespace Stock.API.Business.Abstract
{
    public class StockService : IStockService
    {
        private readonly StockDbContext _context;

        public StockService(StockDbContext context)
        {
            _context = context;
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
        }
    }
}
