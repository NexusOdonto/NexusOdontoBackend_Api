using Application.Contracts.Services.Common;
using Application.DTOs.Odontogram.FullOdontogram;
using Application.DTOs.Odontogram.Odontogram;

namespace Application.Contracts.Services.Odontogram
{
    public interface IOdontogramService : ICrudService<OdontogramResponseDto, CreateOdontogramRequestDto, UpdateOdontogramRequestDto>
    {
        Task<FullOdontogramResponseDto> SaveFullOdontogramAsync(SaveFullOdontogramRequestDto request, CancellationToken cancellationToken = default);
        Task<FullOdontogramResponseDto?> GetLatestByPatientIdAsync(Guid patientId, CancellationToken cancellationToken = default);
        Task<FullOdontogramResponseDto?> GetFullByIdAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
