namespace Application.DTOs.Pacientes
{
    public class PacienteResponseDto
    {
        public Guid Id { get; set; }

        public Guid PersonaId { get; set; }

        public string? ContactoEmergencia { get; set; }

        public string? TelefonoEmergencia { get; set; }

        public bool Activo { get; set; }

        public DateTime FechaRegistro { get; set; }

        public DateTime? ActualizadoEn { get; set; }
    }
}
