namespace Common.Order.Dto
{
    public class OrderDto
    {

        public string CustomerId { get; set; }
        public string CustomerEmail { get; set; }
        public ICollection<OrderItemDto> OrderItems { get; set; } = new List<OrderItemDto>();
        public PaymentDto Payment { get; set; }
        public AddressDto Address { get; set; }

    }
}
