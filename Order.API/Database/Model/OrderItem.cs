using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Order.API.Database
{
    public class OrderItem
    {
        [Key]
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        [Precision(18, 2)]
        public decimal UnitPrice { get; set; }
        public Guid OrderId { get; set; }
        public Order Order { get; set; }
        public int Quantity { get; set; }
    }
}
