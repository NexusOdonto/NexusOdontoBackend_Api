using Domain.Common;

namespace Domain.Entities;

public class Profesional : BaseEntity
{
    // FK escalar: la entidad Empleado pertenece a otra tarjeta
    public long EmpleadoId { get; set; }
    public string RegistroProfesional { get; set; } = null!;
}
