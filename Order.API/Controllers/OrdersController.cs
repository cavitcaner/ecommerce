using Common.Order;
using Common.Order.Dto;
using Common.Payment;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Order.API.Database;

namespace Order.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly OrderDbContext _context;
        private readonly IPublishEndpoint _publishEndpoint;

        public OrdersController(OrderDbContext context, IPublishEndpoint publishEndpoint)
        {
            _context = context;
            _publishEndpoint = publishEndpoint;
        }

        [HttpPost]
        public async Task<IActionResult> Orders(OrderDto orderRequest)
        {
            var order = new Database.Order();
            order.Message = "Kontrol Ediliyor!";
            order.CustomerId = orderRequest.CustomerId;
            order.CustomerEmail = orderRequest.CustomerEmail;
            order.Items = orderRequest.OrderItems.Select(x => new OrderItem
            {
                ProductId = x.ProductId,
                Quantity = x.Quantity,
                UnitPrice = x.UnitPrice,
                OrderId = order.Id
            }).ToList();
            order.Address = new Address
            {
                City = orderRequest.Address.City,
                FullAdress = orderRequest.Address.FullAdress,
                Province = orderRequest.Address.Province,
                District = orderRequest.Address.District,
            };

            await _context.AddAsync(order);
            await _context.SaveChangesAsync();


            var orderCreatedEvent = new OrderCreatedEvent()
            {
                CustomerId = orderRequest.CustomerId,
                CustomerEmail = orderRequest.CustomerEmail,
                OrderId = order.Id,
                Payment = new PaymentMessage
                {
                    CardName = orderRequest.Payment.CardName,
                    CardNumber = orderRequest.Payment.CardNumber,
                    Expiration = orderRequest.Payment.Expiration,
                    CVV = orderRequest.Payment.CVV,
                    TotalPrice = orderRequest.OrderItems.Sum(x => x.UnitPrice * x.Quantity)
                },
                orderItems = orderRequest.OrderItems.Select(x => new OrderItemMessage
                {
                    Quantity = x.Quantity,
                    ProductId = x.ProductId,
                    UnitPrice = x.UnitPrice
                }).ToList()
            };


            await _publishEndpoint.Publish(orderCreatedEvent);


            return Accepted("/Orders", new { OrderId = order.Id });
        }

        [HttpGet]
        public async Task<IActionResult> Orders()
        {
            var orders = await _context.Orders
                .Select(x => new OrderResponseDto
                {
                    OrderId = x.Id,
                    CustomerId = x.CustomerId,
                    Status = x.Status,
                    Message = x.Message,
                    TotalPrice = x.Items.Sum(z => z.UnitPrice * z.Quantity)
                })
                .ToListAsync();

            if (orders.Any() == false)
                return NotFound();

            return Ok(orders);
        }

        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrder(Guid orderId)
        {
            var order = await _context.Orders.Where(x => x.Id == orderId)
                .Select(x => new OrderResponseDto
                {
                    OrderId = x.Id,
                    Status = x.Status,
                    CustomerId = x.CustomerId,
                    Message = x.Message,
                    TotalPrice = x.Items.Sum(z => z.UnitPrice * z.Quantity)
                })
                .FirstOrDefaultAsync();

            if (order == null)
                return NotFound();

            return Ok(order);
        }
     
    }
}
