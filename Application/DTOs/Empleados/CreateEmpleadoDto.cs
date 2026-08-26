namespace Application.DTOs.Empleados
{
    public class CreateEmpleadoDto
    {
        public Guid PersonaId { get; set; }

        public Guid CargoId { get; set; }

        public DateTime FechaVinculacion { get; set; }
    }
}
