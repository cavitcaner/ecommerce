using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class QueueConst
    {
        public const string StockReservedEventQueueName = "stock-reserved-queue";
        public const string StockNotReservedEventQueueName = "stock-not-reserved-queue";
        public const string OrderCreatedEventQueueName = "order-created-queue";
        public const string PaymentSuccessEventQueueName = "payment-successed-queue";
        public const string PaymentFailedEventQueueName = "payment-not-successed-queue";
    }
}
