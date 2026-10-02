using Api.Filters;
using Application.Contracts.Services.Chatbot;
using Application.DTOs.Chatbot.Gateway;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Chatbot;

[ApiController]
[Authorize]
[Route("api/v1/[controller]")]
public sealed class ChatbotController(IChatbotGatewayService service) : ControllerBase
{
    [HttpGet("qr")]
    [RequirePermission("CHATBOT", PermissionAction.View)]
    [ProducesResponseType(typeof(QrDataResponseDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<QrDataResponseDto>> GetQr(CancellationToken cancellationToken)
    {
        var data = await service.GetQrDataAsync(cancellationToken);
        return Ok(data ?? new QrDataResponseDto { Connected = false, State = "error" });
    }

    [HttpPost("qr/restart")]
    [RequirePermission("CHATBOT", PermissionAction.Edit)]
    [ProducesResponseType(typeof(QrOperationResultDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<QrOperationResultDto>> RestartQr(CancellationToken cancellationToken)
    {
        var result = await service.RestartQrAsync(cancellationToken);
        return Ok(result);
    }

    [HttpPost("qr/logout")]
    [RequirePermission("CHATBOT", PermissionAction.Edit)]
    [ProducesResponseType(typeof(QrOperationResultDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<QrOperationResultDto>> LogoutQr(CancellationToken cancellationToken)
    {
        var result = await service.LogoutQrAsync(cancellationToken);
        return Ok(result);
    }

    [HttpPost("resolve-identity")]
    [RequirePermission("CHATBOT", PermissionAction.View)]
    [ProducesResponseType(typeof(ResolveIdentityResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ResolveIdentityResponseDto>> ResolveIdentity(
        [FromBody] ResolveIdentityRequestDto request,
        CancellationToken cancellationToken)
    {
        if (request is null || string.IsNullOrWhiteSpace(request.Identifier))
        {
            return BadRequest(new { message = "El identificador es obligatorio." });
        }

        var result = await service.ResolveIdentityAsync(request, cancellationToken);
        return Ok(result);
    }

    [HttpPost("resume")]
    [RequirePermission("CHATBOT", PermissionAction.Edit)]
    [ProducesResponseType(typeof(ResumeOperationResultDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ResumeOperationResultDto>> ResumeConversation(
        [FromBody] ResumeConversationRequestDto request,
        CancellationToken cancellationToken)
    {
        if (request is null || string.IsNullOrWhiteSpace(request.PhoneNumber))
        {
            return BadRequest(new { message = "El número de teléfono es obligatorio." });
        }

        var result = await service.ResumeConversationAsync(request, cancellationToken);
        return Ok(result);
    }

    [HttpPost("resume-all")]
    [RequirePermission("CHATBOT", PermissionAction.Edit)]
    [ProducesResponseType(typeof(ResumeOperationResultDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<ResumeOperationResultDto>> ResumeAll(CancellationToken cancellationToken)
    {
        var result = await service.ResumeAllAsync(cancellationToken);
        return Ok(result);
    }
}
