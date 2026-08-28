namespace Application.Contracts.Services.Common;

public interface ICrudService<TResponse, TCreate, TUpdate>
{
    Task<IReadOnlyList<TResponse>> GetAllAsync();
    Task<TResponse?> GetByIdAsync(Guid id);
    Task<TResponse> CreateAsync(TCreate request);
    Task<TResponse?> UpdateAsync(Guid id, TUpdate request);
    Task<bool> DeleteAsync(Guid id);
}
