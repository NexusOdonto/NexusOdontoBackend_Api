namespace Domain.ValueObjects;

public sealed record NombreRol
{
    public const int MaxLength = 100;

    private NombreRol(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static NombreRol Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("El nombre del rol es obligatorio.", nameof(value));
        }

        var normalizedValue = value.Trim();

        if (normalizedValue.Length > MaxLength)
        {
            throw new ArgumentException(
                $"El nombre del rol no puede superar los {MaxLength} caracteres.",
                nameof(value));
        }

        return new NombreRol(normalizedValue);
    }

    public override string ToString() => Value;
}
