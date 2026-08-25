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
}
