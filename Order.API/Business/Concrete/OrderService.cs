using Common.Order;
using Common.Order.Dto;
using Common.Payment;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Order.API.Database;

namespace Order.API.Business.Abstract
{
    public class OrderService : IOrderService
    {
        private readonly OrderDbContext _context;

        private readonly IPublishEndpoint _publishEndpoint;
        public OrderService(OrderDbContext context, IPublishEndpoint publishEndpoint)
        {
            _context = context;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<OrderCreatedEvent> CreateOrderAsync(OrderDto orderRequest)
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


            var createdEvent = new OrderCreatedEvent()
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

            await _publishEndpoint.Publish(createdEvent);

            return createdEvent;
        }

        public async Task<List<OrderResponseDto>> GetAllOrdersAsync()
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

            return orders;
        }

        public async Task<OrderResponseDto> GetOrderAsync(Guid orderId)
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

            return order;
        }

        public async Task UpdateOrderStateAsync(Guid orderId, OrderStatus status, string message)
        {
            var order = await _context.Orders.FirstAsync(x => x.Id == orderId);

            order.Status = status;
            order.Message = message;

            _context.Update(order);
            await _context.SaveChangesAsync();
        }
    }
}
