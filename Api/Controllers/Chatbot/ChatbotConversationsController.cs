using Api.Filters;
using Application.Contracts.Services.Chatbot;
using Application.DTOs.Chatbot.ChatbotConversation;
using Application.DTOs.Chatbot.ChatbotMessage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Chatbot;

[ApiController]
[Authorize]
[Route("api/v1/[controller]")]
public sealed class ChatbotConversationsController(IChatbotConversationService service) : ControllerBase
{
    [HttpGet]
    [RequirePermission("CHATBOT", PermissionAction.View)]
    [ProducesResponseType(typeof(IReadOnlyList<ChatbotConversationResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<ChatbotConversationResponseDto>>> List()
        => Ok(await service.GetAllAsync());

    [HttpGet("{id:guid}")]
    [RequirePermission("CHATBOT", PermissionAction.View)]
    [ProducesResponseType(typeof(ChatbotConversationResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ChatbotConversationResponseDto>> GetById(Guid id)
    {
        var item = await service.GetByIdAsync(id);
        return item is null ? NotFound() : Ok(item);
    }

    [HttpGet("{id:guid}/messages")]
    [RequirePermission("CHATBOT", PermissionAction.View)]
    [ProducesResponseType(typeof(IReadOnlyList<ChatbotMessageResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<ChatbotMessageResponseDto>>> GetMessages(Guid id)
        => Ok(await service.GetMessagesAsync(id));

    [HttpPost]
    [RequirePermission("CHATBOT", PermissionAction.Create)]
    [ProducesResponseType(typeof(ChatbotConversationResponseDto), StatusCodes.Status201Created)]
    public async Task<ActionResult<ChatbotConversationResponseDto>> Create([FromBody] CreateChatbotConversationRequestDto request)
    {
        var created = await service.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:guid}")]
    [RequirePermission("CHATBOT", PermissionAction.Edit)]
    [ProducesResponseType(typeof(ChatbotConversationResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ChatbotConversationResponseDto>> Update(Guid id, [FromBody] UpdateChatbotConversationRequestDto request)
    {
        var updated = await service.UpdateAsync(id, request);
        return updated is null ? NotFound() : Ok(updated);
    }

    [HttpDelete("{id:guid}")]
    [RequirePermission("CHATBOT", PermissionAction.Delete)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
        => await service.DeleteAsync(id) ? NoContent() : NotFound();
}
