namespace Application.DTOs.Empleados
{
    public class UpdateEmpleadoDto
    {
        public Guid CargoId { get; set; }

        public DateTime FechaVinculacion { get; set; }

        public bool Activo { get; set; }
    }
}
