namespace Application.DTOs.Odontogram.Tooth;

public sealed class ToothResponseDto
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public Guid DentitionTypeId { get; set; }
    public int Quadrant { get; set; }
    public int Position { get; set; }
}

public sealed class CreateToothRequestDto
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public Guid DentitionTypeId { get; set; }
    public int Quadrant { get; set; }
    public int Position { get; set; }
}

public sealed class UpdateToothRequestDto
{
    public string? Code { get; set; }
    public string Name { get; set; } = string.Empty;
    public Guid DentitionTypeId { get; set; }
    public int Quadrant { get; set; }
    public int Position { get; set; }
}
