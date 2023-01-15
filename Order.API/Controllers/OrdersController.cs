using Common.Order;
using Common.Payment;
using Events.Order;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Order.API.Database;
using Order.API.DTO;

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
        public async Task<IActionResult> Create(OrderCreateDto orderRequest)
        {
            var order = new Database.Order();
            order.CustomerId = orderRequest.CustomerId;
            order.Items = orderRequest.OrderItems.Select(x => new OrderItem
            {
                ProductId = x.ProductId,
                ProductName = x.ProductName,
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
                    ProductId = x.ProductId
                }).ToList()
            };


            await _publishEndpoint.Publish(orderCreatedEvent);


            return Ok();
        }
    }
}
