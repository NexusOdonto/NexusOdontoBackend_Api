namespace Application.DTOs.Catalogos
{
    public class EstadoCitaDto
    {
        public Guid Id { get; set; }

        public string Codigo { get; set; } = string.Empty;

        public string Nombre { get; set; } = string.Empty;

        public string? Descripcion { get; set; }

        public string? ColorHex { get; set; }

        public int? Orden { get; set; }

        public bool EsFinal { get; set; }

        public bool Activo { get; set; }
    }
}
