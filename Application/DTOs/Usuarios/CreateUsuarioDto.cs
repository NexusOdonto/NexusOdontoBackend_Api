namespace Application.DTOs.Usuarios
{
    public class CreateUsuarioDto
    {
        public Guid PersonaId { get; set; }

        public string Password { get; set; } = string.Empty;
    }
}
