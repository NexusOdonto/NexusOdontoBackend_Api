namespace Application.DTOs.Availabilities;

public sealed class AvailabilityResponseDto
{
    public Guid Id { get; set; }
    public Guid ProfessionalId { get; set; }
    public int StartDay { get; set; }
    public int EndDay { get; set; }
    public string StartTime { get; set; } = string.Empty;
    public string? LunchStartTime { get; set; }
    public string? LunchEndTime { get; set; }
    public string EndTime { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}
