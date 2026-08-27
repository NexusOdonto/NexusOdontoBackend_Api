using Domain.Common;

namespace Domain.ValueObjects.Security;

public sealed record RefreshTokenHash
{
    public string Value { get; }

    private RefreshTokenHash(string value)
    {
        Value = value;
    }

    public static RefreshTokenHash Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new DomainException("Refresh token hash cannot be empty.");
        }

        if (value.Length > 512)
        {
            throw new DomainException(
                "Refresh token hash cannot exceed 512 characters.");
        }

        return new RefreshTokenHash(value);
    }

    public override string ToString() => "[PROTECTED]";
}
