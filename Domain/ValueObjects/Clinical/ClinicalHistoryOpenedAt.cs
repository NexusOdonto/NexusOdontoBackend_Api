using Domain.Common;

namespace Domain.ValueObjects.Clinical;

public sealed record ClinicalHistoryOpenedAt
{
    public DateTime Value { get; }

    private ClinicalHistoryOpenedAt(DateTime value)
    {
        Value = value;
    }

    public static ClinicalHistoryOpenedAt Create(DateTime value)
    {
        if (value == default)
            throw new DomainException("Clinical history opening date is required.");

        return new ClinicalHistoryOpenedAt(value);
    }

    public override string ToString() => Value.ToString("O");
}
