using Domain.Common;
using Domain.ValueObjects;

namespace Domain.Entities;

public sealed class Usuario : BaseEntity
{
    private Usuario()
    {
    }

    public Usuario(string nombre, string correo, string passwordHash, long rolId)
    {
        Nombre = NombreUsuario.Create(nombre);
        Correo = Domain.ValueObjects.Correo.Create(correo);
        PasswordHash = Domain.ValueObjects.PasswordHash.Create(passwordHash);
        RolId = ValidateRolId(rolId);
    }

    public NombreUsuario Nombre { get; private set; } = null!;

    public Correo Correo { get; private set; } = null!;

    public PasswordHash PasswordHash { get; private set; } = null!;

    public long RolId { get; private set; }

    public Rol Rol { get; private set; } = null!;

    public void CambiarNombre(string nombre)
    {
        Nombre = NombreUsuario.Create(nombre);
    }

    public void CambiarCorreo(string correo)
    {
        Correo = Domain.ValueObjects.Correo.Create(correo);
    }

    public void CambiarPasswordHash(string passwordHash)
    {
        PasswordHash = Domain.ValueObjects.PasswordHash.Create(passwordHash);
    }

    public void CambiarRol(long rolId)
    {
        RolId = ValidateRolId(rolId);
    }

    private static long ValidateRolId(long rolId)
        => rolId > 0
            ? rolId
            : throw new ArgumentException(
                "RolId debe ser mayor que cero.",
                nameof(rolId));
}
