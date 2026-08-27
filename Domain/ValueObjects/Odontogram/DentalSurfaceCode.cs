using Domain.Common;

namespace Domain.ValueObjects.Odontogram;

public sealed record DentalSurfaceCode
{
    public string Value { get; }

    private DentalSurfaceCode(string value)
    {
        Value = value;
    }

    public static DentalSurfaceCode Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("Dental surface code cannot be empty.");

        var normalized = value.Trim();

        if (normalized.Length > 50)
            throw new DomainException("Dental surface code cannot exceed 50 characters.");

        return new DentalSurfaceCode(normalized);
    }

    public override string ToString() => Value;
}
