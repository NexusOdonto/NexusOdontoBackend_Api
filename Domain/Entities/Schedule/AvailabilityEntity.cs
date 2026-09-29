using Domain.Common;
using Domain.Entities.People;

namespace Domain.Entities.Schedule;

public class AvailabilityEntity : Entity
{
    public Guid ProfessionalId { get; set; }
    public int StartDay { get; set; }
    public int EndDay { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly LunchStartTime { get; set; }
    public TimeOnly LunchEndTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public bool IsActive { get; set; } = true;

    /// <summary>First date (inclusive) the row applies; null means since always.</summary>
    public DateOnly? EffectiveFrom { get; set; }

    /// <summary>Last date (inclusive) the row applies; null means open-ended.</summary>
    public DateOnly? EffectiveTo { get; set; }

    public ProfessionalEntity? ProfessionalEntity { get; set; }

    private AvailabilityEntity()
    {
    }

    public AvailabilityEntity(
        Guid professionalId,
        int startDay,
        int endDay,
        TimeOnly startTime,
        TimeOnly lunchStartTime,
        TimeOnly lunchEndTime,
        TimeOnly endTime)
    {
        ProfessionalId = professionalId;
        StartDay = startDay;
        EndDay = endDay;
        StartTime = startTime;
        LunchStartTime = lunchStartTime;
        LunchEndTime = lunchEndTime;
        EndTime = endTime;
    }

    public void Update(
        int startDay,
        int endDay,
        TimeOnly startTime,
        TimeOnly lunchStartTime,
        TimeOnly lunchEndTime,
        TimeOnly endTime,
        bool isActive)
    {
        StartDay = startDay;
        EndDay = endDay;
        StartTime = startTime;
        LunchStartTime = lunchStartTime;
        LunchEndTime = lunchEndTime;
        EndTime = endTime;
        IsActive = isActive;
    }
}
