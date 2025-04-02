using Domain.Database.Entities;

namespace Domain.Database.Seed;

public class Products
{
    public static IEnumerable<Product> GetProducts()
    {
        return new []
        {
            // Laptops
            new Product
            {
                Id = 1,
                Name = "Dell XPS 15",
                Description = "Powerful laptop with Intel Core i7, 16GB RAM, and 512GB SSD.",
                StockQuantity = 10,
                Sku = "LAPTOP-001",
                Price = 1599.99m,
                CreatedWhenUtc = DateTime.UtcNow,
                IsDiscontinued = false
            },
            new Product
            {
                Id = 2,
                Name = "MacBook Pro 14\"",
                Description = "Apple M2 Pro chip, 16GB RAM, 512GB SSD, Retina Display.",
                StockQuantity = 8,
                Sku = "LAPTOP-002",
                Price = 1999.99m,
                CreatedWhenUtc = DateTime.UtcNow,
                IsDiscontinued = false
            },
            new Product
            {
                Id = 3,
                Name = "Lenovo ThinkPad X1 Carbon",
                Description = "Business laptop with Intel Core i7, 16GB RAM, and 1TB SSD.",
                StockQuantity = 6,
                Sku = "LAPTOP-003",
                Price = 1899.99m,
                CreatedWhenUtc = DateTime.UtcNow,
                IsDiscontinued = false
            },
            new Product
            {
                Id = 4,
                Name = "ASUS ROG Zephyrus G14",
                Description = "Gaming laptop with AMD Ryzen 9, RTX 4060, and 16GB RAM.",
                StockQuantity = 5,
                Sku = "LAPTOP-004",
                Price = 1799.99m,
                CreatedWhenUtc = DateTime.UtcNow,
                IsDiscontinued = false
            },
            new Product
            {
                Id = 5,
                Name = "HP Spectre x360 14",
                Description = "2-in-1 convertible laptop with OLED display and Intel Core i7.",
                StockQuantity = 7,
                Sku = "LAPTOP-005",
                Price = 1699.99m,
                CreatedWhenUtc = DateTime.UtcNow,
                IsDiscontinued = false
            },
            new Product
            {
                Id = 6,
                Name = "Acer Swift 3",
                Description = "Budget-friendly laptop with AMD Ryzen 7, 16GB RAM, and 512GB SSD.",
                StockQuantity = 12,
                Sku = "LAPTOP-006",
                Price = 799.99m,
                CreatedWhenUtc = DateTime.UtcNow,
                IsDiscontinued = false
            },

            // Cellphones
            new Product
            {
                Id = 7,
                Name = "iPhone 14 Pro",
                Description = "128GB, Space Black, 48MP main camera, A16 Bionic chip.",
                StockQuantity = 15,
                Sku = "PHONE-001",
                Price = 999.99m,
                CreatedWhenUtc = DateTime.UtcNow,
                IsDiscontinued = false
            },
            new Product
            {
                Id = 8,
                Name = "Samsung Galaxy S23 Ultra",
                Description = "256GB, Phantom Black, 200MP camera, S Pen included.",
                StockQuantity = 12,
                Sku = "PHONE-002",
                Price = 1199.99m,
                CreatedWhenUtc = DateTime.UtcNow,
                IsDiscontinued = false
            },
            new Product
            {
                Id = 9,
                Name = "Google Pixel 7 Pro",
                Description = "128GB, Obsidian, Tensor G2 chip, AI-powered camera.",
                StockQuantity = 9,
                Sku = "PHONE-003",
                Price = 899.99m,
                CreatedWhenUtc = DateTime.UtcNow,
                IsDiscontinued = false
            },
            new Product
            {
                Id = 10,
                Name = "OnePlus 11 5G",
                Description = "256GB, Eternal Green, Snapdragon 8 Gen 2, 120Hz AMOLED.",
                StockQuantity = 11,
                Sku = "PHONE-004",
                Price = 799.99m,
                CreatedWhenUtc = DateTime.UtcNow,
                IsDiscontinued = false
            },
            new Product
            {
                Id = 11,
                Name = "Xiaomi 13 Pro",
                Description = "256GB, Ceramic Black, 1-inch Sony IMX989 sensor, Leica optics.",
                StockQuantity = 10,
                Sku = "PHONE-005",
                Price = 1099.99m,
                CreatedWhenUtc = DateTime.UtcNow,
                IsDiscontinued = false
            },
            new Product
            {
                Id = 12,
                Name = "Sony Xperia 1 V",
                Description = "256GB, Frosted Black, 4K OLED display, 12MP triple camera.",
                StockQuantity = 6,
                Sku = "PHONE-006",
                Price = 1199.99m,
                CreatedWhenUtc = DateTime.UtcNow,
                IsDiscontinued = false
            }
        };
    }
}