namespace Application.DTOs.Chats.ScheduleChange;

public sealed class ScheduleChangeNotificationDto
{
    public Guid AppointmentId { get; set; }
    public Guid PatientId { get; set; }
    public string PatientName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public Guid ServiceId { get; set; }
    public string Service { get; set; } = string.Empty;
    public Guid ProfessionalId { get; set; }
    public string Professional { get; set; } = string.Empty;
    public DateTime StartsAt { get; set; }
    public DateTime EndsAt { get; set; }
}

public sealed class ScheduleChangeNotifyResult
{
    public bool Success { get; init; }

    /// <summary>"sent" or "queued" when the bot accepted the notification.</summary>
    public string? Status { get; init; }

    public string? Error { get; init; }
}
