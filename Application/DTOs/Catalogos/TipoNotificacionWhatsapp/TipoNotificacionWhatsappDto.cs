namespace Application.DTOs.Catalogos
{
    public class TipoNotificacionWhatsappDto
    {
        public Guid Id { get; set; }

        public string Codigo { get; set; } = string.Empty;

        public string Nombre { get; set; } = string.Empty;

        public string? Descripcion { get; set; }

        public string? PlantillaPredeterminada { get; set; }

        public bool Activo { get; set; }

        public int? Orden { get; set; }
    }
}
