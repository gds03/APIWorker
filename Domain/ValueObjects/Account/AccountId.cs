using System.Text.RegularExpressions;
using Domain.Infrastructure;
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
        ResultBuilder<AccountId> builder = new();

        if (string.IsNullOrWhiteSpace(value))
            builder.Error("AccountId cannot be empty.");

        else if (!Pattern.IsMatch(value))
            builder.Error("Invalid format. Expected format: 1234-ABCDEFGH-12");

        return builder.Build(()=> new AccountId(value));
    }
}
