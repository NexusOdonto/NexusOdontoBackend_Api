using Api.Controllers.Common;
using Application.Contracts.Services.People;
using Application.DTOs.Users;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.People;

[Route("api/v1/[controller]")]
public sealed class UsersController(IUserService service)
    : CrudControllerBase<IUserService, UserDetailResponseDto, CreateUserDto, UpdateUserDto>(service)
{
    protected override string PermissionModule => "USERS";
}
