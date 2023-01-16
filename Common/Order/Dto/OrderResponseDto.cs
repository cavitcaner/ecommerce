namespace Common.Order.Dto
{
    public class OrderResponseDto
    {
        public Guid OrderId { get; set; }
        public OrderStatus Status { get; set; }
        public string? Message { get; set; }
        public decimal TotalPrice { get; set; }
        public string CustomerId { get; set; }
    }
}
