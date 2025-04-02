using System.Text.RegularExpressions;
using FluentResults;

namespace Domain.ValueObjects.Account;

public readonly record struct AccountId
{
    private static readonly Regex Pattern = new(@"^\d{4}-[A-Za-z]{8}-\d{2}$", RegexOptions.Compiled);
    
    public string Value { get; }
    
    private AccountId(string value) => Value = value;
    
    public static implicit operator string(AccountId accountId) => accountId.Value;
    public override string ToString() => Value;

    public static Result<AccountId> Create(string? value)
    {
        List<Error> errors = [];

        if (string.IsNullOrWhiteSpace(value))
            errors.Add("AccountId cannot be empty.");

        else if (!Pattern.IsMatch(value))
            errors.Add("Invalid format. Expected format: 1234-ABCDEFGH-12");

        return errors.Any()
            ? Result.Fail<AccountId>(errors.Select(e => e.ToString()))
            : Result.Ok(new AccountId(value!));
    }
}
