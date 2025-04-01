namespace API.Features.Orders.PlaceOrder;

public interface IOrderIdentifierGenerator
{
    string Generate();
}

public class OrderIdentifierGenerator : IOrderIdentifierGenerator
{
    private static Random random = new();
    
    public string Generate()
    {
        int regionId = random.Next(100, 999); 

        // Generate a timestamp-based identifier (7 digits)
        long timestampPart = DateTimeOffset.UtcNow.ToUnixTimeSeconds() % 10000000;

        // Generate a random unique sequence (7 digits)
        int randomPart = random.Next(1000000, 9999999); 

        return $"{regionId}-{timestampPart}-{randomPart}";
    }
}