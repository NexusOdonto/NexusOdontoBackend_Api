namespace Application.Contracts.Services.Common;

public interface ICatalogCrudService<TDto>
{
    Task<IReadOnlyList<TDto>> GetAllAsync();
    Task<TDto?> GetByIdAsync(Guid id);
    Task<TDto> CreateAsync(TDto request);
    Task<TDto?> UpdateAsync(Guid id, TDto request);
    Task<bool> DeleteAsync(Guid id);
}
