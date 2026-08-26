namespace Application.DTOs.Disponibilidades
{
    public class CreateDisponibilidadDto
    {
        public Guid ProfesionalId { get; set; }

        public int DiaInicio { get; set; }

        public int DiaFin { get; set; }

        public string HoraInicio { get; set; } = string.Empty;

        public string HoraAlmuerzo { get; set; } = string.Empty;

        public string HoraRetorno { get; set; } = string.Empty;

        public string HoraFin { get; set; } = string.Empty;
    }
}
