namespace Application.DTOs.Roles
{
    public class CreateRolDto
    {
        public string Codigo { get; set; } = string.Empty;

        public string Nombre { get; set; } = string.Empty;

        public string? Descripcion { get; set; }
    }
}
