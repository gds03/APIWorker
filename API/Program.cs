﻿// ApiService/Program.cs
using API.Infrastructure.Extensions;
using Domain.Database;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile("appsettings.development.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables()
    .Build();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMassTransit(x =>
{
    x.AddEntityFrameworkOutbox<AppDbContext>(o =>
    {
        o.UseMySql();
        o.UseBusOutbox();
    });
    
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


builder.Services.AddDbContext<AppDbContext>(options =>
{
    var dbHost = configuration["DATABASE:Host"] ?? "localhost";
    var dbName = configuration["DATABASE:Name"] ?? "guestDb";
    var dbUser = configuration["DATABASE:User"] ?? "guest";
    var dbPass = configuration["DATABASE:Password"] ?? "guest";

    options.UseMySQL($"Server={dbHost};Port=3306;Database={dbName};User={dbUser};Password={dbPass};");
});

builder.Services.AddHttpClients();
builder.Services.AddHandlers();
builder.Services.AddRouting();
builder.Services.AddControllers(options => {
    options.SuppressAsyncSuffixInActionNames = false;
});
builder.Services.AddEndpointsApiExplorer(); // For minimal APIs
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
});

builder.Logging.AddFilter("MassTransit", LogLevel.Debug);

builder.WebHost.UseUrls("http://0.0.0.0:5000"); // Ensure API listens on all interfaces

var app = builder.Build();
app.EnsureDbIsReady();

// Enable Swagger UI in development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
        c.RoutePrefix = "swagger";
        c.DisplayOperationId();
    });
}

// Minimal API
app.MapGet("/", () => "API is running").WithName("EntryPoint");
app.MapControllers();
await app.RunAsync();
