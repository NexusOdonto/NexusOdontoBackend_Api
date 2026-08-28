using Application.DTOs.Catalogs.Specialty;

namespace Application.Contracts.Services.Catalogs
{
    public interface ISpecialtyService
    {
        Task<IReadOnlyList<SpecialtyDto>> GetAllAsync();
        Task<SpecialtyDto?> GetByIdAsync(Guid id);
        Task<SpecialtyDto> CreateAsync(SpecialtyDto request);
        Task<SpecialtyDto?> UpdateAsync(Guid id, SpecialtyDto request);
        Task<bool> DeleteAsync(Guid id);
    }
}
