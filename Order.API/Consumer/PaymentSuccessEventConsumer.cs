using Common.Order;
using Common.Payment;
using MassTransit;
using Order.API.Business.Abstract;

namespace Order.API.Consumer
{
    public class PaymentSuccessEventConsumer : IConsumer<PaymentSuccessEvent>
    {
        private readonly IOrderService _orderService;

        public PaymentSuccessEventConsumer(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task Consume(ConsumeContext<PaymentSuccessEvent> context)
        {
            await _orderService.UpdateOrderStateAsync(context.Message.OrderId, OrderStatus.Success, "Sipariş başarılı bir şekilde oluşturulmuştur.");
        }
    }
}
