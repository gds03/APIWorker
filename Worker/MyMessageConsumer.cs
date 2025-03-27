using Contracts;
using MassTransit;

public class MyMessageConsumer : IConsumer<SendMessage>
{
    public Task Consume(ConsumeContext<SendMessage> context)
    {
        Console.WriteLine($"Received: {context.Message.Time}");
        return Task.CompletedTask;
    }
}