using Application.Contracts.Services.Common;
using Application.DTOs.Support.SupportTicket;

namespace Application.Contracts.Services.Chatbot
{
    public interface ISupportTicketService : ICrudService<SupportTicketResponseDto, CreateSupportTicketRequestDto, UpdateSupportTicketRequestDto>
    {
    }
}
