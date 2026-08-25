namespace Application.DTOs;

public sealed class CrearServicioRequestDto
{
    public string Codigo { get; set; } = string.Empty;

    public string Nombre { get; set; } = string.Empty;

    public string? Descripcion { get; set; }

    public int DuracionMinutos { get; set; }

    public decimal? Precio { get; set; }

    public int? Orden { get; set; }
}
