namespace Application.DTOs.Sesiones
{
    public class SesionDto
    {
        public Guid Id { get; set; }

        public Guid UsuarioId { get; set; }

        public string? Dispositivo { get; set; }

        public string? DireccionIp { get; set; }

        public string? AgenteUsuario { get; set; }

        public DateTime FechaCreacion { get; set; }

        public DateTime FechaExpiracion { get; set; }

        public DateTime? FechaUltimoUso { get; set; }

        public DateTime? RevocadaEn { get; set; }

        public Guid EstadoSesionId { get; set; }
    }
}
