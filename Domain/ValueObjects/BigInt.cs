using FluentResults;

namespace Domain.ValueObjects;

public readonly record struct BigInt
{
    public long Value { get; }

    private BigInt(long value) => Value = value;
    
    public static implicit operator long(BigInt bigInt) => bigInt.Value;
    public static implicit operator BigInt(long value) => Create(value).Value;
    public override string ToString() => Value.ToString();

    public static Result<BigInt> Create(long value)
    {
        if (value < 1)
        {
            return Result.Fail<BigInt>($"BigInt cannot empty.");
        }

        return Result.Ok(new BigInt(value));
    }
}
