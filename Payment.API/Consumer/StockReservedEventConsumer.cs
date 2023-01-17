using Common.Payment;
using Common.Stock;
using MassTransit;

namespace Payment.API.Consumer
{
    public class StockReservedEventConsumer : IConsumer<StockReservedEvent>
    {
        private readonly ISendEndpointProvider _sendEndpointProvider;
        private readonly IPublishEndpoint _publishEndpoint;

        public StockReservedEventConsumer(IPublishEndpoint publishEndpoint, ISendEndpointProvider sendEndpointProvider)
        {
            _publishEndpoint = publishEndpoint;
            _sendEndpointProvider = sendEndpointProvider;
        }

        public async Task Consume(ConsumeContext<StockReservedEvent> context)
        {
            //test amaçlı yazılmıştır.
            if (context.Message.Payment.CardName == "fail")
            {
                Console.WriteLine("Ödeme alınamadı!");

                await _publishEndpoint.Publish(new PaymentFailedEvent
                {
                    OrderId = context.Message.OrderId,
                    OrderItems = context.Message.OrderItems,
                    Message = "Ödeme alırken hata oluştu. Kart bilgierinizi kontrol ediniz."
                });

                return;
            }

            Console.WriteLine("Ödeme başarılı bir şekilde alındı!");
            await _publishEndpoint.Publish(new PaymentSuccessEvent
            {
                CustomerEmail = context.Message.CustomerEmail,
                OrderId = context.Message.OrderId,
                OrderItems = context.Message.OrderItems
            });
        }
    }
}
