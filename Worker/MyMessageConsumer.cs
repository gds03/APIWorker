using Events;
using MassTransit;

namespace Worker;

public class MyMessageConsumer : IConsumer<OrderPlaced>
{
    private readonly ILogger<MyMessageConsumer> _logger;

    public MyMessageConsumer(ILogger<MyMessageConsumer> logger)
    {
        _logger = logger;
    }
    public Task Consume(ConsumeContext<OrderPlaced> context)
    {
        _logger.LogError("RECEIVED!!!! ");
        
        Console.WriteLine($"Received Message");
        return Task.CompletedTask;
    }
}