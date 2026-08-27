using Domain.Entities.Schedule;

namespace Application.Contracts.Repositories;

public interface IAppointmentRepository : IRepository<Appointment>
{
    Task<IReadOnlyList<Appointment>> GetByProfessionalInRangeAsync(
        Guid professionalId,
        DateTime from,
        DateTime to,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Appointment>> GetByPatientAsync(
        Guid patientId,
        CancellationToken cancellationToken = default);
}
