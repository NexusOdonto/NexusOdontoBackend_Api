namespace Application.DTOs.Usuarios;

public sealed class UpdateUsuarioRequestDto
{
    public string Nombre { get; set; } = string.Empty;

    public string Correo { get; set; } = string.Empty;

    public long RolId { get; set; }
}
