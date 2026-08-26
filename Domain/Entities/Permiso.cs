using Domain.Common;
using Domain.ValueObjects;

namespace Domain.Entities;

public sealed class Permiso : BaseEntity
{
    public ModuloPermiso Modulo { get; private set; } = null!;

    public bool PuedeCrear { get; private set; }

    public bool PuedeEditar { get; private set; }

    public bool PuedeVer { get; private set; }

    public bool PuedeEliminar { get; private set; }

    public ICollection<Rol> Roles { get; private set; } = [];

    private Permiso()
    {
    }

    public Permiso(
        string modulo,
        bool puedeCrear,
        bool puedeEditar,
        bool puedeVer,
        bool puedeEliminar)
    {
        Modulo = ModuloPermiso.Create(modulo);
        PuedeCrear = puedeCrear;
        PuedeEditar = puedeEditar;
        PuedeVer = puedeVer;
        PuedeEliminar = puedeEliminar;
    }

    public void Actualizar(
        string modulo,
        bool puedeCrear,
        bool puedeEditar,
        bool puedeVer,
        bool puedeEliminar)
    {
        FechaActualizacion = DateTime.UtcNow;
        Modulo = ModuloPermiso.Create(modulo);
        PuedeCrear = puedeCrear;
        PuedeEditar = puedeEditar;
        PuedeVer = puedeVer;
        PuedeEliminar = puedeEliminar;
    }
}
