using Domain.Database.Entities;

namespace Domain.Database.Seed;

public class AccountSeed
{
    public static IEnumerable<Account> Accounts => new[]
    {
        new Account
        {
            Id = "9999-ZYXWVUTS-99",
            Email = "admin@admin.com",
            IsActive = true,
            IsVerified = true
        }
    };
}