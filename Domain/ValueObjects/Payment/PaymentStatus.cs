namespace Domain.ValueObjects.Payment;

public enum PaymentStatusEnum
{
    Pending,
    PaymentReceived,
    PaymentRejected,
    Payed
}

public readonly record struct PaymentStatus
{
    public string Value { get; }

    public PaymentStatus(PaymentStatusEnum paymentStatus) => Value = paymentStatus.ToString();

    public static implicit operator string(PaymentStatus paymentStatus) => paymentStatus.Value;
    public override string ToString() => Value;
}
