using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Api.Controllers.Common;
using Api.Filters;
using Application.Contracts.Services.Chatbot;
using Application.DTOs.Support.SupportTicket;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Chatbot;

[Route("api/v1/[controller]")]
public sealed class SupportTicketsController(ISupportTicketService service)
    : CrudControllerBase<ISupportTicketService, SupportTicketResponseDto, CreateSupportTicketRequestDto, UpdateSupportTicketRequestDto>(service)
{
    protected override string PermissionModule => "CHATBOT";

    [HttpPost("{id:guid}/reply")]
    [RequirePermission("CHATBOT", PermissionAction.Edit)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status502BadGateway)]
    public async Task<IActionResult> Reply(Guid id, [FromBody] ReplyTicketRequestDto request, CancellationToken cancellationToken)
    {
        if (request == null || string.IsNullOrWhiteSpace(request.Content))
            return BadRequest(new { message = "El contenido del mensaje es obligatorio." });

        var outcome = await Service.ReplyToTicketAsync(id, request.Content, request.MessageRoleId, GetUserId(), cancellationToken);
        return outcome switch
        {
            ReplyTicketOutcome.Success => Ok(new { message = "Mensaje enviado al paciente en WhatsApp con éxito." }),
            ReplyTicketOutcome.DeliveryFailed => StatusCode(
                StatusCodes.Status502BadGateway,
                new { message = "No se pudo entregar el mensaje por WhatsApp (chatbot/Evolution). Intenta de nuevo." }),
            _ => NotFound(new { message = "Ticket no encontrado o sin conversación asociada." }),
        };
    }

    [HttpPost("{id:guid}/resolve")]
    [RequirePermission("CHATBOT", PermissionAction.Edit)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Resolve(Guid id, [FromBody] ResolveTicketRequestDto? request, CancellationToken cancellationToken)
    {
        var success = await Service.ResolveTicketAsync(id, request?.Resolution, GetUserId(), cancellationToken);
        return success
            ? Ok(new { message = "Ticket resuelto y atenci�n devuelta al asistente virtual." })
            : NotFound(new { message = "Ticket no encontrado." });
    }

    private Guid? GetUserId()
    {
        var claim = User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);
        return Guid.TryParse(claim, out var userId) ? userId : null;
    }
}
