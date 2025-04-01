namespace Domain.Database;

public class Account
{
    public string Id { get; set; }
    public bool IsActive { get; set; }
    public bool IsVerified { get; set; }
    public DateTime CreatedWhenUtc { get; set; } 
    public string Email { get; set; } 
    public ICollection<Order> Orders { get; set; } = new List<Order>();
}

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

public class Payment
{
    public long Id { get; set; }
    public long OrderId { get; set; } public Order Order { get; set; } 
    public DateTime CreatedWhenUtc { get; set; }
    public decimal Price { get; set; }
    public string Status { get; set; }
}