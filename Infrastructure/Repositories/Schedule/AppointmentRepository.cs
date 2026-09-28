using Application.Contracts.Repositories.Schedule;
using Domain.Entities.Catalogs;
using Domain.Entities.People;
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
        var appointments = await Set
            .AsNoTracking()
            .IgnoreQueryFilters()
            .Where(a => ids.Contains(a.Id))
            .ToListAsync(cancellationToken);
        if (appointments.Count == 0)
        {
            return appointments;
        }

        // The appointment navigations are bound to unused shadow FK columns, so Include() never
        // loads them; the related rows are fetched through the real FK columns instead.
        var patientIds = appointments.Select(a => a.PatientId).Distinct().ToList();
        var professionalIds = appointments.Select(a => a.ProfessionalId).Distinct().ToList();
        var serviceIds = appointments.Select(a => a.ServiceId).Distinct().ToList();

        var patients = await Context.Set<PatientEntity>()
            .AsNoTracking()
            .IgnoreQueryFilters()
            .Include(p => p.PersonEntity)
            .Where(p => patientIds.Contains(p.Id))
            .ToDictionaryAsync(p => p.Id, cancellationToken);
        var professionals = await Context.Set<ProfessionalEntity>()
            .AsNoTracking()
            .IgnoreQueryFilters()
            .Include(p => p.EmployeeEntity!).ThenInclude(e => e.PersonEntity)
            .Where(p => professionalIds.Contains(p.Id))
            .ToDictionaryAsync(p => p.Id, cancellationToken);
        var services = await Context.Set<ServiceEntity>()
            .AsNoTracking()
            .IgnoreQueryFilters()
            .Where(s => serviceIds.Contains(s.Id))
            .ToDictionaryAsync(s => s.Id, cancellationToken);

        foreach (var a in appointments)
        {
            a.PatientEntity = patients.GetValueOrDefault(a.PatientId);
            a.ProfessionalEntity = professionals.GetValueOrDefault(a.ProfessionalId);
            a.ServiceEntity = services.GetValueOrDefault(a.ServiceId);
        }

        return appointments;
    }
}
