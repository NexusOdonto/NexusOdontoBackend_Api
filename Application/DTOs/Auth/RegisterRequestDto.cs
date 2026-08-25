namespace Application.DTOs.Auth
{
    public sealed class RegisterRequestDto
    {
        public string Nombre { get; set; } = string.Empty;

        public string Correo { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public Guid RolId { get; set; }
    }
}
