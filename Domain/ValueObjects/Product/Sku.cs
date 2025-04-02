using Domain.Infrastructure;
using FluentResults;

namespace Domain.ValueObjects.Product;


public readonly record struct Sku
{
    public string Value { get; }
    private Sku(string value) => Value = value;

    public static implicit operator string(Sku Sku) => Sku.Value;
    public override string ToString() => Value;
    public static Result<Sku> Create(string? value)
    {
        ResultBuilder<Sku> builder = new();

        if (string.IsNullOrWhiteSpace(value))
            builder.Error("Sku cannot be empty.");
        
        return builder.Build(()=> new Sku(value));
    }
}
