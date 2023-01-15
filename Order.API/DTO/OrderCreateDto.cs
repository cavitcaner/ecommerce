namespace Order.API.DTO
{
    public class OrderCreateDto
    {

        public string CustomerId { get; set; }
        public ICollection<OrderItemDto> OrderItems { get; set; } = new List<OrderItemDto>();
        public PaymentDto Payment { get; set; }
        public AddressDto Address { get; set; }

    }
}
