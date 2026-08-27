using Domain.Common;

namespace Domain.ValueObjects.Security;

public sealed record RoleCode
{
    public string Value { get; }

    private RoleCode(string value)
    {
        Value = value;
    }

    public static RoleCode Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new DomainException("Role code cannot be empty.");
        }

        var normalized = value.Trim();

        if (normalized.Length > 50)
        {
            throw new DomainException(
                "Role code cannot exceed 50 characters.");
        }

        return new RoleCode(normalized);
    }

    public override string ToString() => Value;
}
