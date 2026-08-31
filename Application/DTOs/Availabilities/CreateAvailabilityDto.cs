namespace Application.DTOs.Availabilities
{
    public class CreateAvailabilityDto
    {
        public Guid ProfessionalId { get; set; }
        public int StartDay { get; set; }
        public int EndDay { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly LunchStartTime { get; set; }
        public TimeOnly LunchEndTime { get; set; }
        public TimeOnly EndTime { get; set; }
    }
}
