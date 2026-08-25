namespace Domain.ValueObjects;

public sealed record NombreUsuario
{
    public const int MaxLength = 100;

    private NombreUsuario(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static NombreUsuario Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("El nombre del usuario es obligatorio.", nameof(value));
        }

        var normalizedValue = value.Trim();

        if (normalizedValue.Length > MaxLength)
        {
            throw new ArgumentException(
                $"El nombre del usuario no puede superar los {MaxLength} caracteres.",
                nameof(value));
        }

        return new NombreUsuario(normalizedValue);
    }

    public override string ToString() => Value;
}
