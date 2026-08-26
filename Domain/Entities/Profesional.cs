using Domain.Common;

namespace Domain.Entities;

public class Profesional : BaseEntity
{
    public Guid EmpleadoId { get; set; }
    public string RegistroProfesional { get; set; } = null!;

    private Profesional()
    {
    }

    public Profesional(Guid empleadoId, string registroProfesional)
    {
        EmpleadoId = empleadoId;
        RegistroProfesional = registroProfesional;
    }

    public void Actualizar(string registroProfesional)
    {
        FechaActualizacion = DateTime.UtcNow;
        RegistroProfesional = registroProfesional;
    }
}
