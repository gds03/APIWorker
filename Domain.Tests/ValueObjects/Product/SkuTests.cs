using Domain.ValueObjects.Product;

namespace Domain.Tests.ValueObjects.Product;

public class SkuTests
{
    [Fact]
    public void Create_ValidSku_ReturnsSuccess()
    {
        // Arrange
        var validSku = "SKU12345";

        // Act
        var result = Sku.Create(validSku);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(validSku, result.Value.Value);  // The value should match the input SKU
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("    ")]  // Whitespaces only
    public void Create_InvalidSku_ReturnsFailure(string invalidSku)
    {
        // Act
        var result = Sku.Create(invalidSku);

        // Assert
        Assert.True(result.IsFailed);
        Assert.Contains(result.Errors, e => e.Message == "Sku cannot be empty.");
    }

    [Fact]
    public void Equals_SameValue_ReturnsTrue()
    {
        // Arrange
        var sku1 = Sku.Create("SKU12345").Value;
        var sku2 = Sku.Create("SKU12345").Value;

        // Act & Assert
        Assert.Equal(sku1, sku2);
        Assert.True(sku1 == sku2);   // Check equality operator
        Assert.False(sku1 != sku2);  // Check inequality operator
    }

    [Fact]
    public void Equals_DifferentValue_ReturnsFalse()
    {
        // Arrange
        var sku1 = Sku.Create("SKU12345").Value;
        var sku2 = Sku.Create("SKU54321").Value;

        // Act & Assert
        Assert.NotEqual(sku1, sku2);
        Assert.False(sku1 == sku2);
        Assert.True(sku1 != sku2);
    }

    [Fact]
    public void ImplicitConversion_ToString_ReturnsValue()
    {
        // Arrange
        var sku = Sku.Create("SKU12345").Value;

        // Act
        string stringValue = sku;

        // Assert
        Assert.Equal("SKU12345", stringValue);
    }

    [Fact]
    public void ToString_ReturnsExpectedValue()
    {
        // Arrange
        var sku = Sku.Create("SKU12345").Value;

        // Act & Assert
        Assert.Equal("SKU12345", sku.ToString());
    }
}
