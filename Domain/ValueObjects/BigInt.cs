using Domain.Infrastructure;
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
        ResultBuilder<BigInt> builder = new();

        if (value < 1 )
            builder.Error("BoundedInt cannot be lower than 1");
        
        return builder.Build(() => new BigInt(value));
    }
}
