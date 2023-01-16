using Common.Payment;
using Common.Stock;
using MassTransit;

namespace Payment.API.Consumer
{
    public class StockReservedEventConsumer : IConsumer<StockReservedEvent>
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public StockReservedEventConsumer(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public async Task Consume(ConsumeContext<StockReservedEvent> context)
        {
            Console.WriteLine("Ödeme başarılı bir şekilde alındı!");

            //test amaçlı yazılmıştır.
            if(context.Message.Payment.CardName == "fail")
            {
                await _publishEndpoint.Publish(new PaymentSuccessEvent
                {
                    CustomerEmail = context.Message.CustomerEmail,
                    OrderId = context.Message.OrderId,
                    OrderItems = context.Message.OrderItems
                });

                return;
            }

            await _publishEndpoint.Publish(new PaymentSuccessEvent
            {
                CustomerEmail = context.Message.CustomerEmail,
                OrderId = context.Message.OrderId,
                OrderItems = context.Message.OrderItems
            });
        }
    }
}
