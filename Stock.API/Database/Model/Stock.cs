using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Stock.API.Database
{
    public class Stock
    {
        [Key]
        public int Id { get; set; }
        public Guid ProductId { get; set; } = Guid.NewGuid();
        public string ProductName { get; set; }
        [Precision(18, 2)]
        public double UnitPrice { get; set; }
        public int UnitInStock { get; set; }
    }
}
