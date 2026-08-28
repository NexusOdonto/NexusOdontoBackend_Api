using Api.Controllers.Common;
using Application.Contracts.Services.People;
using Application.DTOs.Patients;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.People;

[Route("api/v1/[controller]")]
public sealed class PatientsController(IPatientService service)
    : CrudControllerBase<IPatientService, PatientResponseDto, CreatePatientDto, UpdatePatientDto>(service)
{
    protected override string PermissionModule => "PATIENTS";
}
