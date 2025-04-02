namespace Domain.ValueObjects.Order;

public enum OrderStatusEnum
{
    WaitingForPayment,
    WaitingForDispatched,
    Dispatched,
    Delivered
}

public readonly record struct OrderStatus
{
    public string Value { get; }

    public OrderStatus(OrderStatusEnum orderStatus) => Value = orderStatus.ToString();
    public static implicit operator OrderStatus(string orderStatus) => new(Enum.Parse<OrderStatusEnum>(orderStatus));
    public static implicit operator string(OrderStatus orderStatus) => orderStatus.Value;
    public override string ToString() => Value;
}
