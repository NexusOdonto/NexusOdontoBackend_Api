using Application.DTOs.Chatbot.Gateway;

namespace Application.Contracts.Services.Chatbot;

public interface IChatbotGatewayService
{
    Task<QrDataResponseDto?> GetQrDataAsync(CancellationToken cancellationToken = default);
    Task<QrOperationResultDto> RestartQrAsync(CancellationToken cancellationToken = default);
    Task<QrOperationResultDto> LogoutQrAsync(CancellationToken cancellationToken = default);
    Task<ResolveIdentityResponseDto> ResolveIdentityAsync(ResolveIdentityRequestDto request, CancellationToken cancellationToken = default);
    Task<ResumeOperationResultDto> ResumeConversationAsync(ResumeConversationRequestDto request, CancellationToken cancellationToken = default);
    Task<ResumeOperationResultDto> ResumeAllAsync(CancellationToken cancellationToken = default);
}
