namespace Domain.Database.Entities;

public abstract class Payment
{
    public long Id { get; set; }
    public long OrderId { get; set; } public Order Order { get; set; } 
    public DateTime CreatedWhenUtc { get; set; }
    public decimal Price { get; set; }
    public string Status { get; set; }
    public string PaymentType { get; set; }
}

public class CardPayment : Payment
{
    public string CardNumber { get; set; }
    public int Month { get; set; }
    public int Year { get; set; }
    public int CVV { get; set; }
    public string Name { get; set; }
}

public class PayPalPayment : Payment
{
    public string PayPalEmail { get; set; }
}

public class CryptoPayment : Payment
{
    public string WalletAddress { get; set; }
    public string CryptoType { get; set; } // e.g., Bitcoin, Ethereum
}