namespace Application.DTOs.Catalogos
{
    public class ServicioDto
    {
        public Guid Id { get; set; }

        public string Codigo { get; set; } = string.Empty;

        public string Nombre { get; set; } = string.Empty;

        public string? Descripcion { get; set; }

        public int DuracionMinutos { get; set; }

        public decimal? Precio { get; set; }

        public bool Activo { get; set; }

        public int? Orden { get; set; }

        public DateTime CreadoEn { get; set; }
    }
}
