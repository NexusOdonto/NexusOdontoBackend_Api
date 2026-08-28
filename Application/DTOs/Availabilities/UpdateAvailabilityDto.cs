namespace Application.DTOs.Availabilities
{
    public class UpdateAvailabilityDto
    {
        public int StartDay { get; set; }
        public int EndDay { get; set; }
        public string StartTime { get; set; } = string.Empty;
        public string LunchStartTime { get; set; } = string.Empty;
        public string LunchEndTime { get; set; } = string.Empty;
        public string EndTime { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
