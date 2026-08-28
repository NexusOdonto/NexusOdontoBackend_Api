using Application.DTOs.Catalogs.Sex;

namespace Application.Contracts.Services.Catalogs
{
    public interface ISexService
    {
        Task<IReadOnlyList<SexDto>> GetAllAsync();
        Task<SexDto?> GetByIdAsync(Guid id);
        Task<SexDto> CreateAsync(SexDto request);
        Task<SexDto?> UpdateAsync(Guid id, SexDto request);
        Task<bool> DeleteAsync(Guid id);
    }
}
