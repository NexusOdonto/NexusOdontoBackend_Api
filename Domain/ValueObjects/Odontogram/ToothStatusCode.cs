using Domain.Common;

namespace Domain.ValueObjects.Odontogram;

public sealed record ToothStatusCode
{
    public string Value { get; }

    private ToothStatusCode(string value)
    {
        Value = value;
    }

    public static ToothStatusCode Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("Tooth status code cannot be empty.");

        var normalized = value.Trim();

        if (normalized.Length > 50)
            throw new DomainException("Tooth status code cannot exceed 50 characters.");

        return new ToothStatusCode(normalized);
    }

    public override string ToString() => Value;
}
