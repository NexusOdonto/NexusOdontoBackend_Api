using Domain.Common;

namespace Domain.ValueObjects.Odontogram;

public sealed record FindingStatusCode
{
    public string Value { get; }

    private FindingStatusCode(string value)
    {
        Value = value;
    }

    public static FindingStatusCode Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("Finding status code cannot be empty.");

        var normalized = value.Trim();

        if (normalized.Length > 50)
            throw new DomainException("Finding status code cannot exceed 50 characters.");

        return new FindingStatusCode(normalized);
    }

    public override string ToString() => Value;
}
