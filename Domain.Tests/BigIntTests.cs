using Domain.ValueObjects;
using FluentResults;

namespace Domain.Tests;

public class BigIntTests
{
    [Fact]
    public void Create_ValidValue_ReturnsBigInt()
    {
        // Arrange
        long validValue = 100;

        // Act
        Result<BigInt> result = BigInt.Create(validValue);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(validValue, result.Value.Value);
    }

    [Fact]
    public void Create_InvalidValue_ReturnsError()
    {
        // Arrange
        long invalidValue = 0; // Less than 1

        // Act
        Result<BigInt> result = BigInt.Create(invalidValue);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains("BigInt cannot empty.", result.Errors[0].Message);
    }

    [Fact]
    public void ImplicitConversion_FromBigIntToLong_ReturnsValue()
    {
        // Arrange
        BigInt bigInt = BigInt.Create(500).Value;

        // Act
        long value = bigInt;

        // Assert
        Assert.Equal(500, value);
    }

    [Fact]
    public void ImplicitConversion_FromLongToBigInt_ReturnsBigInt()
    {
        // Arrange
        long value = 999;

        // Act
        BigInt bigInt = value; // Implicit conversion

        // Assert
        Assert.Equal(value, bigInt.Value);
    }

    [Fact]
    public void ImplicitConversion_FromLongToBigInt_UsesValidation()
    {
        // Arrange
        long invalidValue = 0;

        // Act
        Assert.Throws<InvalidOperationException>(() =>
        {
            BigInt bigInt = invalidValue;
        });
    }

    [Fact]
    public void ToString_ReturnsCorrectStringRepresentation()
    {
        // Arrange
        BigInt bigInt = BigInt.Create(1234).Value;

        // Act
        string result = bigInt.ToString();

        // Assert
        Assert.Equal("1234", result);
    }

    [Fact]
    public void Equals_SameValue_ReturnsTrue()
    {
        // Arrange
        BigInt bigInt1 = BigInt.Create(42).Value;
        BigInt bigInt2 = BigInt.Create(42).Value;

        // Act & Assert
        Assert.Equal(bigInt1, bigInt2);
        Assert.True(bigInt1 == bigInt2);
        Assert.False(bigInt1 != bigInt2);
    }

    [Fact]
    public void Equals_DifferentValues_ReturnsFalse()
    {
        // Arrange
        BigInt bigInt1 = BigInt.Create(42).Value;
        BigInt bigInt2 = BigInt.Create(99).Value;

        // Act & Assert
        Assert.NotEqual(bigInt1, bigInt2);
        Assert.False(bigInt1 == bigInt2);
        Assert.True(bigInt1 != bigInt2);
    }
}
