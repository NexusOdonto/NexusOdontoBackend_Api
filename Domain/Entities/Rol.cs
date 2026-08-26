using Domain.Common;
using Domain.ValueObjects;

namespace Domain.Entities;

public sealed class Rol : BaseEntity
{
    public NombreRol Nombre { get; private set; } = null!;

    public ICollection<Usuario> Usuarios { get; private set; } = [];

    public ICollection<Permiso> Permisos { get; private set; } = [];

    private Rol()
    {
    }

    public Rol(string nombre)
    {
        Nombre = NombreRol.Create(nombre);
    }

    public void Actualizar(string nombre)
    {
        FechaActualizacion = DateTime.UtcNow;
        Nombre = NombreRol.Create(nombre);
    }
}
