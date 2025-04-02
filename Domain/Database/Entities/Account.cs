namespace Domain.Database.Entities;

public class Account
{
    public string Id { get; set; }
    public bool IsActive { get; set; }
    public bool IsVerified { get; set; }
    public DateTime CreatedWhenUtc { get; set; } 
    public string Email { get; set; } 
    public ICollection<Order> Orders { get; set; } = new List<Order>();
}