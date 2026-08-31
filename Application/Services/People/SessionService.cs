using System.Security.Cryptography;
using System.Text;
using Application.Contracts.Repositories.Common;
using Application.Contracts.Repositories.People;
using Application.Contracts.Services.People;
using Application.DTOs.Sessions;
using Domain.Entities.People;

namespace Application.Services.People;

public sealed class SessionService(
    ISessionRepository sessionRepository,
    IUnitOfWork unitOfWork) : ISessionService
{
    public async Task<IReadOnlyList<SessionDto>> GetAllAsync()
    {
        var items = await sessionRepository.GetAllAsync();
        return items.Select(Map).ToList();
    }

    public async Task<SessionDto?> GetByIdAsync(Guid id)
    {
        var entity = await sessionRepository.GetByIdAsync(id);
        return entity is null ? null : Map(entity);
    }

    public async Task<SessionDto> CreateAsync(SessionDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        return await ServiceWrite.InTransactionAsync(unitOfWork, async () =>
        {
            var tokenFamily = Guid.NewGuid().ToString("N");
            var refreshHash = Convert.ToHexString(
                SHA256.HashData(Encoding.UTF8.GetBytes(Guid.NewGuid().ToString("N"))));

            var entity = new SessionEntity(
                request.UserId,
                refreshHash,
                tokenFamily,
                request.ExpiresAt == default ? DateTime.UtcNow.AddDays(7) : request.ExpiresAt,
                request.SessionStatusId);

            await sessionRepository.AddAsync(entity);
            return Map(entity);
        });
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var entity = await sessionRepository.GetByIdAsync(id);
        if (entity is null) return false;

        await ServiceWrite.InTransactionAsync(unitOfWork, () =>
        {
            entity.Update(entity.SessionStatusId, entity.LastUsedAt, DateTime.UtcNow);
            sessionRepository.Update(entity);
            return Task.CompletedTask;
        });
        return true;
    }

    private static SessionDto Map(SessionEntity e) => new()
    {
        Id = e.Id,
        UserId = e.UserId,
        CreatedAt = e.CreatedAt,
        ExpiresAt = e.ExpiresAt,
        LastUsedAt = e.LastUsedAt,
        RevokedAt = e.RevokedAt,
        SessionStatusId = e.SessionStatusId
    };
}
