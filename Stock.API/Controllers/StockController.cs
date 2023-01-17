using Common.Stock.DTO;
using Microsoft.AspNetCore.Mvc;
using Stock.API.Business.Abstract;

namespace Stock.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockController : Controller
    {
        private readonly IStockService _stockService;

        public StockController(IStockService stockService)
        {
            _stockService = stockService;
        }

        [HttpGet]
        public async Task<IActionResult> Stock()
        {
            List<StockDto> stocks = await _stockService.GetAllStocksAsync();

            return Ok(stocks);
        }
    }
}
