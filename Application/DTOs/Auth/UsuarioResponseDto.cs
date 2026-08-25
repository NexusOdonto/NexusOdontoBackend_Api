namespace Application.DTOs.Auth
{
    public sealed class UsuarioResponseDto
    {
        public long Id { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public string Correo { get; set; } = string.Empty;

        public long RolId { get; set; }
    }
}
