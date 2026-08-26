namespace Application.DTOs.Pacientes
{
    public class CreatePacienteDto
    {
        public Guid PersonaId { get; set; }

        public string? ContactoEmergencia { get; set; }

        public string? TelefonoEmergencia { get; set; }
    }
}
