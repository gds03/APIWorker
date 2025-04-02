using Domain.ValueObjects.Payment;

namespace Domain.Tests.ValueObjects.Payment;

public class VisaCardTests
{
    [Fact]
    public void Create_ValidCard_ShouldReturnSuccess()
    {
        // Arrange
        string validCardNumber = "4111111111111111";
        int validExpirationMonth = 12;
        int validExpirationYear = 2025;
        string validCVV = "123";

        // Act
        var result = VisaCard.Create(validCardNumber, validExpirationMonth, validExpirationYear, validCVV);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(validCardNumber, result.Value.CardNumber);
        Assert.Equal(validExpirationMonth, result.Value.ExpirationMonth);
        Assert.Equal(validExpirationYear, result.Value.ExpirationYear);
        Assert.Equal(validCVV, result.Value.Cvv);
    }

    [Fact]
    public void Create_InvalidCardNumber_ShouldReturnFailure()
    {
        // Arrange
        string invalidCardNumber = "1234567890123456";
        int validExpirationMonth = 12;
        int validExpirationYear = 2025;
        string validCVV = "123";

        // Act
        var result = VisaCard.Create(invalidCardNumber, validExpirationMonth, validExpirationYear, validCVV);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains("number isn't valid for a visa card.", result.Errors.Select(e => e.Message));
    }

    [Fact]
    public void Create_ExpiredCard_ShouldReturnFailure()
    {
        // Arrange
        string validCardNumber = "4111111111111111";
        int expiredMonth = 1;
        int expiredYear = 2020;
        string validCVV = "123";

        // Act
        var result = VisaCard.Create(validCardNumber, expiredMonth, expiredYear, validCVV);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains("Expiration date is invalid.", result.Errors.Select(e => e.Message));
    }

    [Fact]
    public void Create_InvalidCVV_ShouldReturnFailure()
    {
        // Arrange
        string validCardNumber = "4111111111111111";
        int validExpirationMonth = 12;
        int validExpirationYear = 2025;
        string invalidCVV = "12"; // Invalid CVV

        // Act
        var result = VisaCard.Create(validCardNumber, validExpirationMonth, validExpirationYear, invalidCVV);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains("CVV code is invalid.", result.Errors.Select(e => e.Message));
    }

    [Fact]
    public void Create_EmptyCardNumber_ShouldReturnFailure()
    {
        // Arrange
        string invalidCardNumber = "";
        int validExpirationMonth = 12;
        int validExpirationYear = 2025;
        string validCVV = "123";

        // Act
        var result = VisaCard.Create(invalidCardNumber, validExpirationMonth, validExpirationYear, validCVV);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains("number cannot be empty.", result.Errors.Select(e => e.Message));
    }

    [Fact]
    public void Create_NullCardNumber_ShouldReturnFailure()
    {
        // Arrange
        string? invalidCardNumber = null;
        int validExpirationMonth = 12;
        int validExpirationYear = 2025;
        string validCVV = "123";

        // Act
        var result = VisaCard.Create(invalidCardNumber, validExpirationMonth, validExpirationYear, validCVV);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains("number cannot be empty.", result.Errors.Select(e => e.Message));
    }

    [Fact]
    public void Create_InvalidLuhnCheck_ShouldReturnFailure()
    {
        // Arrange
        string invalidCardNumber = "4111111111111112"; // Luhn check fails
        int validExpirationMonth = 12;
        int validExpirationYear = 2025;
        string validCVV = "123";

        // Act
        var result = VisaCard.Create(invalidCardNumber, validExpirationMonth, validExpirationYear, validCVV);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains("number is not valid", result.Errors.Select(e => e.Message));
    }

    [Fact]
    public void Create_ValidAmexCard_ShouldReturnFailure()
    {
        // Arrange
        string validCardNumber = "378282246310005"; // American Express card
        int validExpirationMonth = 12;
        int validExpirationYear = 2025;
        string validCVV = "1234"; // Amex uses a 4-digit CVV

        // Act
        var result = VisaCard.Create(validCardNumber, validExpirationMonth, validExpirationYear, validCVV);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains( "number isn't valid for a visa card.", result.Errors.Select(e => e.Message));
    }
}