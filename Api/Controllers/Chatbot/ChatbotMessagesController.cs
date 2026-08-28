using Api.Filters;
using Application.Contracts.Services.Chatbot;
using Application.DTOs.Chatbot.ChatbotMessage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Chatbot;

[ApiController]
[Authorize]
[Route("api/v1/[controller]")]
public sealed class ChatbotMessagesController(IChatbotMessageService service) : ControllerBase
{
    [HttpGet]
    [RequirePermission("CHATBOT", PermissionAction.View)]
    [ProducesResponseType(typeof(IReadOnlyList<ChatbotMessageResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<ChatbotMessageResponseDto>>> List()
        => Ok(await service.GetAllAsync());

    [HttpGet("{id:guid}")]
    [RequirePermission("CHATBOT", PermissionAction.View)]
    [ProducesResponseType(typeof(ChatbotMessageResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ChatbotMessageResponseDto>> GetById(Guid id)
    {
        var item = await service.GetByIdAsync(id);
        return item is null ? NotFound() : Ok(item);
    }

    [HttpPost]
    [RequirePermission("CHATBOT", PermissionAction.Create)]
    [ProducesResponseType(typeof(ChatbotMessageResponseDto), StatusCodes.Status201Created)]
    public async Task<ActionResult<ChatbotMessageResponseDto>> Create([FromBody] CreateChatbotMessageRequestDto request)
    {
        var created = await service.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpDelete("{id:guid}")]
    [RequirePermission("CHATBOT", PermissionAction.Delete)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
        => await service.DeleteAsync(id) ? NoContent() : NotFound();
}
