namespace Domain.ValueObjects.Payment;

public enum PaymentTypeEnum
{
    Card,
    PayPal,
    Crypto,
}

public readonly record struct PaymentType
{
    public string Value { get; }

    public PaymentType(PaymentTypeEnum paymentType) => Value = paymentType.ToString();
    public static implicit operator string(PaymentType paymentStatus) => paymentStatus.Value;
    public override string ToString() => Value;
}