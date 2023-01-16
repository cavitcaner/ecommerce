using Common.Order;
using Common.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Stock
{
    public class StockReservedEvent
    {
        public Guid OrderId { get; set; }
        public string CustomerId { get; set; }
        public string CustomerEmail { get; set; }
        public PaymentMessage Payment { get; set; }

        public List<OrderItemMessage> OrderItems { get; set; } = new List<OrderItemMessage>();
    }
}
