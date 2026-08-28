using Application.DTOs.Clinical.ClinicalHistory;

namespace Application.Contracts.Services.Clinical
{
    public interface IClinicalHistoryService
    {
        Task<IReadOnlyList<ClinicalHistoryResponseDto>> GetAllAsync();
        Task<ClinicalHistoryResponseDto?> GetByIdAsync(Guid id);
        Task<ClinicalHistoryResponseDto?> UpdateAsync(Guid id, UpdateClinicalHistoryRequestDto request);
        Task<bool> DeleteAsync(Guid id);
    }
}
