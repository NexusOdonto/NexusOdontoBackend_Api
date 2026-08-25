using Domain.Common;

namespace Domain.Entities;

public class Profesional : BaseEntity
{
    public Guid EmpleadoId { get; set; }
    public string RegistroProfesional { get; set; } = null!;
}
