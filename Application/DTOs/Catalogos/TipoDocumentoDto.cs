namespace Application.DTOs.Catalogos
{
    public class TipoDocumentoDto
    {
        public Guid Id { get; set; }

        public string Codigo { get; set; } = string.Empty;

        public string Nombre { get; set; } = string.Empty;

        public string? Descripcion { get; set; }

        public bool Activo { get; set; }

        public int? Orden { get; set; }

        public DateTime FechaCreacion { get; set; }
    }
}
