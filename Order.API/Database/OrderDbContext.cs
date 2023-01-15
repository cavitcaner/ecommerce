using Microsoft.EntityFrameworkCore;
using Order.API.Database;

namespace Order.API.Database
{
    public class OrderDbContext : DbContext
    {
        public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options)
        {
        }

        public DbSet<Database.Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
    }
}
