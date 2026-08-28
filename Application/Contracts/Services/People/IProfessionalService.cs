using Application.Contracts.Services.Common;
using Application.DTOs.Professionals;

namespace Application.Contracts.Services.People
{
    public interface IProfessionalService : ICrudService<ProfessionalResponseDto, CreateProfessionalDto, UpdateProfessionalDto>
    {
    }
}
