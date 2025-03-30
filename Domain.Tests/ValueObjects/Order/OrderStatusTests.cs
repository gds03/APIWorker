using Domain.ValueObjects.Orders;

namespace Domain.Tests.ValueObjects.Order;

public class OrderStatusTests
{
    [Theory]
    [InlineData(OrderStatusEnum.Pending, "Pending")]
    [InlineData(OrderStatusEnum.PaymentReceived, "PaymentReceived")]
    [InlineData(OrderStatusEnum.PaymentRejected, "PaymentRejected")]
    [InlineData(OrderStatusEnum.Payed, "Payed")]
    public void Create_ValidOrderStatusEnum_ReturnsCorrectValue(OrderStatusEnum orderStatusEnum, string expectedString)
    {
        // Act
        var orderStatus = new OrderStatus(orderStatusEnum);

        // Assert
        Assert.Equal(expectedString, orderStatus.Value);
    }

    [Fact]
    public void Equals_SameValue_ReturnsTrue()
    {
        // Arrange
        var status1 = new OrderStatus(OrderStatusEnum.Pending);
        var status2 = new OrderStatus(OrderStatusEnum.Pending);

        // Act & Assert
        Assert.Equal(status1, status2);
        Assert.True(status1 == status2);   // Check equality operator
        Assert.False(status1 != status2);  // Check inequality operator
    }

    [Fact]
    public void Equals_DifferentValue_ReturnsFalse()
    {
        // Arrange
        var status1 = new OrderStatus(OrderStatusEnum.Pending);
        var status2 = new OrderStatus(OrderStatusEnum.Payed);

        // Act & Assert
        Assert.NotEqual(status1, status2);
        Assert.False(status1 == status2);
        Assert.True(status1 != status2);
    }

    [Fact]
    public void ImplicitConversion_ToString_ReturnsValue()
    {
        // Arrange
        var orderStatus = new OrderStatus(OrderStatusEnum.PaymentReceived);

        // Act
        string stringValue = orderStatus;

        // Assert
        Assert.Equal("PaymentReceived", stringValue);
    }

    [Fact]
    public void ToString_ReturnsExpectedValue()
    {
        // Arrange
        var orderStatus = new OrderStatus(OrderStatusEnum.PaymentRejected);

        // Act & Assert
        Assert.Equal("PaymentRejected", orderStatus.ToString());
    }
}
