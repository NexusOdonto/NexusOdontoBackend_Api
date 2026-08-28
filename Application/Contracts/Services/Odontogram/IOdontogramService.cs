using Application.Contracts.Services.Common;
using Application.DTOs.Odontogram.Odontogram;

namespace Application.Contracts.Services.Odontogram
{
    public interface IOdontogramService : ICrudService<OdontogramResponseDto, CreateOdontogramRequestDto, UpdateOdontogramRequestDto>
    {
    }
}
