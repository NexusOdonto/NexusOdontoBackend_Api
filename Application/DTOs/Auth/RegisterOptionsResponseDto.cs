using Application.DTOs.Catalogs.DocumentType;
using Application.DTOs.Catalogs.Sex;

namespace Application.DTOs.Auth;

public sealed class RegisterOptionsResponseDto
{
    public IReadOnlyList<DocumentTypeDto> DocumentTypes { get; set; } = [];

    public IReadOnlyList<SexDto> Sexes { get; set; } = [];
}
