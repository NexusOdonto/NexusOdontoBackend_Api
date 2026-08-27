using Domain.Common;

namespace Domain.ValueObjects.Security;

public sealed record AuditEventTypeCode
{
    public string Value { get; }

    private AuditEventTypeCode(string value)
    {
        Value = value;
    }

    public static AuditEventTypeCode Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new DomainException("Audit event type code cannot be empty.");
        }

        var normalized = value.Trim();

        if (normalized.Length > 60)
        {
            throw new DomainException(
                "Audit event type code cannot exceed 60 characters.");
        }

        return new AuditEventTypeCode(normalized);
    }

    public override string ToString() => Value;
}
