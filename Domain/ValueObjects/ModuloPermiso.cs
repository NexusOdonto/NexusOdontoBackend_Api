namespace Domain.ValueObjects;

public sealed record ModuloPermiso
{
    public const int MaxLength = 80;

    private ModuloPermiso(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static ModuloPermiso Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("El módulo del permiso es obligatorio.", nameof(value));
        }

        var normalizedValue = value.Trim().ToUpperInvariant();

        if (normalizedValue.Length > MaxLength)
        {
            throw new ArgumentException(
                $"El módulo del permiso no puede superar los {MaxLength} caracteres.",
                nameof(value));
        }

        return new ModuloPermiso(normalizedValue);
    }

    public override string ToString() => Value;
}
