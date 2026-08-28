using Application.DTOs.Clinical.Diagnosis;

namespace Application.Contracts.Services.Clinical
{
    public interface IDiagnosisService
    {
        Task<IReadOnlyList<DiagnosisResponseDto>> GetAllAsync();
        Task<DiagnosisResponseDto?> GetByIdAsync(Guid id);
        Task<DiagnosisResponseDto> CreateAsync(CreateDiagnosisRequestDto request);
        Task<bool> DeleteAsync(Guid id);
    }
}
