using Domain.Common;

namespace Domain.Entities;

public class Persona : BaseEntity
{
    public Guid TipoDocumentoId { get; set; }
    public string NumeroDocumento { get; set; } = null!;
    public string Nombres { get; set; } = null!;
    public string Apellidos { get; set; } = null!;
    public DateTime? FechaNacimiento { get; set; }
    public Guid? SexoId { get; set; }
    public string? Telefono { get; set; }
    public string? Email { get; set; }
    public string? Direccion { get; set; }

    // Navegacion hacia el dependiente (1:1, lado "uno" opcional)
    public virtual Paciente? Paciente { get; set; }
}

