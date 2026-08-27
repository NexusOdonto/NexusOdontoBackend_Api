using Application.Contracts.Services.Catalogs;
using Application.DTOs.Catalogs.SupportTicketStatus;
using Domain.Entities.Catalogs;

namespace Application.Services.Catalogs;

public sealed class SupportTicketStatusService(CatalogServiceCore<SupportTicketStatus> core) : ISupportTicketStatusService
{
    public Task<IReadOnlyList<SupportTicketStatusDto>> GetAllAsync() =>
        core.GetAllAsync(EnglishCatalogMapping.ToSupportTicketStatusDto);

    public Task<SupportTicketStatusDto?> GetByIdAsync(Guid id) =>
        core.GetByIdAsync(id, EnglishCatalogMapping.ToSupportTicketStatusDto);

    public Task<SupportTicketStatusDto> CreateAsync(CreateSupportTicketStatusRequestDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        var entity = new SupportTicketStatus(request.Code, request.Name, request.Description, request.IsFinal, request.AllowsAssignment, request.SortOrder);
        return core.CreateAsync(entity, EnglishCatalogMapping.ToSupportTicketStatusDto);
    }

    public Task<SupportTicketStatusDto?> UpdateAsync(Guid id, UpdateSupportTicketStatusRequestDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        return core.UpdateAsync(id, entity =>
        {
            entity.Update(request.Name, request.Description, request.IsFinal, request.AllowsAssignment, request.SortOrder); EnglishCatalogMapping.ApplyActive(entity, request.IsActive);
        }, EnglishCatalogMapping.ToSupportTicketStatusDto);
    }

    public Task<bool> DeleteAsync(Guid id) => core.DeleteAsync(id);
}
