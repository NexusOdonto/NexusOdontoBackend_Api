using Application.Contracts.Services.Common;
using Application.DTOs.Odontogram.OdontogramTooth;

namespace Application.Contracts.Services.Odontogram
{
    public interface IOdontogramToothService : ICrudService<OdontogramToothResponseDto, CreateOdontogramToothRequestDto, UpdateOdontogramToothRequestDto>
    {
    }
}
