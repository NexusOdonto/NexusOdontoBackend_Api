namespace Application.DTOs.Catalogos
{
    public class PrioridadNotificacionDto
    {
        public Guid Id { get; set; }

        public string Codigo { get; set; } = string.Empty;

        public string Nombre { get; set; } = string.Empty;

        public string? Descripcion { get; set; }

        public string? ColorHex { get; set; }

        public int? Nivel { get; set; }

        public bool Activo { get; set; }

        public int? Orden { get; set; }
    }
}
