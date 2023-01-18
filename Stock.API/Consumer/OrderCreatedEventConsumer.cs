using Common;
using Common.Order;
using Common.Stock;
using MassTransit;
using Stock.API.Business.Abstract;

namespace Stock.API.Consumer
{
    public class OrderCreatedEventConsumer : IConsumer<OrderCreatedEvent>
    {
        private readonly IStockService _stockService;

        public OrderCreatedEventConsumer( IStockService stockService)
        {
            _stockService = stockService;
        }

        public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
        {
            await _stockService.CheckAndDecreaseStockAsync(context.Message);
          
        }
    }
}
