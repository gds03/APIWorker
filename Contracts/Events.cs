namespace Contracts;

public record OrderPlaced(Guid OrderId, string Status, string RequestSku, int RequestAmount, DateTime UtcNow);