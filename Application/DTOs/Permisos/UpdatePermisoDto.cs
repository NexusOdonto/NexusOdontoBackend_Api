namespace Application.DTOs.Permisos
{
    public class UpdatePermisoDto
    {
        public string Modulo { get; set; } = string.Empty;

        public Guid AccionPermisoId { get; set; }

        public string? Descripcion { get; set; }
    }
}
