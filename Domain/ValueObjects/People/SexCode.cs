using Domain.Common;

namespace Domain.ValueObjects.People;

public sealed record SexCode
{
    public string Value { get; }

    private SexCode(string value)
    {
        Value = value;
    }

    public static SexCode Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("Sex code cannot be empty.");

        var normalized = value.Trim();

        if (normalized.Length > 20)
            throw new DomainException("Sex code cannot exceed 20 characters.");

        return new SexCode(normalized);
    }

    public override string ToString() => Value;
}
