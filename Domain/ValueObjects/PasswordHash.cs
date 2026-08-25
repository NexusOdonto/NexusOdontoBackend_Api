namespace Domain.ValueObjects;

public sealed record PasswordHash
{
    public const int MaxLength = 500;

    private PasswordHash(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static PasswordHash Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("El hash de la contraseña es obligatorio.", nameof(value));
        }

        if (value.Length > MaxLength)
        {
            throw new ArgumentException(
                $"El hash de la contraseña no puede superar los {MaxLength} caracteres.",
                nameof(value));
        }

        return new PasswordHash(value);
    }

    public override string ToString() => Value;
}
