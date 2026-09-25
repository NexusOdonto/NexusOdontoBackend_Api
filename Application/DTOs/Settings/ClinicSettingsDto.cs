using System.Text.Json.Serialization;

namespace Application.DTOs.Settings;

public sealed class ClinicSettingsDto
{
    [JsonPropertyName("nombreClinica")]
    public string NombreClinica { get; set; } = "NexusOdonto Dental Clinic";

    [JsonPropertyName("razonSocial")]
    public string RazonSocial { get; set; } = "NexusOdonto Salud Oral S.A.S.";

    [JsonPropertyName("nit")]
    public string Nit { get; set; } = "901.458.729-3";

    [JsonPropertyName("telefono")]
    public string Telefono { get; set; } = "+57 (601) 745-9000";

    [JsonPropertyName("whatsapp")]
    public string Whatsapp { get; set; } = "+57 324 6030217";

    [JsonPropertyName("email")]
    public string Email { get; set; } = "NexusOdonto@nexus.co";

    [JsonPropertyName("direccion")]
    public string Direccion { get; set; } = "Calle 100 # 15-20, Centro Médico Odontológico";

    [JsonPropertyName("ciudad")]
    public string Ciudad { get; set; } = "Bogotá, Colombia";

    [JsonPropertyName("horaApertura")]
    public string HoraApertura { get; set; } = "08:00";

    [JsonPropertyName("horaCierre")]
    public string HoraCierre { get; set; } = "17:00";

    [JsonPropertyName("duracionCitaDefault")]
    public int DuracionCitaDefault { get; set; } = 45;

    [JsonPropertyName("diasHabiles")]
    public List<string> DiasHabiles { get; set; } = ["Lunes", "Martes", "Miércoles", "Jueves", "Viernes", "Sábado"];

    [JsonPropertyName("moneda")]
    public string Moneda { get; set; } = "COP ($)";

    [JsonPropertyName("prefijoRecibo")]
    public string PrefijoRecibo { get; set; } = "REC-";

    [JsonPropertyName("notificarWhatsApp")]
    public bool NotificarWhatsApp { get; set; } = true;

    [JsonPropertyName("notificarEmail")]
    public bool NotificarEmail { get; set; } = true;

    [JsonPropertyName("horasAnticipacionRecordatorio")]
    public int HorasAnticipacionRecordatorio { get; set; } = 24;

    [JsonPropertyName("backupAutomatico")]
    public bool BackupAutomatico { get; set; } = true;

    [JsonPropertyName("updatedAt")]
    public DateTime? UpdatedAt { get; set; }
}
