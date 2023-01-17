using Common.Order;
using Common.Payment;
using Common.Stock.DTO;

namespace Stock.API.Business.Abstract
{
    public interface IStockService
    {
        Task<bool> CheckAndDecreaseStockAsync(OrderCreatedEvent orderRequest);
        Task<List<StockDto>> GetAllStocksAsync();
        Task StockIncreaseAsync(PaymentFailedEvent message);
    }
}
