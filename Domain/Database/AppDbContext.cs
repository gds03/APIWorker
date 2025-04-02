using Domain.Database.Entities;
using Domain.Infrastructure;
using MassTransit;

namespace Domain.Database;

using Microsoft.EntityFrameworkCore;
using System;

public class AppDbContext : DbContext
{
    public DbSet<Account> Accounts { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Payment> Payments { get; set; }

    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {        
    }
    protected override void OnModelCreating(ModelBuilder b)
    {
        //
        // Orders
        
        // Order-Account 1:1 - Account-Order:1-N
        b.Entity<Order>()
            .HasOne(o => o.Account)
            .WithMany(a => a.Orders)
            .HasForeignKey(o => o.AccountId);
        
        // Order to payment 1:1
        b.Entity<Order>()
            .HasOne(o => o.Payment)
            .WithOne(p => p.Order)
            .HasForeignKey<Payment>(p => p.OrderId);
        
        // Order to Product 1:N - Product-Order 1:N
        b.Entity<Order>()
            .HasMany(o => o.Products)
            .WithMany(p => p.Orders);

        b.Entity<Order>()
            .HasIndex(o => o.Identifier);
        
        //
        // Payment
        
        b.Entity<Payment>()
            .HasDiscriminator<string>("PaymentType")
            .HasValue<CardPayment>("Card")
            .HasValue<PayPalPayment>("PayPal")
            .HasValue<CryptoPayment>("Crypto");
        
        //
        // MassTransit Outbox

        b.AddInboxStateEntity(x => x.Metadata.SetTableName("___MassTransit___InboxState"));
        b.AddOutboxMessageEntity(x => x.Metadata.SetTableName("___MassTransit___OutboxMessage"));
        b.AddOutboxStateEntity(x => x.Metadata.SetTableName("___MassTransit___OutboxState"));
    }

    public override int SaveChanges()
    {
        SetCreatedWhenUtc();

        return base.SaveChanges();
    }
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        SetCreatedWhenUtc();

        return await base.SaveChangesAsync(cancellationToken);
    }
    private void SetCreatedWhenUtc()
    {
        foreach (var entry in ChangeTracker.Entries())
        {
            string propertyName = nameof(ICreatedWhenUtc.CreatedWhenUtc);
            if (entry.State == EntityState.Added && entry.Entity is ICreatedWhenUtc createdWhenUtc && createdWhenUtc.CreatedWhenUtc == default)
            {
                createdWhenUtc.CreatedWhenUtc = DateTime.UtcNow;
            }
        }
    }
}