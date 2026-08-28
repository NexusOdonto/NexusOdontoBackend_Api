namespace Application.DTOs.Service;

public sealed class ServiceResponseDto
{
    public Guid Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int DurationMinutes { get; set; }
    public decimal? Price { get; set; }
    public bool IsActive { get; set; }
    public int? SortOrder { get; set; }
    public DateTime CreatedAt { get; set; }
}
