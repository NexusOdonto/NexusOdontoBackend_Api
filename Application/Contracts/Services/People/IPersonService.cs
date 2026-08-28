using Application.DTOs.People;

namespace Application.Contracts.Services.People
{
    public interface IPersonService
    {
        Task<IReadOnlyList<object>> GetAllAsync();
        Task<object?> GetByIdAsync(Guid id);
        Task<object> CreateAsync(CreatePersonDto request);
        Task<object?> UpdateAsync(Guid id, UpdatePersonDto request);
        Task<bool> DeleteAsync(Guid id);
    }
}
