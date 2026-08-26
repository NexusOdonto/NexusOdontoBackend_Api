using Domain.Common;

namespace Domain.Entities;

public class Paciente : BaseEntity
{
    public Guid PersonaId { get; set; }
    public string? ContactoEmergencia { get; set; }
    public string? TelefonoEmergencia { get; set; }

    public Persona? Persona { get; set; }

    private Paciente()
    {

    }

    public Paciente(Guid personaId, string? contactoEmergencia, string? telefonoEmergencia)
    {
        PersonaId = personaId;
        ContactoEmergencia = contactoEmergencia;
        TelefonoEmergencia = telefonoEmergencia;
    }

    public void Actualizar(string? contactoEmergencia, string? telefonoEmergencia)
    {
        FechaActualizacion = DateTime.UtcNow;
        ContactoEmergencia = contactoEmergencia;
        TelefonoEmergencia = telefonoEmergencia;
    }
}
