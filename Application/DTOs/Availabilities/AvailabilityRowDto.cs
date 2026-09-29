namespace Application.DTOs.Availabilities;

public sealed class AvailabilityRowDto
{
    public int StartDay { get; set; }
    public int EndDay { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly LunchStartTime { get; set; }
    public TimeOnly LunchEndTime { get; set; }
    public TimeOnly EndTime { get; set; }
}

public sealed class AvailabilityImpactRequestDto
{
    public Guid ProfessionalId { get; set; }
    public List<AvailabilityRowDto> Rows { get; set; } = [];
}

public sealed class ReplaceProfessionalAvailabilityDto
{
    public List<AvailabilityRowDto> Rows { get; set; } = [];
    public bool NotifyPatients { get; set; }
}

public sealed class ReplaceProfessionalAvailabilityResultDto
{
    public IReadOnlyList<AvailabilityResponseDto> Rows { get; set; } = [];
    public IReadOnlyList<Appointments.AffectedAppointmentDto> Affected { get; set; } = [];
    public int Notified { get; set; }
    public int Failed { get; set; }

    /// <summary>Date (Colombia) the saved schedule starts to apply; always the day after the save.</summary>
    public DateOnly EffectiveFrom { get; set; }

    /// <summary>True when the save only cancelled a pending change and kept the current schedule.</summary>
    public bool RevertedToCurrent { get; set; }

    /// <summary>"user" when the professional's own account was notified, "roles" for the admin/reception fallback, "none" on failure.</summary>
    public string ProfessionalNotifiedVia { get; set; } = "none";
}
