using Domain.Common;

namespace Domain.ValueObjects.Clinical;

public sealed record DiagnosisDescription
{
    public string Value { get; }

    private DiagnosisDescription(string value)
    {
        Value = value;
    }

    public static DiagnosisDescription Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("Diagnosis description cannot be empty.");

        return new DiagnosisDescription(value.Trim());
    }

    public override string ToString() => Value;
}
