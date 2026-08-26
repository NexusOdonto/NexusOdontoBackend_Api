namespace Application.DTOs.Personas
{
    public class CreatePersonaDto
    {
        public Guid TipoDocumentoId { get; set; }

        public string NumeroDocumento { get; set; } = string.Empty;

        public string Nombres { get; set; } = string.Empty;

        public string Apellidos { get; set; } = string.Empty;

        public DateTime? FechaNacimiento { get; set; }

        public Guid? SexoId { get; set; }

        public string? Telefono { get; set; }

        public string? Email { get; set; }

        public string? Direccion { get; set; }
    }
}
