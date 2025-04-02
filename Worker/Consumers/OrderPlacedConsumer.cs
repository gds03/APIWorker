using Events;
using MassTransit;

namespace Worker.Consumers;

public class OrderPlacedConsumer : IConsumer<OrderPlaced>
{
    private readonly ILogger<OrderPlacedConsumer> _logger;

    public OrderPlacedConsumer(ILogger<OrderPlacedConsumer> logger)
    {
        _logger = logger;
    }
    public Task Consume(ConsumeContext<OrderPlaced> context)
    {
        Console.WriteLine($"Order Created Event Received: {context.Message.OrderInfo.Identifier}");
        return Task.CompletedTask;
    }
}