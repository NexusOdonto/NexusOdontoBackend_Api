using Domain.Common;

namespace Domain.ValueObjects.Odontogram;

public sealed record FindingTypeCode
{
    public string Value { get; }

    private FindingTypeCode(string value)
    {
        Value = value;
    }

    public static FindingTypeCode Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("Finding type code cannot be empty.");

        var normalized = value.Trim();

        if (normalized.Length > 50)
            throw new DomainException("Finding type code cannot exceed 50 characters.");

        return new FindingTypeCode(normalized);
    }

    public override string ToString() => Value;
}
