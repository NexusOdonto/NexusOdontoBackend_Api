using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Contracts.Services.Common;
using Application.DTOs.Support.SupportTicket;

namespace Application.Contracts.Services.Chatbot
{
    public interface ISupportTicketService : ICrudService<SupportTicketResponseDto, CreateSupportTicketRequestDto, UpdateSupportTicketRequestDto>
    {
        Task<bool> ReplyToTicketAsync(Guid ticketId, string messageContent, string? messageRoleId = null, Guid? userId = null, CancellationToken cancellationToken = default);
        Task<bool> ResolveTicketAsync(Guid ticketId, string? resolution = null, Guid? userId = null, CancellationToken cancellationToken = default);
    }
}
