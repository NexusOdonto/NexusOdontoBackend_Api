using Domain.Common;

namespace Domain.ValueObjects.Clinical;

public sealed record ClinicalAttentionAt
{
    public DateTime Value { get; }

    private ClinicalAttentionAt(DateTime value)
    {
        Value = value;
    }

    public static ClinicalAttentionAt Create(DateTime value)
    {
        if (value == default)
            throw new DomainException("Clinical attention date is required.");

        return new ClinicalAttentionAt(value);
    }

    public override string ToString() => Value.ToString("O");
}
