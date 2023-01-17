using Common.Stock.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stock.API.Database;

namespace Stock.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockController : Controller
    {
        private readonly StockDbContext stockDbContext;

        public StockController(StockDbContext stockDbContext)
        {
            this.stockDbContext = stockDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Stock()
        {
            var stocks = await stockDbContext.Stocks.Select(x => new StockDto
            {
                StockCode = x.Id,
                ProductId = x.ProductId,
                ProductName = x.ProductName,
                UnitPrice = x.UnitPrice,
                UnitInStock = x.UnitInStock
            }).ToListAsync();

            return Ok(stocks);
        }
    }
}
