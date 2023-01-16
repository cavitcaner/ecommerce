using Common.Order.Dto;
using Common.Payment;
using System.Net;
using System.Net.Mail;
using System.Runtime.CompilerServices;

namespace Invoice.API.Adapters
{
    public class MailAdapter : IMailAdapter
    {
        public async Task SendEmail(string to, string title, string body)
        {
            Console.WriteLine(body);
            Console.WriteLine($"'{to}' adresine '{title}' başlığı ile mail gönderildi.");
        }
    }
}
