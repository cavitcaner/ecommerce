using Common.Payment;
using Common.Stock;
using MassTransit;
using Stock.API.Business.Abstract;

namespace Stock.API.Consumer
{
    public class PaymentFailedConsumer : IConsumer<PaymentFailedEvent>
    {
        private readonly IStockService _stockService;

        public PaymentFailedConsumer(IStockService stockService)
        {
            _stockService = stockService;
        }

        public async Task Consume(ConsumeContext<PaymentFailedEvent> context)
        {
            await _stockService.StockIncreaseAsync(context.Message);

        }
    }
}
