using FluentResults;

namespace Domain.ValueObjects.Payment;

public readonly record struct VisaCard
{
    public string CardNumber { get; }
    public int ExpirationMonth { get; }
    public int ExpirationYear { get; }
    public int Cvv { get; }

    private VisaCard(string cardNumber, int expirationMonth, int expirationYear, int cvv)
    {
        CardNumber = cardNumber;
        ExpirationMonth = expirationMonth;
        ExpirationYear = expirationYear;
        Cvv = cvv;
    }
    
    public static implicit operator string(VisaCard validCard) => validCard.CardNumber;

    public override string ToString() => CardNumber;

    public static Result<VisaCard> Create(string? cardNumber, int? expirationMonth, int? expirationYear, int? cvv)
    {
        List<Error> errors = new();
        if (!expirationMonth.HasValue || !expirationYear.HasValue || !IsValidExpiration(expirationMonth.Value, expirationYear.Value))
        {
            errors.Add("Expiration date is invalid.");
        }
        
        if (cvv is null or < 1 or > 999)
        {
            errors.Add("CVV code is invalid.");
        }
        
        if (string.IsNullOrWhiteSpace(cardNumber))
        {
            errors.Add("number cannot be empty.");
        }        
        
        // Check if the card starts with '4' and has a valid length (13 to 19 digits)
        if (!string.IsNullOrWhiteSpace(cardNumber) && (!cardNumber.StartsWith("4") || cardNumber.Length < 13 || cardNumber.Length > 19))
        {
            errors.Add("number isn't valid for a visa card.");
        }       

        if (!string.IsNullOrWhiteSpace(cardNumber) && !IsLuhnValid(cardNumber))
        {
            errors.Add("number is not valid");
        }

        if (errors.Any())
        {
            return Result.Fail<VisaCard>(errors.Select(e => e.ToString()));
        }

        return Result.Ok(new VisaCard(cardNumber!, expirationMonth!.Value, expirationYear!.Value, cvv!.Value));
    }
    
    private static bool IsValidExpiration(int cardMonth, int cardYear)
    {
        // Expiration date should not be in the past
        if (cardMonth < 1 || cardMonth > 12)
            return false;

        var currentYear = DateTime.Now.Year;
        var currentMonth = DateTime.Now.Month;

        if (cardYear > currentYear || (cardYear == currentYear && cardMonth >= currentMonth))
            return true;

        return false;
    }
    
    private static bool IsLuhnValid(string cardNumber)
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