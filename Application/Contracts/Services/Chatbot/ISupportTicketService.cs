using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Contracts.Services.Common;
using Application.DTOs.Support.SupportTicket;

namespace Application.Contracts.Services.Chatbot
{
    public enum ReplyTicketOutcome
    {
        NotFound,
        DeliveryFailed,
        Success
    }

    public interface ISupportTicketService : ICrudService<SupportTicketResponseDto, CreateSupportTicketRequestDto, UpdateSupportTicketRequestDto>
    {
        Task<ReplyTicketOutcome> ReplyToTicketAsync(Guid ticketId, string messageContent, string? messageRoleId = null, Guid? userId = null, CancellationToken cancellationToken = default);
        Task<bool> ResolveTicketAsync(Guid ticketId, string? resolution = null, Guid? userId = null, CancellationToken cancellationToken = default);
    }
}
