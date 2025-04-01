using Domain.ValueObjects.Payment;

namespace Domain.Tests.ValueObjects.Payment;

public class PaymentStatusTests
{
    [Theory]
    [InlineData(PaymentStatusEnum.Pending, "Pending")]
    [InlineData(PaymentStatusEnum.PaymentReceived, "PaymentReceived")]
    [InlineData(PaymentStatusEnum.PaymentRejected, "PaymentRejected")]
    [InlineData(PaymentStatusEnum.Payed, "Payed")]
    public void Create_ValidOrderStatusEnum_ReturnsCorrectValue(PaymentStatusEnum paymentStatusEnum, string expectedString)
    {
        // Act
        var orderStatus = new PaymentStatus(paymentStatusEnum);

        // Assert
        Assert.Equal(expectedString, orderStatus.Value);
    }

    [Fact]
    public void Equals_SameValue_ReturnsTrue()
    {
        // Arrange
        var status1 = new PaymentStatus(PaymentStatusEnum.Pending);
        var status2 = new PaymentStatus(PaymentStatusEnum.Pending);

        // Act & Assert
        Assert.Equal(status1, status2);
        Assert.True(status1 == status2);   // Check equality operator
        Assert.False(status1 != status2);  // Check inequality operator
    }

    [Fact]
    public void Equals_DifferentValue_ReturnsFalse()
    {
        // Arrange
        var status1 = new PaymentStatus(PaymentStatusEnum.Pending);
        var status2 = new PaymentStatus(PaymentStatusEnum.Payed);

        // Act & Assert
        Assert.NotEqual(status1, status2);
        Assert.False(status1 == status2);
        Assert.True(status1 != status2);
    }

    [Fact]
    public void ImplicitConversion_ToString_ReturnsValue()
    {
        // Arrange
        var orderStatus = new PaymentStatus(PaymentStatusEnum.PaymentReceived);

        // Act
        string stringValue = orderStatus;

        // Assert
        Assert.Equal("PaymentReceived", stringValue);
    }

    [Fact]
    public void ToString_ReturnsExpectedValue()
    {
        // Arrange
        var orderStatus = new PaymentStatus(PaymentStatusEnum.PaymentRejected);

        // Act & Assert
        Assert.Equal("PaymentRejected", orderStatus.ToString());
    }
}
