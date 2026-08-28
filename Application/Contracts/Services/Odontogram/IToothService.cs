using Application.DTOs.Odontogram.Tooth;

namespace Application.Contracts.Services.Odontogram
{
    public interface IToothService
    {
        Task<IReadOnlyList<ToothResponseDto>> GetAllAsync();
        Task<ToothResponseDto?> GetByCodeAsync(string code);
        Task<ToothResponseDto> CreateAsync(CreateToothRequestDto request);
        Task<ToothResponseDto?> UpdateAsync(string code, UpdateToothRequestDto request);
        Task<bool> DeleteAsync(string code);
    }
}
