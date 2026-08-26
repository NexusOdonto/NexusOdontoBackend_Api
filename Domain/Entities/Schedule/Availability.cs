using Domain.Common;
using Domain.Entities.People;

namespace Domain.Entities.Schedule;

public class Availability : Entity
{
    public Guid ProfessionalId { get; set; }
    public int StartDay { get; set; }
    public int EndDay { get; set; }
    public string StartTime { get; set; } = null!;
    public string LunchStartTime { get; set; } = null!;
    public string LunchEndTime { get; set; } = null!;
    public string EndTime { get; set; } = null!;
    public bool IsActive { get; set; } = true;

    public Professional? Professional { get; set; }

    private Availability()
    {
    }

    public Availability(
        Guid professionalId,
        int startDay,
        int endDay,
        string startTime,
        string lunchStartTime,
        string lunchEndTime,
        string endTime)
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
        string startTime,
        string lunchStartTime,
        string lunchEndTime,
        string endTime,
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
