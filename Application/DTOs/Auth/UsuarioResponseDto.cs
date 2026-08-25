namespace Application.DTOs.Auth
{
    public sealed class UsuarioResponseDto
    {
        public Guid Id { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public string Correo { get; set; } = string.Empty;

        public Guid RolId { get; set; }
    }
}
