using Domain.Common;
using Domain.ValueObjects;

namespace Domain.Entities;

public sealed class Rol : BaseEntity
{
    private Rol()
    {
    }

    public Rol(string nombre)
    {
        Nombre = NombreRol.Create(nombre);
    }

    public NombreRol Nombre { get; private set; } = null!;

    public ICollection<Usuario> Usuarios { get; private set; } = [];

    public ICollection<Permiso> Permisos { get; private set; } = [];

    public void CambiarNombre(string nombre)
    {
        Nombre = NombreRol.Create(nombre);
    }

    public void AgregarPermiso(Permiso permiso)
    {
        ArgumentNullException.ThrowIfNull(permiso);

        if (!Permisos.Contains(permiso))
        {
            Permisos.Add(permiso);
        }
    }

    public void QuitarPermiso(Permiso permiso)
    {
        ArgumentNullException.ThrowIfNull(permiso);
        Permisos.Remove(permiso);
    }
}
