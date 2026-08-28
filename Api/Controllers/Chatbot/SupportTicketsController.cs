using Api.Controllers.Common;
using Application.Contracts.Services.Chatbot;
using Application.DTOs.Support.SupportTicket;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Chatbot;

[Route("api/v1/[controller]")]
public sealed class SupportTicketsController(ISupportTicketService service)
    : CrudControllerBase<ISupportTicketService, SupportTicketResponseDto, CreateSupportTicketRequestDto, UpdateSupportTicketRequestDto>(service)
{
    protected override string PermissionModule => "CHATBOT";
}
