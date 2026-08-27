using Domain.Common;

namespace Domain.ValueObjects.People;

public sealed record EmergencyContact
{
    public string? Name { get; }
    public string? Phone { get; }

    private EmergencyContact(string? name, string? phone)
    {
        Name = name;
        Phone = phone;
    }

    public static EmergencyContact Create(string? name, string? phone)
    {
        var normalizedName = string.IsNullOrWhiteSpace(name)
            ? null
            : name.Trim();

        var normalizedPhone = string.IsNullOrWhiteSpace(phone)
            ? null
            : phone.Trim();

        if (normalizedName is null && normalizedPhone is null)
            throw new DomainException("Emergency contact must contain a name or phone number.");

        if (normalizedName?.Length > 150)
            throw new DomainException("Emergency contact name cannot exceed 150 characters.");

        if (normalizedPhone?.Length > 30)
            throw new DomainException("Emergency phone cannot exceed 30 characters.");

        return new EmergencyContact(normalizedName, normalizedPhone);
    }
}
