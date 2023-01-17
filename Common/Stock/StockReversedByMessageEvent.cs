namespace Common.Stock
{
    public class StockReversedByMessageEvent
    {
        public Guid OrderId { get; set; }
        public string Message { get; set; }
    }
}
