using Domain.Common;

namespace Domain.ValueObjects.Odontogram;

public sealed record OdontogramToothReference
{
    public Guid OdontogramId { get; }
    public string ToothCode { get; }

    private OdontogramToothReference(Guid odontogramId, string toothCode)
    {
        OdontogramId = odontogramId;
        ToothCode = toothCode;
    }

    public static OdontogramToothReference Create(Guid odontogramId, string toothCode)
    {
        if (odontogramId == Guid.Empty)
            throw new DomainException("Odontogram identifier is required.");

        if (string.IsNullOrWhiteSpace(toothCode))
            throw new DomainException("Tooth code cannot be empty.");

        var normalized = toothCode.Trim();

        if (normalized.Length != 2)
            throw new DomainException("Tooth code must contain exactly 2 characters.");

        if (normalized[0] is < '0' or > '9' || normalized[1] is < '0' or > '9')
            throw new DomainException("Tooth code must contain only digits.");

        return new OdontogramToothReference(odontogramId, normalized);
    }
}
