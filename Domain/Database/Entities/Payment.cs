namespace Domain.Database.Entities;

public class Payment
{
    public long Id { get; set; }
    public long OrderId { get; set; } public Order Order { get; set; } 
    public DateTime CreatedWhenUtc { get; set; }
    public decimal Price { get; set; }
    public string Status { get; set; }
}