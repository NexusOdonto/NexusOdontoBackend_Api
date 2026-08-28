using Application.DTOs.Clinical.ProcedurePerformed;

namespace Application.Contracts.Services.Clinical
{
    public interface IProcedurePerformedService
    {
        Task<IReadOnlyList<ProcedurePerformedResponseDto>> GetAllAsync();
        Task<ProcedurePerformedResponseDto?> GetByIdAsync(Guid id);
        Task<ProcedurePerformedResponseDto> CreateAsync(CreateProcedurePerformedRequestDto request);
        Task<bool> DeleteAsync(Guid id);
    }
}
