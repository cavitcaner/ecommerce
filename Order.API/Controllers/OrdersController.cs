using Common.Order.Dto;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Order.API.Business.Abstract;

namespace Order.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<IActionResult> Orders(OrderDto orderRequest)
        {
            var orderCreatedEvent  = await _orderService.CreateOrderAsync(orderRequest);

            return Accepted("/Orders", new { OrderId = orderCreatedEvent.OrderId});
        }

        [HttpGet]
        public async Task<IActionResult> Orders()
        {
            var orders = await _orderService.GetAllOrdersAsync();

            return Ok(orders);
        }

        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrder(Guid orderId)
        {
            var order = await _orderService.GetOrderAsync(orderId);

            if (order == null)
                return NotFound();

            return Ok(order);
        }
     
    }
}
