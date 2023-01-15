using System.ComponentModel.DataAnnotations;
using Order.API.Common;

namespace Order.API.Order
{
    public class Order
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string CustomerId { get; set; }
        public DateTime CreateDate { get; set; }
        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
        public OrderStatus Status { get; set; }
        public string? FailMessage { get; set; }
        public Address Address { get; set; }
    }
}
