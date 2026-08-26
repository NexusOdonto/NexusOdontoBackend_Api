namespace Application.DTOs.Usuarios
{
    public class UsuarioResponseDto
    {
        public Guid Id { get; set; }

        public Guid PersonaId { get; set; }

        public bool Activo { get; set; }

        public DateTime? UltimoAcceso { get; set; }

        public DateTime FechaCreacion { get; set; }

        public DateTime? ActualizadoEn { get; set; }
    }
}
