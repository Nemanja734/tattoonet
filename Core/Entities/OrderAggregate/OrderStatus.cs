namespace Core.Entities.OrderAggregate;

// we'll use this to set the order status
public enum OrderStatus
{
    Pending,
    PaymentReceived,
    PaymentFailed,
    PaymentMismatch,
}
