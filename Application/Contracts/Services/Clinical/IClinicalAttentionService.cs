using Application.Contracts.Services.Common;
using Application.DTOs.Clinical.ClinicalAttention;

namespace Application.Contracts.Services.Clinical
{
    public interface IClinicalAttentionService : ICrudService<ClinicalAttentionResponseDto, CreateClinicalAttentionRequestDto, UpdateClinicalAttentionRequestDto>
    {
    }
}
