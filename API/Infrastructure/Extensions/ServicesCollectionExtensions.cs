using API.Features.Orders;
using API.Features.Orders.PlaceOrder;
using API.HttpClients;
using Microsoft.Extensions.DependencyInjection;

namespace API.Infrastructure.Extensions;

public static class ServicesCollectionExtensions
{
    public static IServiceCollection AddHandlers(this IServiceCollection services)
    {
        services.Scan(scan => scan
                .FromAssemblyOf<IHandler>() 
                .AddClasses(classes => classes.AssignableTo<IHandler>()) // Find all IHandler implementations
                .AsImplementedInterfaces() // Register them as their own interfaces
                .WithScopedLifetime() // Use Scoped lifetime (can be changed)
        );
        
        return services;
    }

    public static IServiceCollection AddHttpClients(this IServiceCollection services)
    {
        services.AddTransient<IPriceHttpClient, PriceHttpClient>();
        services.AddSingleton<IOrderIdentifierGenerator, OrderIdentifierGenerator>();
        return services;
    }
}