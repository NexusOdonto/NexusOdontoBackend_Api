namespace Application.DTOs.Disponibilidades
{
    public class UpdateDisponibilidadDto
    {
        public int DiaInicio { get; set; }

        public int DiaFin { get; set; }

        public string HoraInicio { get; set; } = string.Empty;

        public string HoraAlmuerzo { get; set; } = string.Empty;

        public string HoraRetorno { get; set; } = string.Empty;

        public string HoraFin { get; set; } = string.Empty;

        public bool Activo { get; set; }
    }
}
