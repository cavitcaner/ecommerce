using Microsoft.EntityFrameworkCore;
using Order.API.Order.Database;

namespace Order.API.Database
{
    public class OrderDbContext : DbContext
    {
        public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options)
        {
        }

        public DbSet<Order.Database.Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
    }
}
