using Common.Order;
using Common.Payment;

namespace Common.Stock
{
    public class StockNotReservedEvent
    {
        public Guid OrderId { get; set; }
        public string Message { get; set; }
    }
}
