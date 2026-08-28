using Application.Contracts.Services.Common;
using Application.DTOs.People;

namespace Application.Contracts.Services.People
{
    public interface IPersonService : ICrudService<PersonResponseDto, CreatePersonDto, UpdatePersonDto>
    {
    }
}
