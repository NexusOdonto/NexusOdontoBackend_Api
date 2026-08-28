using Api.Controllers.Common;
using Application.Contracts.Services.Clinical;
using Application.DTOs.Clinical.PatientAntecedent;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Clinical;

[Route("api/v1/[controller]")]
public sealed class PatientAntecedentsController(IPatientAntecedentService service)
    : CrudControllerBase<IPatientAntecedentService, PatientAntecedentResponseDto, CreatePatientAntecedentRequestDto, UpdatePatientAntecedentRequestDto>(service)
{
    protected override string PermissionModule => "CLINICAL";
}
