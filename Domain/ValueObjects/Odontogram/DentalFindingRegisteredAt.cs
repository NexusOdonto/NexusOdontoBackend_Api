using Domain.Common;

namespace Domain.ValueObjects.Odontogram;

public sealed record DentalFindingRegisteredAt
{
    public DateTime Value { get; }

    private DentalFindingRegisteredAt(DateTime value)
    {
        Value = value;
    }

    public static DentalFindingRegisteredAt Create(DateTime value)
    {
        if (value == default)
            throw new DomainException("Dental finding registration date is required.");

        return new DentalFindingRegisteredAt(value);
    }

    public override string ToString() => Value.ToString("O");
}
