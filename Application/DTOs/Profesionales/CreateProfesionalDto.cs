namespace Application.DTOs.Profesionales
{
    public class CreateProfesionalDto
    {
        public Guid EmpleadoId { get; set; }

        public string RegistroProfesional { get; set; } = string.Empty;
    }
}
