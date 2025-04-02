using Domain.Database.Entities;

namespace Domain.Database.Seed;

public class Accounts
{
    public static IEnumerable<Account> GetAccounts()
    {
        return new[]
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
}