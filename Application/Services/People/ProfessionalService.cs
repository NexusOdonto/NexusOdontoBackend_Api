using Application.Contracts.Repositories.Catalogs;
using Application.Contracts.Repositories.Common;
using Application.Contracts.Repositories.People;
using Application.Contracts.Services.People;
using Application.DTOs.Professionals;
using Domain.Entities.Catalogs;
using Domain.Entities.People;

namespace Application.Services.People;

public sealed class ProfessionalService(
    IProfessionalRepository professionalRepository,
    ICatalogRepository<SpecialtyEntity> specialtyCatalogRepository,
    IEmployeeRepository employeeRepository,
    IUnitOfWork unitOfWork) : IProfessionalService
{
    public async Task<IReadOnlyList<ProfessionalResponseDto>> GetAllAsync()
    {
        var items = await professionalRepository.GetAllAsync();
        return items.Select(Map).ToList();
    }

    public async Task<ProfessionalResponseDto?> GetByIdAsync(Guid id)
    {
        var entity = await professionalRepository.GetByIdWithSpecialtiesAsync(id)
            ?? await professionalRepository.GetByIdAsync(id);
        return entity is null ? null : Map(entity);
    }

    public async Task<ProfessionalResponseDto> CreateAsync(CreateProfessionalDto request)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (string.IsNullOrWhiteSpace(request.ProfessionalLicense) && !string.IsNullOrWhiteSpace(request.LicenseNumber))
        {
            request.ProfessionalLicense = request.LicenseNumber;
        }

        if (request.SpecialtyId.HasValue && !request.SpecialtyIds.Contains(request.SpecialtyId.Value))
        {
            request.SpecialtyIds.Add(request.SpecialtyId.Value);
        }

        return await ServiceWrite.InTransactionAsync(unitOfWork, async () =>
        {
            var employeeId = request.EmployeeId;
            if (employeeId == Guid.Empty && request.PersonId.HasValue && request.PersonId.Value != Guid.Empty)
            {
                var emp = await employeeRepository.GetByPersonIdAsync(request.PersonId.Value);
                if (emp is null)
                {
                    var jobTitleId = new Guid("11000000-0000-0000-0000-000000000002");
                    var code = $"OD{Guid.NewGuid():N}"[..7].ToUpper();
                    emp = new EmployeeEntity(request.PersonId.Value, jobTitleId, DateOnly.FromDateTime(DateTime.UtcNow), code);
                    await employeeRepository.AddAsync(emp);
                }
                employeeId = emp.Id;
            }

            var entity = new ProfessionalEntity(employeeId, request.ProfessionalLicense);

            if (request.SpecialtyIds is { Count: > 0 })
            {
                var allSpecialties = await specialtyCatalogRepository.GetAllAsync();
                var selected = allSpecialties.Where(s => request.SpecialtyIds.Contains(s.Id)).ToList();
                foreach (var spec in selected)
                {
                    entity.Specialties.Add(spec);
                }
            }

            await professionalRepository.AddAsync(entity);
            return Map(entity);
        });
    }

    public async Task<ProfessionalResponseDto?> UpdateAsync(Guid id, UpdateProfessionalDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        var entity = await professionalRepository.GetByIdWithSpecialtiesAsync(id)
            ?? await professionalRepository.GetByIdAsync(id);
        if (entity is null) return null;

        return await ServiceWrite.InTransactionAsync(unitOfWork, async () =>
        {
            entity.Update(request.ProfessionalLicense);
            entity.IsActive = request.IsActive;

            if (request.SpecialtyIds is not null)
            {
                entity.Specialties.Clear();
                if (request.SpecialtyIds.Count > 0)
                {
                    var allSpecialties = await specialtyCatalogRepository.GetAllAsync();
                    var selected = allSpecialties.Where(s => request.SpecialtyIds.Contains(s.Id)).ToList();
                    foreach (var spec in selected)
                    {
                        entity.Specialties.Add(spec);
                    }
                }
            }

            professionalRepository.Update(entity);
            return Map(entity);
        });
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var entity = await professionalRepository.GetByIdAsync(id);
        if (entity is null) return false;

        await ServiceWrite.InTransactionAsync(unitOfWork, () =>
        {
            entity.Deactivate();
            professionalRepository.Update(entity);
            return Task.CompletedTask;
        });
        return true;
    }

    private static ProfessionalResponseDto Map(ProfessionalEntity e) => new()
    {
        Id = e.Id,
        EmployeeId = e.EmployeeId,
        ProfessionalLicense = e.ProfessionalLicense,
        SpecialtyIds = e.Specialties.Select(s => s.Id).ToList(),
        IsActive = e.IsActive,
        CreatedAt = e.CreatedAt
    };
}
