using System.Text.RegularExpressions;
using FluentResults;

namespace Domain.ValueObjects.Order;


public readonly record struct OrderIdentifier
{
    private static readonly Regex Pattern = new(@"^\d{3}-\d{7}-\d{7}$", RegexOptions.Compiled);
    
    public string Value { get; }

    private OrderIdentifier(string value) => Value = value;
    
    public static implicit operator string(OrderIdentifier accountId) => accountId.Value;
    public override string ToString() => Value;

    public static Result<OrderIdentifier> Create(string? value)
    {
        List<Error> errors = [];

        if (string.IsNullOrWhiteSpace(value))
            errors.Add("OrderIdentifier cannot be empty.");

        else if (!Pattern.IsMatch(value))
            errors.Add("Invalid format. Expected format: 123-1234567-1234567");

        return errors.Any()
            ? Result.Fail<OrderIdentifier>(errors.Select(e => e.ToString()))
            : Result.Ok(new OrderIdentifier(value!));
    }
}
