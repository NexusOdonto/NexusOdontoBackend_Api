namespace Application.DTOs.Pacientes
{
    public class UpdatePacienteDto
    {
        public string? ContactoEmergencia { get; set; }

        public string? TelefonoEmergencia { get; set; }

        public bool Activo { get; set; }
    }
}
