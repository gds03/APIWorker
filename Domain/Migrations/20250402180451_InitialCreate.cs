using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Domain.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsVerified = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedWhenUtc = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Email = table.Column<string>(type: "longtext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    IsDiscontinued = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: false),
                    Description = table.Column<string>(type: "longtext", nullable: false),
                    StockQuantity = table.Column<long>(type: "bigint", nullable: false),
                    Sku = table.Column<string>(type: "longtext", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedWhenUtc = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    AccountId = table.Column<string>(type: "varchar(255)", nullable: false),
                    CreatedWhenUtc = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Identifier = table.Column<string>(type: "varchar(255)", nullable: false),
                    Total = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<string>(type: "longtext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "OrderProduct",
                columns: table => new
                {
                    OrdersId = table.Column<long>(type: "bigint", nullable: false),
                    ProductsId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderProduct", x => new { x.OrdersId, x.ProductsId });
                    table.ForeignKey(
                        name: "FK_OrderProduct_Orders_OrdersId",
                        column: x => x.OrdersId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderProduct_Products_ProductsId",
                        column: x => x.ProductsId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    OrderId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedWhenUtc = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<string>(type: "longtext", nullable: false),
                    PaymentType = table.Column<string>(type: "varchar(8)", maxLength: 8, nullable: false),
                    CardNumber = table.Column<string>(type: "longtext", nullable: true),
                    Month = table.Column<int>(type: "int", nullable: true),
                    Year = table.Column<int>(type: "int", nullable: true),
                    CVV = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "longtext", nullable: true),
                    WalletAddress = table.Column<string>(type: "longtext", nullable: true),
                    CryptoType = table.Column<string>(type: "longtext", nullable: true),
                    PayPalEmail = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payments_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "Id", "CreatedWhenUtc", "Email", "IsActive", "IsVerified" },
                values: new object[] { "9999-ZYXWVUTS-99", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@admin.com", true, true });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CreatedWhenUtc", "Description", "IsDiscontinued", "Name", "Price", "Sku", "StockQuantity" },
                values: new object[,]
                {
                    { 1L, new DateTime(2025, 4, 2, 18, 4, 51, 418, DateTimeKind.Utc).AddTicks(5648), "Powerful laptop with Intel Core i7, 16GB RAM, and 512GB SSD.", false, "Dell XPS 15", 1599.99m, "LAPTOP-001", 10L },
                    { 2L, new DateTime(2025, 4, 2, 18, 4, 51, 418, DateTimeKind.Utc).AddTicks(5651), "Apple M2 Pro chip, 16GB RAM, 512GB SSD, Retina Display.", false, "MacBook Pro 14\"", 1999.99m, "LAPTOP-002", 8L },
                    { 3L, new DateTime(2025, 4, 2, 18, 4, 51, 418, DateTimeKind.Utc).AddTicks(5653), "Business laptop with Intel Core i7, 16GB RAM, and 1TB SSD.", false, "Lenovo ThinkPad X1 Carbon", 1899.99m, "LAPTOP-003", 6L },
                    { 4L, new DateTime(2025, 4, 2, 18, 4, 51, 418, DateTimeKind.Utc).AddTicks(5655), "Gaming laptop with AMD Ryzen 9, RTX 4060, and 16GB RAM.", false, "ASUS ROG Zephyrus G14", 1799.99m, "LAPTOP-004", 5L },
                    { 5L, new DateTime(2025, 4, 2, 18, 4, 51, 418, DateTimeKind.Utc).AddTicks(5656), "2-in-1 convertible laptop with OLED display and Intel Core i7.", false, "HP Spectre x360 14", 1699.99m, "LAPTOP-005", 7L },
                    { 6L, new DateTime(2025, 4, 2, 18, 4, 51, 418, DateTimeKind.Utc).AddTicks(5658), "Budget-friendly laptop with AMD Ryzen 7, 16GB RAM, and 512GB SSD.", false, "Acer Swift 3", 799.99m, "LAPTOP-006", 12L },
                    { 7L, new DateTime(2025, 4, 2, 18, 4, 51, 418, DateTimeKind.Utc).AddTicks(5659), "128GB, Space Black, 48MP main camera, A16 Bionic chip.", false, "iPhone 14 Pro", 999.99m, "PHONE-001", 15L },
                    { 8L, new DateTime(2025, 4, 2, 18, 4, 51, 418, DateTimeKind.Utc).AddTicks(5660), "256GB, Phantom Black, 200MP camera, S Pen included.", false, "Samsung Galaxy S23 Ultra", 1199.99m, "PHONE-002", 12L },
                    { 9L, new DateTime(2025, 4, 2, 18, 4, 51, 418, DateTimeKind.Utc).AddTicks(5662), "128GB, Obsidian, Tensor G2 chip, AI-powered camera.", false, "Google Pixel 7 Pro", 899.99m, "PHONE-003", 9L },
                    { 10L, new DateTime(2025, 4, 2, 18, 4, 51, 418, DateTimeKind.Utc).AddTicks(5663), "256GB, Eternal Green, Snapdragon 8 Gen 2, 120Hz AMOLED.", false, "OnePlus 11 5G", 799.99m, "PHONE-004", 11L },
                    { 11L, new DateTime(2025, 4, 2, 18, 4, 51, 418, DateTimeKind.Utc).AddTicks(5665), "256GB, Ceramic Black, 1-inch Sony IMX989 sensor, Leica optics.", false, "Xiaomi 13 Pro", 1099.99m, "PHONE-005", 10L },
                    { 12L, new DateTime(2025, 4, 2, 18, 4, 51, 418, DateTimeKind.Utc).AddTicks(5666), "256GB, Frosted Black, 4K OLED display, 12MP triple camera.", false, "Sony Xperia 1 V", 1199.99m, "PHONE-006", 6L }
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderProduct_ProductsId",
                table: "OrderProduct",
                column: "ProductsId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_AccountId",
                table: "Orders",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_Identifier",
                table: "Orders",
                column: "Identifier");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_OrderId",
                table: "Payments",
                column: "OrderId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderProduct");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Accounts");
        }
    }
}
