using Application.Contracts.Services.Common;
using Application.DTOs;
using Application.DTOs.Service;

namespace Application.Contracts.Services.Catalogs
{
    public interface IServiceService : ICrudService<ServiceResponseDto, CreateServiceRequestDto, CreateServiceRequestDto>
    {
    }
}
