namespace API.Infrastructure.Hypermedia;

public class Operation(string operationId, HttpMethod method, string? uri)
{
    public string OperationId { get; } = !string.IsNullOrWhiteSpace(operationId) 
        ? operationId 
        : throw new ArgumentException("OperationId cannot be null or empty.", nameof(operationId));

    public Uri Uri { get; } = !string.IsNullOrWhiteSpace(uri) 
        ? new Uri(uri, UriKind.Relative) 
        : throw new ArgumentException("Uri cannot be null or empty.", nameof(uri));

    public string Method { get; } = method.ToString();
}
