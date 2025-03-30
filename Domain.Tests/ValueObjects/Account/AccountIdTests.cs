using Domain.ValueObjects.Account;

namespace Domain.Tests.ValueObjects.Account;

public class AccountIdTests
{
    [Theory]
    [InlineData("1234-ABCDEFGH-12")] // Valid format
    [InlineData("9999-ZYXWVUTS-99")] // Valid format
    public void Create_ValidAccountId_ReturnsSuccess(string validId)
    {
        // Act
        var result = AccountId.Create(validId);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(validId, result.Value.Value);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void Create_EmptyOrNull_ReturnsFailure(string invalidId)
    {
        // Act
        var result = AccountId.Create(invalidId);

        // Assert
        Assert.True(result.IsFailed);
        Assert.Contains(result.Errors, e => e.Message == "AccountId cannot be empty.");
    }

    [Theory]
    [InlineData("12-ABCDEFGH-12")]  // Too short
    [InlineData("12345-ABCDEFGH-12")] // Too long in first part
    [InlineData("1234-ABCDEFGH12")]  // Missing "-"
    [InlineData("1234-ABCDEFGH-ABC")] // Non-numeric last part
    [InlineData("1234-AB12CDEF-12")] // Numbers in letter part
    public void Create_InvalidFormat_ReturnsFailure(string invalidId)
    {
        // Act
        var result = AccountId.Create(invalidId);

        // Assert
        Assert.True(result.IsFailed);
        Assert.Contains(result.Errors, e => e.Message == "Invalid format. Expected format: 1234-ABCDEFGH-12.");
    }

    [Fact]
    public void Equals_SameValue_ReturnsTrue()
    {
        // Arrange
        var id1 = AccountId.Create("1234-ABCDEFGH-12").Value;
        var id2 = AccountId.Create("1234-ABCDEFGH-12").Value;

        // Act & Assert
        Assert.Equal(id1, id2);
        Assert.True(id1 == id2);
        Assert.False(id1 != id2);
    }

    [Fact]
    public void Equals_DifferentValue_ReturnsFalse()
    {
        // Arrange
        var id1 = AccountId.Create("1234-ABCDEFGH-12").Value;
        var id2 = AccountId.Create("5678-IJKLMNOP-34").Value;

        // Act & Assert
        Assert.NotEqual(id1, id2);
        Assert.False(id1 == id2);
        Assert.True(id1 != id2);
    }

    [Fact]
    public void ImplicitConversion_ToString_ReturnsValue()
    {
        // Arrange
        var id = AccountId.Create("1234-ABCDEFGH-12").Value;

        // Act
        string stringValue = id;

        // Assert
        Assert.Equal("1234-ABCDEFGH-12", stringValue);
    }

    [Fact]
    public void ToString_ReturnsExpectedValue()
    {
        // Arrange
        var id = AccountId.Create("1234-ABCDEFGH-12").Value;

        // Act & Assert
        Assert.Equal("1234-ABCDEFGH-12", id.ToString());
    }
}
