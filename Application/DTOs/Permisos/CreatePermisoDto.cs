namespace Application.DTOs.Permisos
{
    public class CreatePermisoDto
    {
        public string Modulo { get; set; } = string.Empty;

        public Guid AccionPermisoId { get; set; }

        public string? Descripcion { get; set; }
    }
}
