using Application.Contracts.Services.Common;
using Application.DTOs.Users;

namespace Application.Contracts.Services.People
{
    public interface IUserService : ICrudService<UserDetailResponseDto, CreateUserDto, UpdateUserDto>
    {
    }
}
