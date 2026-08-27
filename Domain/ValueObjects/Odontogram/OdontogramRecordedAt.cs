using Domain.Common;

namespace Domain.ValueObjects.Odontogram;

public sealed record OdontogramRecordedAt
{
    public DateTime Value { get; }

    private OdontogramRecordedAt(DateTime value)
    {
        Value = value;
    }

    public static OdontogramRecordedAt Create(DateTime value)
    {
        if (value == default)
            throw new DomainException("Odontogram record date is required.");

        return new OdontogramRecordedAt(value);
    }

    public override string ToString() => Value.ToString("O");
}
