using Application.Contracts.Repositories.Schedule;
using Domain.Entities.Schedule;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Schedule;

public sealed class AppointmentRepository : Common.Repository<AppointmentEntity>, IAppointmentRepository
{
    public AppointmentRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IReadOnlyList<AppointmentEntity>> GetByProfessionalInRangeAsync(
        Guid professionalId,
        DateTime from,
        DateTime to,
        CancellationToken cancellationToken = default)
    {
        return await Set
            .Where(a =>
                a.ProfessionalId == professionalId &&
                a.StartsAt < to &&
                a.EndsAt > from)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<AppointmentEntity>> GetByPatientAsync(
        Guid patientId,
        CancellationToken cancellationToken = default)
    {
        return await Set
            .Where(a => a.PatientId == patientId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<AppointmentEntity>> GetWithDetailsAsync(
        IReadOnlyCollection<Guid> appointmentIds,
        CancellationToken cancellationToken = default)
    {
        if (appointmentIds.Count == 0)
        {
            return [];
        }

        var ids = appointmentIds.Distinct().ToList();
        return await Set
            .AsNoTracking()
            .IgnoreQueryFilters()
            .Include(a => a.PatientEntity!).ThenInclude(p => p.PersonEntity)
            .Include(a => a.ProfessionalEntity!).ThenInclude(p => p.EmployeeEntity!).ThenInclude(e => e.PersonEntity)
            .Include(a => a.ServiceEntity)
            .Where(a => ids.Contains(a.Id))
            .ToListAsync(cancellationToken);
    }
}
