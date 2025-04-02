// WorkerService/Program.cs
using System;
using System.Threading.Tasks;
using Domain.Database;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Worker;
using Worker.Consumers;


var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile("appsettings.development.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables()
    .Build();

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            var dbHost = configuration["DATABASE:Host"] ?? "localhost";
            var dbName = configuration["DATABASE:Name"] ?? "guestDb";
            var dbUser = configuration["DATABASE:User"] ?? "guest";
            var dbPass = configuration["DATABASE:Password"] ?? "guest";

            options.UseMySQL($"Server={dbHost};Port=3306;Database={dbName};User={dbUser};Password={dbPass};");
        });
        services.AddMassTransit(x =>
        {
            x.SetKebabCaseEndpointNameFormatter();
            
            x.AddConsumer<OrderPlacedConsumer>();
            
            x.UsingRabbitMq((context, cfg) =>
            {
                var rabbitHost = configuration["RABBITMQ:Host"] ?? "localhost";
                var rabbitUser = configuration["RABBITMQ:User"] ?? "guest";
                var rabbitPass = configuration["RABBITMQ:Password"] ?? "guest";
                
                cfg.Host(rabbitHost, "/", h =>
                {
                    h.Username(rabbitUser);
                    h.Password(rabbitPass);
                });

                cfg.ConfigureEndpoints(context);
            });
            
        });
        
        
    })
    .Build();

await host.RunAsync();
