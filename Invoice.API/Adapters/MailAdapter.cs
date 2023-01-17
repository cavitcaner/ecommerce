namespace Invoice.API.Adapters
{
    public class MailAdapter : IMailAdapter
    {
        public async Task SendEmail(string to, string title, string body)
        {
            //Burada smtp girilerek smtp üzerinden mail gönderme işlemi yapılabilir.
            Console.WriteLine(body);
            Console.WriteLine($"'{to}' adresine '{title}' başlığı ile mail gönderildi.");
        }
    }
}
