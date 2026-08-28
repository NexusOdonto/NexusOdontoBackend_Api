using Application.Contracts.Services.Common;
using Application.DTOs.Odontogram.DentalFinding;

namespace Application.Contracts.Services.Odontogram
{
    public interface IDentalFindingService : ICrudService<DentalFindingResponseDto, CreateDentalFindingRequestDto, UpdateDentalFindingRequestDto>
    {
    }
}
