using Domain.Common;

namespace Domain.ValueObjects.Security;

public sealed record PasswordHash
{
    public string Value { get; }

    private PasswordHash(string value)
    {
        Value = value;
    }

    public static PasswordHash Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new DomainException("Password hash cannot be empty.");
        }

        if (value.Length > 500)
        {
            throw new DomainException(
                "Password hash cannot exceed 500 characters.");
        }

        return new PasswordHash(value);
    }

    public override string ToString() => "[PROTECTED]";
}
