namespace Application.DTOs.Catalogs.WhatsAppNotificationType
{
    public class WhatsAppNotificationTypeDto
    {
        public Guid Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? DefaultTemplate { get; set; }
        public bool IsActive { get; set; }
        public int? SortOrder { get; set; }
    }
}
