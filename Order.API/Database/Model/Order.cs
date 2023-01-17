using System.ComponentModel.DataAnnotations;
using Common.Order;

namespace Order.API.Database
{
    public class Order
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string CustomerId { get; set; }
        public string CustomerEmail { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
        public OrderStatus Status { get; set; }
        public string? Message { get; set; }
        public Address Address { get; set; }
    }
}
