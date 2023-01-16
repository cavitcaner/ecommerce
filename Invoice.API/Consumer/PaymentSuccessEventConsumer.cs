using Common.Payment;
using Invoice.API.Adapters;
using MassTransit;

namespace Invoice.API.Consumer
{
    public class PaymentSuccessEventConsumer : IConsumer<PaymentSuccessEvent>
    {
        private readonly IMailAdapter _mailAdapter;

        public PaymentSuccessEventConsumer(IMailAdapter mailAdapter)
        {
            _mailAdapter = mailAdapter;
        }
        
        public async Task Consume(ConsumeContext<PaymentSuccessEvent> context)
        {
            var body = "Siparişiniz oluşturuldu. Sipariş Detayları;";

            context.Message.OrderItems.ForEach(x => body += $"\nÜrün Adı: {x.ProductName}\nAdet: {x.Quantity}\nBirim Fiyat:{x.UnitPrice}");

            Console.WriteLine("Fatura Hazırlandı");

            await _mailAdapter.SendEmail(context.Message.CustomerEmail, "Siparişiniz Oluşturuldu - Fatura", body);

        }
    }
}
