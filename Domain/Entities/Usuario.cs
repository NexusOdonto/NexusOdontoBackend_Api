using Domain.Common;
using Domain.ValueObjects;

namespace Domain.Entities;

public sealed class Usuario : BaseEntity
{
    public NombreUsuario Nombre { get; private set; } = null!;

    public Correo Correo { get; private set; } = null!;

    public PasswordHash PasswordHash { get; private set; } = null!;

    public Guid RolId { get; private set; }

    public Rol Rol { get; private set; } = null!;

    private Usuario()
    {
    }

    public Usuario(string nombre, string correo, string passwordHash, Guid rolId)
    {
        Nombre = NombreUsuario.Create(nombre);
        Correo = Domain.ValueObjects.Correo.Create(correo);
        PasswordHash = Domain.ValueObjects.PasswordHash.Create(passwordHash);
        RolId = ValidateRolId(rolId);
    }

    public void Actualizar(string nombre, string correo, Guid rolId)
    {
        FechaActualizacion = DateTime.UtcNow;
        Nombre = NombreUsuario.Create(nombre);
        Correo = Domain.ValueObjects.Correo.Create(correo);
        RolId = ValidateRolId(rolId);
    }

    private static Guid ValidateRolId(Guid rolId)
        => rolId != Guid.Empty
            ? rolId
            : throw new ArgumentException(
                "RolId no puede ser un GUID vacío.",
                nameof(rolId));
}
