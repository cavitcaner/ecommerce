using Common.Order.Dto;

namespace Invoice.API.Adapters
{
    public interface IMailAdapter
    {
        Task SendEmail(string to, string title, string body);
    }
}
