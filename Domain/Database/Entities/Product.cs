namespace Domain.Database.Entities;

public class Product
{
    public long Id { get; set; }
    public bool IsDiscontinued { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public long StockQuantity { get; set; }
    public string Sku { get; set; }
    public decimal Price { get; set; }
    public DateTime CreatedWhenUtc { get; set; }
    
    public ICollection<Order> Orders { get; set; } = new List<Order>();
}