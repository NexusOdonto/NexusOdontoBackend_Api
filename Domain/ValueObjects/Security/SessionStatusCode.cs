using Domain.Common;

namespace Domain.ValueObjects.Security;

public sealed record SessionStatusCode
{
    public string Value { get; }

    private SessionStatusCode(string value)
    {
        Value = value;
    }

    public static SessionStatusCode Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new DomainException("Session status code cannot be empty.");
        }

        var normalized = value.Trim();

        if (normalized.Length > 50)
        {
            throw new DomainException(
                "Session status code cannot exceed 50 characters.");
        }

        return new SessionStatusCode(normalized);
    }

    public override string ToString() => Value;
}
