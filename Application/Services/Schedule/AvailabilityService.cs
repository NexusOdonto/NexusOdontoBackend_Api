using Application.Contracts.Repositories.Common;
using Application.Contracts.Repositories.People;
using Application.Contracts.Repositories.Schedule;
using Application.Contracts.Services.Schedule;
using Application.DTOs.Availabilities;
using Domain.Entities.People;
using Domain.Entities.Schedule;

namespace Application.Services.Schedule;

public sealed class AvailabilityService(
    IAvailabilityRepository repository,
    IProfessionalRepository professionalRepository,
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

        var professionalId = await ResolveProfessionalIdAsync(request.ProfessionalId);

        var startTime = FormatTime(request.StartTime, "08:00");
        var lunchStartTime = FormatTime(request.LunchStartTime, "12:00");
        var lunchEndTime = FormatTime(request.LunchEndTime, "12:00");
        var endTime = FormatTime(request.EndTime, "17:00");

        var startDay = request.StartDay is >= 1 and <= 7 ? request.StartDay : 1;
        var endDay = request.EndDay is >= 1 and <= 7 ? request.EndDay : 5;

        return await ServiceWrite.InTransactionAsync(unitOfWork, async () =>
        {
            var entity = new AvailabilityEntity(
                professionalId,
                startDay,
                endDay,
                startTime,
                lunchStartTime,
                lunchEndTime,
                endTime);

            await repository.AddAsync(entity);
            return Map(entity);
        });
    }

    public async Task<AvailabilityResponseDto?> UpdateAsync(Guid id, UpdateAvailabilityDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        var entity = await repository.GetByIdAsync(id);
        if (entity is null) return null;

        var startTime = FormatTime(request.StartTime, entity.StartTime);
        var lunchStartTime = FormatTime(request.LunchStartTime, entity.LunchStartTime);
        var lunchEndTime = FormatTime(request.LunchEndTime, entity.LunchEndTime);
        var endTime = FormatTime(request.EndTime, entity.EndTime);

        var startDay = request.StartDay is >= 1 and <= 7 ? request.StartDay : entity.StartDay;
        var endDay = request.EndDay is >= 1 and <= 7 ? request.EndDay : entity.EndDay;

        return await ServiceWrite.InTransactionAsync(unitOfWork, async () =>
        {
            entity.Update(
                startDay,
                endDay,
                startTime,
                lunchStartTime,
                lunchEndTime,
                endTime,
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

    private async Task<Guid> ResolveProfessionalIdAsync(Guid inputId)
    {
        // 1. Intentar directamente como ProfessionalId
        var prof = await professionalRepository.GetByIdAsync(inputId);
        if (prof is not null) return prof.Id;

        // 2. Intentar como EmployeeId
        prof = await professionalRepository.GetByEmployeeIdAsync(inputId);
        if (prof is not null) return prof.Id;

        // 3. Buscar en la lista general por PersonId o EmployeeId
        var all = await professionalRepository.GetAllAsync();
        prof = all.FirstOrDefault(p =>
            p.Id == inputId ||
            p.EmployeeId == inputId ||
            (p.EmployeeEntity != null && p.EmployeeEntity.PersonId == inputId));

        if (prof is not null) return prof.Id;

        throw new KeyNotFoundException($"No se encontró un profesional odontólogo activo correspondiente al ID '{inputId}'. Por favor verifique el ID del profesional.");
    }

    private static string FormatTime(string? time, string fallback)
    {
        if (string.IsNullOrWhiteSpace(time)) return fallback;
        time = time.Trim();
        if (TimeSpan.TryParse(time, out var parsed))
        {
            return parsed.ToString(@"hh\:mm");
        }
        return time.Length > 5 ? time[..5] : time;
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
