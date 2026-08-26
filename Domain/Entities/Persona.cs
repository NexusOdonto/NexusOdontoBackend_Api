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

    public Paciente? Paciente { get; set; }

    private Persona()
    {
    }

    public Persona(
        Guid tipoDocumentoId,
        string numeroDocumento,
        string nombres,
        string apellidos,
        DateTime? fechaNacimiento = null,
        Guid? sexoId = null,
        string? telefono = null,
        string? email = null,
        string? direccion = null)
    {
        TipoDocumentoId = tipoDocumentoId;
        NumeroDocumento = numeroDocumento;
        Nombres = nombres;
        Apellidos = apellidos;
        FechaNacimiento = fechaNacimiento;
        SexoId = sexoId;
        Telefono = telefono;
        Email = email;
        Direccion = direccion;
    }

    public void Actualizar(
        string nombres,
        string apellidos,
        DateTime? fechaNacimiento,
        Guid? sexoId,
        string? telefono,
        string? email,
        string? direccion)
    {
        FechaActualizacion = DateTime.UtcNow;
        Nombres = nombres;
        Apellidos = apellidos;
        FechaNacimiento = fechaNacimiento;
        SexoId = sexoId;
        Telefono = telefono;
        Email = email;
        Direccion = direccion;
    }
}
