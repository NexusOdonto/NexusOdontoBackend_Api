using Api.Controllers.Common;
using Application.Contracts.Services.Clinical;
using Application.DTOs.Clinical.ClinicalAttention;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Clinical;

[Route("api/v1/[controller]")]
public sealed class ClinicalAttentionsController(IClinicalAttentionService service)
    : CrudControllerBase<IClinicalAttentionService, ClinicalAttentionResponseDto, CreateClinicalAttentionRequestDto, UpdateClinicalAttentionRequestDto>(service)
{
    protected override string PermissionModule => "CLINICAL";
}
