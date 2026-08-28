using Application.DTOs.Professionals;

namespace Application.Contracts.Services.People
{
    public interface IProfessionalService
    {
        Task<IReadOnlyList<object>> GetAllAsync();
        Task<object?> GetByIdAsync(Guid id);
        Task<object> CreateAsync(CreateProfessionalDto request);
        Task<object?> UpdateAsync(Guid id, UpdateProfessionalDto request);
        Task<bool> DeleteAsync(Guid id);
    }
}
