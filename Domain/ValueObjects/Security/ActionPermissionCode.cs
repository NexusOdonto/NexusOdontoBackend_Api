using Domain.Common;

namespace Domain.ValueObjects.Security;

public sealed record ActionPermissionCode
{
    public string Value { get; }

    private ActionPermissionCode(string value)
    {
        Value = value;
    }

    public static ActionPermissionCode Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new DomainException("Action permission code cannot be empty.");
        }

        var normalized = value.Trim();

        if (normalized.Length > 50)
        {
            throw new DomainException(
                "Action permission code cannot exceed 50 characters.");
        }

        return new ActionPermissionCode(normalized);
    }

    public override string ToString() => Value;
}
