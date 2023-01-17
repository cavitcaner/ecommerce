using Common.Order;
using Common.Order.Dto;

namespace Order.API.Business.Abstract
{
    public interface IOrderService
    {
        Task<OrderCreatedEvent> CreateOrderAsync(OrderDto orderRequest);
        Task<List<OrderResponseDto>> GetAllOrdersAsync();
        Task<OrderResponseDto> GetOrderAsync(Guid orderId);
        Task UpdateOrderStateAsync(Guid orderId, OrderStatus status, string message);
    }
}
