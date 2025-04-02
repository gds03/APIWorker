
namespace Domain.Database.Entities;

public class Order
{
    public long Id { get; set; }
    public string AccountId { get; set; } public Account Account { get; set; } 
    public Payment? Payment { get; set; }
    public DateTime CreatedWhenUtc { get; set; }

    public string Identifier { get; set; }
    public decimal Total { get; set; }
    public string Status { get; set; }
    public ICollection<Product> Products { get; set; } = new List<Product>();
}