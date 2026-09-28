using Domain.Entities.Schedule;

namespace Application.Services.Schedule;

public static class AvailabilityRules
{
    public static bool IsWithinAvailability(DateTime startsAt, DateTime endsAt, AvailabilityEntity availability) =>
        IsWithinAvailability(
            startsAt,
            endsAt,
            availability.StartDay,
            availability.EndDay,
            availability.StartTime,
            availability.LunchStartTime,
            availability.LunchEndTime,
            availability.EndTime);

    /// <summary>
    /// Days are ISO (1=Mon..7=Sun). A lunch range where start &gt;= end means no lunch break.
    /// </summary>
    public static bool IsWithinAvailability(
        DateTime startsAt,
        DateTime endsAt,
        int startDay,
        int endDay,
        TimeOnly startTime,
        TimeOnly lunchStartTime,
        TimeOnly lunchEndTime,
        TimeOnly endTime)
    {
        var startIsoDay = ToIsoDayOfWeek(startsAt.DayOfWeek);
        var endIsoDay = ToIsoDayOfWeek(endsAt.DayOfWeek);

        if (startIsoDay < startDay || startIsoDay > endDay)
        {
            return false;
        }

        if (endsAt.Date != startsAt.Date && (endIsoDay < startDay || endIsoDay > endDay))
        {
            return false;
        }

        var apptStart = TimeOnly.FromDateTime(startsAt);
        var apptEnd = TimeOnly.FromDateTime(endsAt);

        if (apptStart < startTime || apptEnd > endTime)
        {
            return false;
        }

        if (lunchStartTime < lunchEndTime &&
            apptStart < lunchEndTime &&
            apptEnd > lunchStartTime)
        {
            return false;
        }

        return true;
    }

    public static int ToIsoDayOfWeek(DayOfWeek dayOfWeek) =>
        dayOfWeek == DayOfWeek.Sunday ? 7 : (int)dayOfWeek;
}
