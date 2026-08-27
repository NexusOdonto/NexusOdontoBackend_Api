using Domain.Common;

namespace Domain.ValueObjects.Odontogram;

public sealed record DentitionTypeCode
{
    public string Value { get; }

    private DentitionTypeCode(string value)
    {
        Value = value;
    }

    public static DentitionTypeCode Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("Dentition type code cannot be empty.");

        var normalized = value.Trim();

        if (normalized.Length > 50)
            throw new DomainException("Dentition type code cannot exceed 50 characters.");

        return new DentitionTypeCode(normalized);
    }

    public override string ToString() => Value;
}
