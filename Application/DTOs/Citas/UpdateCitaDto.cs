namespace Application.DTOs.Citas
{
    public class UpdateCitaDto
    {
        public Guid ProfesionalId { get; set; }

        public Guid ServicioId { get; set; }

        public DateTime FechaHoraInicio { get; set; }

        public DateTime FechaHoraFin { get; set; }

        public Guid EstadoCitaId { get; set; }

        public Guid OrigenCitaId { get; set; }

        public string? MotivoConsulta { get; set; }

        public string? Observaciones { get; set; }

        public string? MotivoCancelacion { get; set; }
    }
}
