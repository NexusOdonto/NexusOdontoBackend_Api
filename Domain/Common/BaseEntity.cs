namespace Domain.Common;

public abstract class BaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

    public DateTime? FechaActualizacion { get; set; } = DateTime.UtcNow;

    public bool Activo { get; set; } = true;

    protected BaseEntity()
    {
    }
}