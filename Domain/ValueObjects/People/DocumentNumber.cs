using Domain.Common;

namespace Domain.ValueObjects.People;

public sealed record DocumentNumber
{
    public string Value { get; }

    private DocumentNumber(string value)
    {
        Value = value;
    }

    public static DocumentNumber Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("Document number cannot be empty.");

        var normalized = value.Trim();

        if (normalized.Length > 30)
            throw new DomainException("Document number cannot exceed 30 characters.");

        return new DocumentNumber(normalized);
    }

    public override string ToString() => Value;
}
