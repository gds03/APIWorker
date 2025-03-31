namespace Domain.Database;

public class Account
{
    public long Id { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedWhenUtc { get; set; } 
    public string Email { get; set; } 
    public ICollection<Order> Orders { get; set; } = new List<Order>();
}
public class Order
{
    public long Id { get; set; }
    public long AccountId { get; set; }
    public DateTime CreatedWhenUtc { get; set; }
    public string Sku { get; set; }
    public string Status { get; set; }
    public int Amount { get; set; }
    public decimal Price { get; set; }

    public Account Account { get; set; }
    public Payment? Payment { get; set; }
}

public class Payment
{
    public long Id { get; set; }
    public long OrderId { get; set; }
    
    public DateTime CreatedWhenUtc { get; set; }
    public decimal Price { get; set; }
    public string Status { get; set; }
    public Order Order { get; set; }
}