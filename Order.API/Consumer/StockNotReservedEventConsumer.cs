using Common.Order;
using Common.Stock;
using MassTransit;
using Order.API.Business.Abstract;

namespace Order.API.Consumer
{
    public class StockNotReservedEventConsumer : IConsumer<StockNotReservedEvent>
    {
        private readonly IOrderService _orderService;

        public StockNotReservedEventConsumer(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task Consume(ConsumeContext<StockNotReservedEvent> context)
        {
            await _orderService.UpdateOrderStateAsync(context.Message.OrderId, OrderStatus.Fail, context.Message.Message);
        }
    }
}
