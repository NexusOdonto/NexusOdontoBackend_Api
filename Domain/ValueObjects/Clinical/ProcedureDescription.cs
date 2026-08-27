using Domain.Common;

namespace Domain.ValueObjects.Clinical;

public sealed record ProcedureDescription
{
    public string Value { get; }

    private ProcedureDescription(string value)
    {
        Value = value;
    }

    public static ProcedureDescription Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("Procedure description cannot be empty.");

        return new ProcedureDescription(value.Trim());
    }

    public override string ToString() => Value;
}
