using Common;
using Common.Order;
using Common.Stock;
using MassTransit;
using Stock.API.Business.Abstract;

namespace Stock.API.Consumer
{
    public class OrderCreatedEventConsumer : IConsumer<OrderCreatedEvent>
    {
        private readonly ISendEndpointProvider _sendEndpointProvider;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IStockService _stockService;

        public OrderCreatedEventConsumer(ISendEndpointProvider sendEndpointProvider, IPublishEndpoint publishEndpoint, IStockService stockService)
        {
            _sendEndpointProvider = sendEndpointProvider;
            _publishEndpoint = publishEndpoint;
            _stockService = stockService;
        }

        public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
        {
            var allInStock = await _stockService.CheckAndDecreaseStockAsync(context.Message);
            if (allInStock)
            {
                var sendEndpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri($"queue:{QueueConst.StockReservedEventQueueName}"));

                await sendEndpoint.Send(new StockReservedEvent()
                {
                    CustomerEmail = context.Message.CustomerEmail,
                    CustomerId = context.Message.CustomerId,
                    OrderId = context.Message.OrderId,
                    OrderItems = context.Message.orderItems,
                    Payment = context.Message.Payment
                });
            }
            else
            {
                await _publishEndpoint.Publish(new StockNotReservedEvent
                {
                    Message = "Yeterli stok bulunamadı.",
                    OrderId = context.Message.OrderId
                });
            }

        }
    }
}
