using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Locked_IN_Backend.DTOs.Chat;
using Locked_IN_Backend.Exceptions;
using Locked_IN_Backend.Interfaces;
using Locked_IN_Backend.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Locked_IN_Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MessageController : ControllerBase
{
    private readonly IMessageService _messageService;
    private readonly IHubContext<ChatHub> _hubContext;

    public MessageController(IMessageService messageService, IHubContext<ChatHub> hubContext)
    {
        _messageService = messageService;
        _hubContext = hubContext;
    }

    /// <summary>
    /// Send a message to a chat.
    /// Best Practice: Persistence First - saves to DB, then broadcasts via SignalR.
    /// </summary>
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> SendMessage([FromBody] SendMessageDto sendMessageDto)
    {
        var userIdClaim = User.FindFirstValue(JwtRegisteredClaimNames.Sub);
        if (string.IsNullOrEmpty(userIdClaim)) return Unauthorized();
        var userId = int.Parse(userIdClaim);
        
        // Persistence First: Save message to database
        var result = await _messageService.SendMessageAsync(userId, sendMessageDto);

        // Then broadcast via SignalR to the chat group
        await _hubContext.Clients.Group($"Chat_{sendMessageDto.ChatId}")
            .SendAsync("ReceiveMessage", result);

        return CreatedAtAction(nameof(GetChatMessages), new { chatId = sendMessageDto.ChatId }, result);
    }

    /// <summary>
    /// Get messages for a specific chat with pagination
    /// </summary>
    [Authorize]
    [HttpGet("{chatId}")]
    public async Task<IActionResult> GetChatMessages(int chatId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 50)
    {
        var userIdClaim = User.FindFirstValue(JwtRegisteredClaimNames.Sub);
        if (string.IsNullOrEmpty(userIdClaim)) return Unauthorized();
        var userId = int.Parse(userIdClaim);
        
        var result = await _messageService.GetChatMessagesAsync(userId, chatId, pageNumber, pageSize);
        return Ok(result);
    }

    /// <summary>
    /// Edit a message.
    /// Best Practice: Persistence First - updates DB, then broadcasts via SignalR.
    /// </summary>
    [Authorize]
    [HttpPut]
    public async Task<IActionResult> EditMessage([FromBody] EditMessageDto editMessageDto)
    {
        var userIdClaim = User.FindFirstValue(JwtRegisteredClaimNames.Sub);
        if (string.IsNullOrEmpty(userIdClaim)) return Unauthorized();
        var userId = int.Parse(userIdClaim);
        
        // Persistence First: Update message in database
        var result = await _messageService.EditMessageAsync(userId, editMessageDto);
    
        // Then broadcast via SignalR
        await _hubContext.Clients.Group($"Chat_{result.ChatId}")
            .SendAsync("MessageEdited", result);

        return Ok(result);
    }

    /// <summary>
    /// Delete a message (soft delete).
    /// Best Practice: Persistence First - updates DB, then broadcasts via SignalR.
    /// </summary>
    [Authorize]
    [HttpDelete("{messageId}")]
    public async Task<IActionResult> DeleteMessage(int messageId)
    {
        var userIdClaim = User.FindFirstValue(JwtRegisteredClaimNames.Sub);
        if (string.IsNullOrEmpty(userIdClaim)) return Unauthorized();
        var userId = int.Parse(userIdClaim);
        
        // Persistence First: Soft delete in database
        await _messageService.DeleteMessageAsync(userId, messageId);
    
        return NoContent();
    }
}
