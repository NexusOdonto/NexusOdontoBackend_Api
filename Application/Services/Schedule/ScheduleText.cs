using Application.DTOs.Availabilities;

namespace Application.Services.Schedule;

/// <summary>
/// Spanish descriptions of weekly schedules for notifications. Names are hardcoded because the
/// container may run with invariant globalization.
/// </summary>
public static class ScheduleText
{
    private static readonly string[] Days = ["", "lunes", "martes", "miércoles", "jueves", "viernes", "sábado", "domingo"];
    private static readonly string[] Months =
        ["", "enero", "febrero", "marzo", "abril", "mayo", "junio", "julio", "agosto", "septiembre", "octubre", "noviembre", "diciembre"];

    /// <summary>"mañana miércoles 30 de septiembre".</summary>
    public static string FormatTomorrow(DateOnly date) =>
        $"mañana {Days[AvailabilityRules.ToIsoDayOfWeek(date.DayOfWeek)]} {date.Day} de {Months[date.Month]}";

    /// <summary>"lunes a viernes 8:00–12:00 y 13:00–17:00 (almuerzo 12:00–13:00); sábado 8:00–12:00; no atiende domingo".</summary>
    public static string Describe(IReadOnlyList<AvailabilityRowDto> rows)
    {
        var perDay = new string?[8];
        for (var day = 1; day <= 7; day++)
        {
            var slots = rows
                .Where(r => r.StartDay <= day && day <= r.EndDay)
                .OrderBy(r => r.StartTime)
                .Select(DescribeRow)
                .Distinct()
                .ToList();
            perDay[day] = slots.Count == 0 ? null : string.Join(" y ", slots);
        }

        var parts = new List<string>();
        var off = new List<int>();
        var start = 1;
        while (start <= 7)
        {
            var end = start;
            while (end < 7 && perDay[end + 1] == perDay[start]) end++;

            if (perDay[start] is null)
            {
                for (var d = start; d <= end; d++) off.Add(d);
            }
            else
            {
                parts.Add($"{DayRange(start, end)} {perDay[start]}");
            }

            start = end + 1;
        }

        if (parts.Count == 0) return "sin días de atención";
        if (off.Count > 0) parts.Add($"no atiende {JoinDays(off)}");
        return string.Join("; ", parts);
    }

    private static string DescribeRow(AvailabilityRowDto r)
    {
        if (r.LunchStartTime < r.LunchEndTime)
        {
            return $"{Hour(r.StartTime)}–{Hour(r.LunchStartTime)} y {Hour(r.LunchEndTime)}–{Hour(r.EndTime)} " +
                   $"(almuerzo {Hour(r.LunchStartTime)}–{Hour(r.LunchEndTime)})";
        }

        return $"{Hour(r.StartTime)}–{Hour(r.EndTime)}";
    }

    private static string Hour(TimeOnly t) => $"{t.Hour}:{t.Minute:D2}";

    private static string DayRange(int start, int end) => (end - start) switch
    {
        0 => Days[start],
        1 => $"{Days[start]} y {Days[end]}",
        _ => $"{Days[start]} a {Days[end]}"
    };

    private static string JoinDays(IReadOnlyList<int> days) =>
        days.Count == 1
            ? Days[days[0]]
            : $"{string.Join(", ", days.Take(days.Count - 1).Select(d => Days[d]))} ni {Days[days[^1]]}";
}
