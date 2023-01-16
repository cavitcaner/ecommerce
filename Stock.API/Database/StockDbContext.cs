﻿using Microsoft.EntityFrameworkCore;
using Stock.API.Database;

namespace Stock.API.Database
{
    public class StockDbContext : DbContext
    {
        public StockDbContext(DbContextOptions<StockDbContext> options) : base(options)
        {
        }

        public DbSet<Database.Stock> Stocks { get; set; }
    }
}
