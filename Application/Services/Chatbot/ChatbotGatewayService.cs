using Application.Contracts.Repositories.Chatbot;
using Application.Contracts.Repositories.Common;
using Application.Contracts.Services.Chatbot;
using Application.DTOs.Chatbot.Gateway;
using Microsoft.Extensions.Logging;

namespace Application.Services.Chatbot;

public sealed class ChatbotGatewayService(
    IChatbotClient chatbotClient,
    IChatbotConversationRepository conversationRepository,
    IUnitOfWork unitOfWork,
    ILogger<ChatbotGatewayService> logger) : IChatbotGatewayService
{
    public async Task<QrDataResponseDto?> GetQrDataAsync(CancellationToken cancellationToken = default)
    {
        return await chatbotClient.GetQrDataAsync(cancellationToken);
    }

    public async Task<QrOperationResultDto> RestartQrAsync(CancellationToken cancellationToken = default)
    {
        var success = await chatbotClient.RestartQrAsync(cancellationToken);
        return new QrOperationResultDto
        {
            Status = success ? "success" : "error",
            Message = success ? "Instancia QR reiniciada correctamente." : "Error reiniciando instancia QR."
        };
    }

    public async Task<QrOperationResultDto> LogoutQrAsync(CancellationToken cancellationToken = default)
    {
        var success = await chatbotClient.LogoutQrAsync(cancellationToken);
        return new QrOperationResultDto
        {
            Status = success ? "ok" : "error",
            Unlinked = success,
            Message = success ? "Sesión de WhatsApp cerrada exitosamente." : "Error al cerrar sesión de WhatsApp."
        };
    }

    public async Task<ResolveIdentityResponseDto> ResolveIdentityAsync(
        ResolveIdentityRequestDto request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (string.IsNullOrWhiteSpace(request.Identifier))
        {
            return new ResolveIdentityResponseDto
            {
                Status = "unresolved",
                Identifier = string.Empty
            };
        }

        var result = await chatbotClient.ResolveIdentityAsync(request.Identifier, request.ConversationId, cancellationToken);
        if (result is not null && !string.IsNullOrWhiteSpace(result.Phone) && request.ConversationId.HasValue)
        {
            try
            {
                var conv = await conversationRepository.GetByIdAsync(request.ConversationId.Value);
                if (conv is not null && conv.ChatIdentifier != result.Phone)
                {
                    conv.SetChatIdentifier(result.Phone);
                    conversationRepository.Update(conv);
                    await unitOfWork.SaveChangesAsync(cancellationToken);
                }
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "No se pudo actualizar chatIdentifier en BD para la conversación {Id}", request.ConversationId);
            }
        }

        return result ?? new ResolveIdentityResponseDto
        {
            Status = "unresolved",
            Identifier = request.Identifier
        };
    }

    public async Task<ResumeOperationResultDto> ResumeConversationAsync(
        ResumeConversationRequestDto request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (string.IsNullOrWhiteSpace(request.PhoneNumber))
        {
            return new ResumeOperationResultDto
            {
                Status = "error",
                Message = "El número de teléfono es obligatorio."
            };
        }

        var success = await chatbotClient.ResumeConversationAsync(request.PhoneNumber, request.ClearHistory, cancellationToken);
        return new ResumeOperationResultDto
        {
            Status = success ? "success" : "warning",
            PhoneNumber = request.PhoneNumber,
            Message = success ? "Conversación reactivada con NexusBot." : "No se pudo reactivar la conversación en el chatbot."
        };
    }

    public async Task<ResumeOperationResultDto> ResumeAllAsync(CancellationToken cancellationToken = default)
    {
        var success = await chatbotClient.ResumeAllConversationsAsync(cancellationToken);
        return new ResumeOperationResultDto
        {
            Status = success ? "success" : "warning",
            Message = success ? "Todas las conversaciones fueron reactivadas exitosamente." : "No se pudieron reactivar todas las conversaciones."
        };
    }
}
