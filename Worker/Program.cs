// WorkerService/Program.cs
using System;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Worker;


var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile("appsettings.development.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables()
    .Build();

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddMassTransit(x =>
        {
            x.AddConsumer<MyMessageConsumer>();
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

                cfg.ReceiveEndpoint("my-queue", e =>
                {
                    e.ConfigureConsumer<MyMessageConsumer>(context);
                });
            });
        });
    })
    .Build();

await host.RunAsync();
