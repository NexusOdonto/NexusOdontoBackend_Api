using System.Net.Mail;

namespace Domain.ValueObjects;

public sealed record Correo
{
    public const int MaxLength = 150;

    private Correo(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static Correo Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("El correo electrónico es obligatorio.", nameof(value));
        }

        var normalizedValue = value.Trim().ToLowerInvariant();

        if (normalizedValue.Length > MaxLength)
        {
            throw new ArgumentException(
                $"El correo electrónico no puede superar los {MaxLength} caracteres.",
                nameof(value));
        }

        if (!MailAddress.TryCreate(normalizedValue, out var mailAddress) ||
            !string.Equals(mailAddress.Address, normalizedValue, StringComparison.OrdinalIgnoreCase))
        {
            throw new ArgumentException("El correo electrónico no tiene un formato válido.", nameof(value));
        }

        return new Correo(normalizedValue);
    }

    public override string ToString() => Value;
}
