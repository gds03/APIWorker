namespace Events;


public record OrderInfo(
    string AccountId,
    DateTime CreatedWhenUtc,
    string Identifier,
    decimal Total,
    string Status
);

public record OrderPlaced(OrderInfo OrderInfo, PaymentInfo PaymentInfo);
    
    