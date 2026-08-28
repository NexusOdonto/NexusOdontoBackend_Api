using Application.Contracts.Repositories.Common;
using Application.Contracts.Repositories.Schedule;
using Application.Contracts.Services.Schedule;
using Application.DTOs.Availabilities;
using Domain.Entities.Schedule;

namespace Application.Services.Schedule;

public sealed class AvailabilityService(
    IAvailabilityRepository repository,
    IUnitOfWork unitOfWork) : IAvailabilityService
{
    public async Task<IReadOnlyList<AvailabilityResponseDto>> GetAllAsync()
    {
        var items = await repository.GetAllAsync();
        return items.Select(Map).ToList();
    }

    public async Task<AvailabilityResponseDto?> GetByIdAsync(Guid id)
    {
        var entity = await repository.GetByIdAsync(id);
        return entity is null ? null : Map(entity);
    }

    public async Task<AvailabilityResponseDto> CreateAsync(CreateAvailabilityDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        return await ServiceWrite.InTransactionAsync(unitOfWork, async () =>
        {
            var entity = new AvailabilityEntity(
                request.ProfessionalId,
                request.StartDay,
                request.EndDay,
                request.StartTime,
                request.LunchStartTime,
                request.LunchEndTime,
                request.EndTime);
            await repository.AddAsync(entity);
            return Map(entity);
        });
    }

    public async Task<AvailabilityResponseDto?> UpdateAsync(Guid id, UpdateAvailabilityDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        var entity = await repository.GetByIdAsync(id);
        if (entity is null) return null;

        return await ServiceWrite.InTransactionAsync(unitOfWork, async () =>
        {
            entity.Update(
                request.StartDay,
                request.EndDay,
                request.StartTime,
                request.LunchStartTime,
                request.LunchEndTime,
                request.EndTime,
                request.IsActive);
            repository.Update(entity);
            return Map(entity);
        });
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var entity = await repository.GetByIdAsync(id);
        if (entity is null) return false;
        await ServiceWrite.InTransactionAsync(unitOfWork, () =>
        {
            entity.Update(entity.StartDay, entity.EndDay, entity.StartTime, entity.LunchStartTime, entity.LunchEndTime, entity.EndTime, false);
            repository.Update(entity);
            return Task.CompletedTask;
        });
        return true;
    }

    private static AvailabilityResponseDto Map(AvailabilityEntity e) => new()
    {
        Id = e.Id,
        ProfessionalId = e.ProfessionalId,
        StartDay = e.StartDay,
        EndDay = e.EndDay,
        StartTime = e.StartTime,
        LunchStartTime = e.LunchStartTime,
        LunchEndTime = e.LunchEndTime,
        EndTime = e.EndTime,
        IsActive = e.IsActive
    };
}
