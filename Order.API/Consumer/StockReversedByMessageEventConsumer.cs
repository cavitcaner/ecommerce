using Common.Order;
using Common.Stock;
using MassTransit;
using Order.API.Business.Abstract;

namespace Order.API.Consumer
{
    public class StockReversedByMessageEventConsumer : IConsumer<StockReversedByMessageEvent>
    {
        private readonly IOrderService _orderService;

        public StockReversedByMessageEventConsumer(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task Consume(ConsumeContext<StockReversedByMessageEvent> context)
        {
            await _orderService.UpdateOrderStateAsync(context.Message.OrderId, OrderStatus.Fail, context.Message.Message);
        }
    }
}
