using System.Text.Json;
using Application.Contracts.Repositories.Common;
using Application.Contracts.Repositories.Odontogram;
using Application.Contracts.Services.Odontogram;
using Domain.Entities.Odontogram;

namespace Application.Services.Odontogram;

public sealed class ToothService(
    IToothRepository repository,
    IUnitOfWork unitOfWork) : IToothService
{
    public async Task<IReadOnlyList<object>> GetAllAsync()
    {
        var items = await repository.GetAllAsync();
        return items.Select(Map).Cast<object>().ToList();
    }

    public async Task<object?> GetByIdAsync(Guid id)
    {
        var entity = await repository.GetByCodeAsync(id.ToString("D"));
        entity ??= await repository.GetByCodeAsync(id.ToString("N"));
        return entity is null ? null : Map(entity);
    }

    public async Task<object> CreateAsync(object request)
    {
        ArgumentNullException.ThrowIfNull(request);
        var dto = Deserialize(request);
        return await ServiceWrite.InTransactionAsync(unitOfWork, async () =>
        {
            var entity = new ToothEntity(dto.Code, dto.Name, dto.DentitionTypeId, dto.Quadrant, dto.Position);
            await repository.AddAsync(entity);
            return Map(entity);
        });
    }

    public async Task<object?> UpdateAsync(Guid id, object request)
    {
        ArgumentNullException.ThrowIfNull(request);
        var dto = Deserialize(request);
        var code = string.IsNullOrWhiteSpace(dto.Code) ? id.ToString("D") : dto.Code;
        var entity = await repository.GetByCodeAsync(code);
        if (entity is null) return null;

        return await ServiceWrite.InTransactionAsync(unitOfWork, async () =>
        {
            entity.Update(dto.Name, dto.DentitionTypeId, dto.Quadrant, dto.Position);
            repository.Update(entity);
            return Map(entity);
        });
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var entity = await repository.GetByCodeAsync(id.ToString("D"))
            ?? await repository.GetByCodeAsync(id.ToString("N"));
        if (entity is null) return false;
        await ServiceWrite.InTransactionAsync(unitOfWork, () => { repository.Remove(entity); return Task.CompletedTask; });
        return true;
    }

    private static ToothPayload Deserialize(object request)
    {
        if (request is ToothPayload typed) return typed;
        var json = JsonSerializer.Serialize(request);
        return JsonSerializer.Deserialize<ToothPayload>(json)
            ?? throw new InvalidOperationException("Invalid ToothEntity request payload.");
    }

    private static object Map(ToothEntity e) => new
    {
        e.Code, e.Name, e.DentitionTypeId, e.Quadrant, e.Position
    };

    private sealed class ToothPayload
    {
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public Guid DentitionTypeId { get; set; }
        public int Quadrant { get; set; }
        public int Position { get; set; }
    }
}
