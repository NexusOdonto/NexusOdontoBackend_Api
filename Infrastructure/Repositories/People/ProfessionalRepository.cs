using Application.Contracts.Repositories.People;
using Domain.Entities.People;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.People;

public sealed class ProfessionalRepository : Common.Repository<ProfessionalEntity>, IProfessionalRepository
{
    public ProfessionalRepository(AppDbContext context) : base(context)
    {
    }

    public override async Task<IReadOnlyList<ProfessionalEntity>> GetAllAsync()
    {
        return await Set
            .IgnoreQueryFilters()
            .Include(p => p.Specialties)
            .ToListAsync();
    }

    public async Task<ProfessionalEntity?> GetByEmployeeIdAsync(
        Guid employeeId,
        CancellationToken cancellationToken = default)
    {
        return await Set
            .Include(p => p.Specialties)
            .FirstOrDefaultAsync(p => p.EmployeeId == employeeId, cancellationToken);
    }

    public async Task<ProfessionalEntity?> GetByLicenseAsync(
        string professionalLicense,
        CancellationToken cancellationToken = default)
    {
        return await Set
            .Include(p => p.Specialties)
            .FirstOrDefaultAsync(
                p => p.ProfessionalLicense == professionalLicense,
                cancellationToken);
    }

    public async Task<ProfessionalEntity?> GetByIdWithSpecialtiesAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await Set
            .IgnoreQueryFilters()
            .Include(p => p.Specialties)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }
}
