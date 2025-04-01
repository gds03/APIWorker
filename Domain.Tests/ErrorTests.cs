using Domain.ValueObjects;

namespace Domain.Tests;

using System;
using Xunit;

public class ErrorTests
{
    [Fact]
    public void Constructor_ShouldCreateError_WhenValidMessageIsProvided()
    {
        // Arrange
        var message = "Something went wrong";

        // Act
        var error = new Error(message);

        // Assert
        Assert.Equal(message, error.ToString());
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_ShouldThrowArgumentException_WhenMessageIsNullOrWhitespace(string invalidMessage)
    {
        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => new Error(invalidMessage));
        Assert.Equal("errorMessage cannot be empty.", ex.Message);
    }

    [Fact]
    public void ImplicitConversion_ShouldReturnStringValue()
    {
        // Arrange
        var message = "Critical error!";
        var error = new Error(message);

        // Act
        string result = error;

        // Assert
        Assert.Equal(message, result);
    }

    [Fact]
    public void ToString_ShouldReturnValue()
    {
        // Arrange
        var message = "Error occurred.";
        var error = new Error(message);

        // Act
        var result = error.ToString();

        // Assert
        Assert.Equal(message, result);
    }
}
