using Domain.Common;

namespace Domain.ValueObjects.Security;

public sealed record AuditEventDescription
{
    public string Value { get; }

    private AuditEventDescription(string value)
    {
        Value = value;
    }

    public static AuditEventDescription Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new DomainException("Audit event description cannot be empty.");
        }

        var normalized = value.Trim();

        if (normalized.Length > 1000)
        {
            throw new DomainException(
                "Audit event description cannot exceed 1000 characters.");
        }

        return new AuditEventDescription(normalized);
    }

    public override string ToString() => Value;
}
