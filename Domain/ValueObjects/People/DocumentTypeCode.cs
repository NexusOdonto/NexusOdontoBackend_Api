using Domain.Common;

namespace Domain.ValueObjects.People;

public sealed record DocumentTypeCode
{
    public string Value { get; }

    private DocumentTypeCode(string value)
    {
        Value = value;
    }

    public static DocumentTypeCode Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("Document type code cannot be empty.");

        var normalized = value.Trim();

        if (normalized.Length > 20)
            throw new DomainException("Document type code cannot exceed 20 characters.");

        return new DocumentTypeCode(normalized);
    }

    public override string ToString() => Value;
}
