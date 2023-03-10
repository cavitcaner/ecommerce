namespace Common
{
    public class QueueConst
    {
        public const string StockReservedEventQueueName = "stock-reserved-queue";
        public const string StockNotReservedEventQueueName = "stock-not-reserved-queue";
        public const string OrderCreatedEventQueueName = "order-created-queue";
        public const string PaymentSuccessEventQueueName = "payment-successed-queue";
        public const string StockReversedByMessageEvent = "order-stock-reversed-queue";
        public const string StockPaymentFailedEventQueueName = "stock-payment-failed-queue";
        public const string InvoicePaymentSuccessEventName = "invoice-payment--success-queue";
    }
}
