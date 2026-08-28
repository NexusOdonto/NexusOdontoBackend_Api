using Api.Controllers.Common;
using Application.Contracts.Services.Schedule;
using Application.DTOs.Appointments;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Schedule;

[Route("api/v1/[controller]")]
public sealed class AppointmentsController(IAppointmentService service)
    : CrudControllerBase<IAppointmentService, AppointmentResponseDto, CreateAppointmentDto, UpdateAppointmentDto>(service)
{
    protected override string PermissionModule => "APPOINTMENTS";
}
