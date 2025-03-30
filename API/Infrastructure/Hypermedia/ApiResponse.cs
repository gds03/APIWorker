namespace API.Infrastructure.Hypermedia;

public abstract record ApiResponse
{
    public string? Description { get; set; }
    public List<Operation> Operations { get; } = [];
}