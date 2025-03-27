// ApiService/Program.cs
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using Contracts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile("appsettings.development.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables()
    .Build();

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://0.0.0.0:5000"); // Ensure API listens on all interfaces
builder.Services.AddMassTransit(x =>
{
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
    });
});

builder.Services.AddControllers();
var app = builder.Build();

app.MapGet("/", () => "API is running");

app.MapPost("/send", async (IBus bus) =>
{
    await bus.Publish(new SendMessage { Time = TimeOnly.FromDateTime(DateTime.UtcNow).ToString("HH:mm:ss") });
    return Results.Ok("Message sent!");
});

app.Run();