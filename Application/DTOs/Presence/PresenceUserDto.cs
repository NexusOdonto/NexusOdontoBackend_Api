namespace Application.DTOs.Presence;

public sealed class PresenceUserDto
{
    public string UserId { get; set; } = string.Empty;

    public string? PersonId { get; set; }

    public string? DocumentNumber { get; set; }

    public string? Email { get; set; }

    public string? Name { get; set; }

    public IReadOnlyList<string> Roles { get; set; } = [];

    public IReadOnlyList<string> Keys { get; set; } = [];
}
