using Domain.Database;
using Domain.Database.Seed;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace API.Infrastructure.Extensions;

public static class WebApplicationExtensions
{
    
    public static Task EnsureDbIsReadyAsync(this WebApplication webApplication)
    {
        using var scope = webApplication.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        dbContext.Database.EnsureCreated();
        
        if (!dbContext.Accounts.Any())
        {
            dbContext.Accounts.AddRange(AccountSeed.Accounts);
        }

        if (!dbContext.Products.Any())
        {
            dbContext.Products.AddRange(ProductSeed.Products);
        }

        return dbContext.SaveChangesAsync();
    }
}