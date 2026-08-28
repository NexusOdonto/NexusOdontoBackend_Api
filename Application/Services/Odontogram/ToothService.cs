using Application.Contracts.Repositories.Common;
using Application.Contracts.Repositories.Odontogram;
using Application.Contracts.Services.Odontogram;
using Application.DTOs.Odontogram.Tooth;
using Domain.Entities.Odontogram;

namespace Application.Services.Odontogram;

public sealed class ToothService(
    IToothRepository repository,
    IUnitOfWork unitOfWork) : IToothService
{
    public async Task<IReadOnlyList<ToothResponseDto>> GetAllAsync()
    {
        var items = await repository.GetAllAsync();
        return items.Select(Map).ToList();
    }

    public async Task<ToothResponseDto?> GetByCodeAsync(string code)
    {
        var entity = await repository.GetByCodeAsync(code);
        return entity is null ? null : Map(entity);
    }

    public async Task<ToothResponseDto> CreateAsync(CreateToothRequestDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        return await ServiceWrite.InTransactionAsync(unitOfWork, async () =>
        {
            var entity = new ToothEntity(request.Code, request.Name, request.DentitionTypeId, request.Quadrant, request.Position);
            await repository.AddAsync(entity);
            return Map(entity);
        });
    }

    public async Task<ToothResponseDto?> UpdateAsync(string code, UpdateToothRequestDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        var entity = await repository.GetByCodeAsync(code);
        if (entity is null) return null;

        return await ServiceWrite.InTransactionAsync(unitOfWork, async () =>
        {
            entity.Update(request.Name, request.DentitionTypeId, request.Quadrant, request.Position);
            repository.Update(entity);
            return Map(entity);
        });
    }

    public async Task<bool> DeleteAsync(string code)
    {
        var entity = await repository.GetByCodeAsync(code);
        if (entity is null) return false;
        await ServiceWrite.InTransactionAsync(unitOfWork, () => { repository.Remove(entity); return Task.CompletedTask; });
        return true;
    }

    private static ToothResponseDto Map(ToothEntity e) => new()
    {
        Code = e.Code,
        Name = e.Name,
        DentitionTypeId = e.DentitionTypeId,
        Quadrant = e.Quadrant,
        Position = e.Position
    };
}
