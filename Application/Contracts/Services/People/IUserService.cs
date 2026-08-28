using Application.DTOs.Users;

namespace Application.Contracts.Services.People
{
    public interface IUserService
    {
        Task<IReadOnlyList<object>> GetAllAsync();
        Task<object?> GetByIdAsync(Guid id);
        Task<object> CreateAsync(CreateUserDto request);
        Task<object?> UpdateAsync(Guid id, UpdateUserDto request);
        Task<bool> DeleteAsync(Guid id);
    }
}
