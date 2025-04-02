using Domain.ValueObjects.Payment;
using FluentResults;

namespace Domain.Tests.ValueObjects.Payment;

public class VisaCardTests
{
 [Fact]
    public void Create_ShouldReturnSuccess_WhenValidCardDetailsProvided()
    {
        // Arrange
        string owner = "John Doe";
        string cardNumber = "4111111111111111"; // Valid Luhn number
        int expirationMonth = DateTime.Now.Month;
        int expirationYear = DateTime.Now.Year + 1;
        int cvv = 123;
        
        // Act
        Result<VisaCard> result = VisaCard.Create(owner, cardNumber, expirationMonth, expirationYear, cvv);
        
        // Assert
        Assert.True(result.IsSuccess);
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Create_ShouldReturnFailure_WhenOwnerIsInvalid(string? owner)
    {
        // Arrange
        string cardNumber = "4111111111111111";
        int expirationMonth = DateTime.Now.Month;
        int expirationYear = DateTime.Now.Year + 1;
        int cvv = 123;
        
        // Act
        Result<VisaCard> result = VisaCard.Create(owner, cardNumber, expirationMonth, expirationYear, cvv);
        
        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains("Owner is required.", result.Errors.Select(e => e.Message));
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("1234567890123456")] // Doesn't start with '4'
    [InlineData("4111111111111")] // Too short
    [InlineData("41111111111111111111")] // Too long
    public void Create_ShouldReturnFailure_WhenCardNumberIsInvalid(string? cardNumber)
    {
        // Arrange
        string owner = "John Doe";
        int expirationMonth = DateTime.Now.Month;
        int expirationYear = DateTime.Now.Year + 1;
        int cvv = 123;
        
        // Act
        Result<VisaCard> result = VisaCard.Create(owner, cardNumber, expirationMonth, expirationYear, cvv);
        
        // Assert
        Assert.False(result.IsSuccess);
    }
    
    [Theory]
    [InlineData(0, 2026)]
    [InlineData(13, 2026)]
    [InlineData(5, 2020)] // Expired
    public void Create_ShouldReturnFailure_WhenExpirationDateIsInvalid(int expirationMonth, int expirationYear)
    {
        // Arrange
        string owner = "John Doe";
        string cardNumber = "4111111111111111";
        int cvv = 123;
        
        // Act
        Result<VisaCard> result = VisaCard.Create(owner, cardNumber, expirationMonth, expirationYear, cvv);
        
        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains("Expiration date is invalid.", result.Errors.Select(e => e.Message));
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData(0)]
    [InlineData(1000)]
    public void Create_ShouldReturnFailure_WhenCvvIsInvalid(int? cvv)
    {
        // Arrange
        string owner = "John Doe";
        string cardNumber = "4111111111111111";
        int expirationMonth = DateTime.Now.Month;
        int expirationYear = DateTime.Now.Year + 1;
        
        // Act
        Result<VisaCard> result = VisaCard.Create(owner, cardNumber, expirationMonth, expirationYear, cvv);
        
        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains("CVV code is invalid.", result.Errors.Select(e => e.Message));
    }
}