namespace Domain.ValueObjects.Orders;

public enum OrderStatusEnum
{
    Pending,
    PaymentReceived,
    PaymentRejected,
    Payed
}

public readonly record struct OrderStatus
{
    public string Value { get; }

    public OrderStatus(OrderStatusEnum orderStatus) => Value = orderStatus.ToString();

    public static implicit operator string(OrderStatus orderStatus) => orderStatus.Value;
    public override string ToString() => Value;
}
