using Application.Contracts.Services.Catalogs;
using Application.DTOs.Catalogs.JobTitle;
using Domain.Entities.Catalogs;

namespace Application.Services.Catalogs;

public sealed class JobTitleService(CatalogServiceCore<JobTitleEntity> core) : IJobTitleService
{
    public Task<IReadOnlyList<JobTitleDto>> GetAllAsync() =>
        core.GetAllAsync(CatalogMapping.ToJobTitleDto);

    public Task<JobTitleDto?> GetByIdAsync(Guid id) =>
        core.GetByIdAsync(id, CatalogMapping.ToJobTitleDto);

    public Task<JobTitleDto> CreateAsync(JobTitleDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        var entity = new JobTitleEntity(request.Code, request.Name, request.Description, request.SortOrder);
        entity.IsActive = request.IsActive;
        return core.CreateAsync(entity, CatalogMapping.ToJobTitleDto);
    }

    public Task<JobTitleDto?> UpdateAsync(Guid id, JobTitleDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        return core.UpdateAsync(id, entity =>
        {
            entity.Update(request.Name, request.Description, request.SortOrder);
            CatalogMapping.ApplyActive(entity, request.IsActive);
        }, CatalogMapping.ToJobTitleDto);
    }

    public Task<bool> DeleteAsync(Guid id) => core.DeleteAsync(id);
}
