using Domain.Database.Entities;

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
        
        
        // Payment
        b.Entity<Payment>()
            .HasDiscriminator<string>("PaymentType")
            .HasValue<CardPayment>("Card")
            .HasValue<PayPalPayment>("PayPal")
            .HasValue<CryptoPayment>("Crypto");

        b.Entity<Account>().HasData(Seed.Accounts.GetAccounts());
        b.Entity<Product>().HasData(Seed.Products.GetProducts());
    }

    public override int SaveChanges()
    {
        foreach (var entry in ChangeTracker.Entries())
        {
            if (entry.State == EntityState.Added && entry.Property("CreatedWhenUtc") != null)
            {
                entry.Property("CreatedWhenUtc").CurrentValue = DateTime.UtcNow;
            }
        }

        return base.SaveChanges();
    }
}