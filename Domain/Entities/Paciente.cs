using Domain.Common;

namespace Domain.Entities;

public class Paciente : BaseEntity
{
    public Guid PersonaId { get; set; }
    public string? ContactoEmergencia { get; set; }
    public string? TelefonoEmergencia { get; set; }
}
