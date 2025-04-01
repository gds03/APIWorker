namespace Domain.ValueObjects;


public readonly record struct Error
{
    private string Value { get; }

    public Error(string errorMessage)
    {
        if (string.IsNullOrWhiteSpace(errorMessage))
            throw new ArgumentException("errorMessage cannot be empty.");
        
        Value = errorMessage;
    }
    public static implicit operator string(Error errorMessage) => errorMessage.Value;
    public override string ToString() => Value;
}
