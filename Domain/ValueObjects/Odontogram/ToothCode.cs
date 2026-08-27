using Domain.Common;

namespace Domain.ValueObjects.Odontogram;

public sealed record ToothCode
{
    public string Value { get; }

    private ToothCode(string value)
    {
        Value = value;
    }

    public static ToothCode Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("Tooth code cannot be empty.");

        var normalized = value.Trim();

        if (normalized.Length != 2)
            throw new DomainException("Tooth code must contain exactly 2 characters.");

        if (normalized[0] is < '0' or > '9' || normalized[1] is < '0' or > '9')
            throw new DomainException("Tooth code must contain only digits.");

        var quadrant = normalized[0] - '0';
        var position = normalized[1] - '0';

        var permanent =
            quadrant is >= 1 and <= 4 &&
            position is >= 1 and <= 8;

        var temporary =
            quadrant is >= 5 and <= 8 &&
            position is >= 1 and <= 5;

        if (!permanent && !temporary)
            throw new DomainException("Invalid FDI tooth code.");

        return new ToothCode(normalized);
    }

    public override string ToString() => Value;
}
