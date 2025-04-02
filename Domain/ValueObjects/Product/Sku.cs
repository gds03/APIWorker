using FluentResults;

namespace Domain.ValueObjects.Product;


public readonly record struct Sku
{
    public string Value { get; }
    private Sku(string value) => Value = value;

    public static implicit operator string(Sku sku) => sku.Value;
    public override string ToString() => Value;
    public static Result<Sku> Create(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Result.Fail<Sku>($"Sku cannot be empty.");
        }

        return Result.Ok(new Sku(value));
    }
}
