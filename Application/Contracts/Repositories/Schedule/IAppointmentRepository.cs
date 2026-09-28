using Application.Contracts.Repositories.Common;
using Domain.Entities.Schedule;

namespace Application.Contracts.Repositories.Schedule;

public interface IAppointmentRepository : IRepository<AppointmentEntity>
{
    Task<IReadOnlyList<AppointmentEntity>> GetByProfessionalInRangeAsync(
        Guid professionalId,
        DateTime from,
        DateTime to,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AppointmentEntity>> GetByPatientAsync(
        Guid patientId,
        CancellationToken cancellationToken = default);

    /// <summary>Loads appointments with patient/professional person, and service navigations (no tracking).</summary>
    Task<IReadOnlyList<AppointmentEntity>> GetWithDetailsAsync(
        IReadOnlyCollection<Guid> appointmentIds,
        CancellationToken cancellationToken = default);
}
