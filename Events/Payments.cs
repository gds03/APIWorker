namespace Events;

public record PaymentInfo(
    string Status,
    string PaymentType,
    decimal Price, 
    string CardNumber, 
    int Month, 
    int Year, 
    int Cvv, 
    string Owner
);