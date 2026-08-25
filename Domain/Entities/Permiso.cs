using Domain.Common;
using Domain.ValueObjects;

namespace Domain.Entities;

public sealed class Permiso : BaseEntity
{
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

    public ModuloPermiso Modulo { get; private set; } = null!;

    public bool PuedeCrear { get; private set; }

    public bool PuedeEditar { get; private set; }

    public bool PuedeVer { get; private set; }

    public bool PuedeEliminar { get; private set; }

    public ICollection<Rol> Roles { get; private set; } = [];

    public void CambiarModulo(string modulo)
    {
        Modulo = ModuloPermiso.Create(modulo);
    }

    public void CambiarAcciones(
        bool puedeCrear,
        bool puedeEditar,
        bool puedeVer,
        bool puedeEliminar)
    {
        PuedeCrear = puedeCrear;
        PuedeEditar = puedeEditar;
        PuedeVer = puedeVer;
        PuedeEliminar = puedeEliminar;
    }
}
