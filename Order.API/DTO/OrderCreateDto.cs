using Microsoft.EntityFrameworkCore;
using Order.API.Common;
using Order.API.Order;

namespace Order.API.DTO
{
    public class OrderCreateDto
    {

        public string CustomerId { get; set; }
        public ICollection<OrderItemDto> OrderItems { get; set; } = new List<OrderItemDto>();
        public PaymentDto Payment { get; set; }
        public Address Address { get; set; }

    }
}
