using Common.Payment;
using Common.Stock;
using MassTransit;
using Stock.API.Business.Abstract;

namespace Stock.API.Consumer
{
    public class PaymentFailedConsumer : IConsumer<PaymentFailedEvent>
    {
        private readonly IStockService _stockService;
        private readonly IPublishEndpoint _publishEndpoint;

        public PaymentFailedConsumer(IStockService stockService, IPublishEndpoint publishEndpoint)
        {
            _stockService = stockService;
            _publishEndpoint = publishEndpoint;
        }

        public async Task Consume(ConsumeContext<PaymentFailedEvent> context)
        {
            await _stockService.StockIncreaseAsync(context.Message);

            await _publishEndpoint.Publish(new StockReversedByMessageEvent
            {
                OrderId = context.Message.OrderId,
                Message = context.Message.Message
            });
        }
    }
}
