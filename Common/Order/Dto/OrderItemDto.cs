﻿namespace Common.Order.Dto
{
    public class OrderItemDto {

        public Guid ProductId { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
    }
}
