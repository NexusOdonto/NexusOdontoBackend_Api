using Application.DTOs.Catalogs.JobTitle;

namespace Application.Contracts.Services.Catalogs
{
    public interface IJobTitleService
    {
        Task<IReadOnlyList<JobTitleDto>> GetAllAsync();
        Task<JobTitleDto?> GetByIdAsync(Guid id);
        Task<JobTitleDto> CreateAsync(JobTitleDto request);
        Task<JobTitleDto?> UpdateAsync(Guid id, JobTitleDto request);
        Task<bool> DeleteAsync(Guid id);
    }
}
