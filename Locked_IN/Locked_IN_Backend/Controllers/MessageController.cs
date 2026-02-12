using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Locked_IN_Backend.DTOs;
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

    public MessageController(IMessageService messageService)
    {
        _messageService = messageService;
    }

    /// <summary>
    /// Send a message to a chat.
    /// Best Practice: Persistence First - saves to DB, then broadcasts via SignalR.
    /// </summary>
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> SendMessage([FromForm] SendMessageDto sendMessageDto)
    {
        var userIdClaim = User.FindFirstValue(JwtRegisteredClaimNames.Sub);
        if (string.IsNullOrEmpty(userIdClaim)) return Unauthorized();
        var userId = int.Parse(userIdClaim);
        var result = await _messageService.SendMessageAsync(userId, sendMessageDto);

        return CreatedAtAction(nameof(GetChatMessages), new { chatId = sendMessageDto.ChatId }, result);
    }

    /// <summary>
    /// Get messages for a specific chat with pagination
    /// </summary>
    [Authorize]
    [HttpGet("{chatId}")]
    public async Task<ActionResult<PagedResult<GetMessageDto>>> GetChatMessages(int chatId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 50)
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
        var result = await _messageService.EditMessageAsync(userId, editMessageDto);
    
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
        await _messageService.DeleteMessageAsync(userId, messageId);
    
        return NoContent();
    }
}
