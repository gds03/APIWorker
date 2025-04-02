using FluentResults;

namespace Domain.ValueObjects.Payment;


public readonly record struct CardNumber
{ 
    public string Value { get; }

    private CardNumber(string value) => Value = value;
    
    
    public static implicit operator string(CardNumber card) => card.Value;
    public override string ToString() => Value;

    public static Result<CardNumber> Create(string? number)
    {
        if (string.IsNullOrWhiteSpace(number))
        {
            return Result.Fail<CardNumber>("number cannot be empty.");
        }
        
        // Check if the card starts with '4' and has a valid length (13 to 19 digits)
        if (!number.StartsWith("4") || number.Length < 13 || number.Length > 19)
        {
            return Result.Fail<CardNumber>("number isn't valid");
        }
        
        return LuhnCheck(number)
            ? Result.Ok<CardNumber>(new CardNumber(number))
            : Result.Fail<CardNumber>("number isn't valid");
    }
    
    private static bool LuhnCheck(string cardNumber)
    {
        int sum = 0;
        bool alternate = false;
        
        for (int i = cardNumber.Length - 1; i >= 0; i--)
        {
            if (!char.IsDigit(cardNumber[i])) return false; // Invalid character
            
            int digit = cardNumber[i] - '0';
            
            if (alternate)
            {
                digit *= 2;
                if (digit > 9)
                    digit -= 9;
            }
            
            sum += digit;
            alternate = !alternate;
        }
        
        return (sum % 10 == 0);
    }
}

