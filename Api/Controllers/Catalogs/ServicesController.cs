using Api.Controllers.Common;
using Application.Contracts.Services.Catalogs;
using Application.DTOs;
using Application.DTOs.Service;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Catalogs;

[Route("api/v1/[controller]")]
public sealed class ServicesController(IServiceService service)
    : CrudControllerBase<IServiceService, ServiceResponseDto, CreateServiceRequestDto, CreateServiceRequestDto>(service)
{
    protected override string PermissionModule => "CATALOGS";
}
