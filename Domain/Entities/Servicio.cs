using Domain.Common;

namespace Domain.Entities;

public class Servicio : BaseEntity
{
    public string Codigo { get; set; } = null!;
    public string Nombre { get; set; } = null!;
    public string? Descripcion { get; set; }
    public int DuracionMinutos { get; set; }
    public decimal? Precio { get; set; }
    public int? Orden { get; set; }

    private Servicio()
    {
    }

    public Servicio(
        string codigo,
        string nombre,
        string? descripcion,
        int duracionMinutos,
        decimal? precio,
        int? orden)
    {
        Codigo = codigo;
        Nombre = nombre;
        Descripcion = descripcion;
        DuracionMinutos = duracionMinutos;
        Precio = precio;
        Orden = orden;
    }

    public void Actualizar(
        string nombre,
        string? descripcion,
        int duracionMinutos,
        decimal? precio,
        int? orden)
    {
        FechaActualizacion = DateTime.UtcNow;
        Nombre = nombre;
        Descripcion = descripcion;
        DuracionMinutos = duracionMinutos;
        Precio = precio;
        Orden = orden;
    }
}
