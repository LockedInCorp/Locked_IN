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
public class ChatController : ControllerBase
{
    private readonly IChatService _chatService;

    public ChatController(IChatService chatService)
    {
        _chatService = chatService;
    }

    /// <summary>
    /// Create a new direct chat with a user
    /// </summary>
    [Authorize]
    [HttpPost("direct/{targetUserId}")]
    public async Task<IActionResult> CreateDirectChat(int targetUserId)
    {
        var userIdClaim = User.FindFirstValue(JwtRegisteredClaimNames.Sub);
        if (string.IsNullOrEmpty(userIdClaim)) return Unauthorized();
        var userId = int.Parse(userIdClaim);
        
        var result = await _chatService.CreateDirectChatAsync(userId, targetUserId);
        return CreatedAtAction(nameof(GetChat), new { chatId = result.Id }, result);
    }

    /// <summary>
    /// Create a new team chat
    /// </summary>
    [Authorize]
    [HttpPost("team/{teamId}")]
    public async Task<IActionResult> CreateTeamChat(int teamId)
    {
        var userIdClaim = User.FindFirstValue(JwtRegisteredClaimNames.Sub);
        if (string.IsNullOrEmpty(userIdClaim)) return Unauthorized();
        var userId = int.Parse(userIdClaim);
        
        var result = await _chatService.CreateTeamChatAsync(userId, teamId);
        return CreatedAtAction(nameof(GetChat), new { chatId = result.Id }, result);
    }

    /// <summary>
    /// Get all chats for a user
    /// </summary>
    [Authorize]
    [HttpGet("user")]
    public async Task<IActionResult> GetUserChats([FromQuery] string? searchTerm = null)
    {
        var userIdClaim = User.FindFirstValue(JwtRegisteredClaimNames.Sub);
        if (string.IsNullOrEmpty(userIdClaim)) return Unauthorized();
        var userId = int.Parse(userIdClaim);
        var result = await _chatService.GetUserChatsAsync(userId, searchTerm);

        return Ok(result);
    }

    /// <summary>
    /// Get a specific chat by ID
    /// </summary>
    [Authorize]
    [HttpGet("{chatId}")]
    public async Task<IActionResult> GetChat(int chatId)
    {
        var userIdClaim = User.FindFirstValue(JwtRegisteredClaimNames.Sub);
        if (string.IsNullOrEmpty(userIdClaim)) return Unauthorized();
        var userId = int.Parse(userIdClaim);
        
        var result = await _chatService.GetChatByIdAsync(userId, chatId);
        return Ok(result);
    }

    /// <summary>
    /// Mark a chat as read.
    /// Best Practice: Persistence First - updates DB, then broadcasts via SignalR.
    /// </summary>
    [Authorize]
    [HttpPost("{chatId}/read")]
    public async Task<IActionResult> MarkChatAsRead(int chatId)
    {
        var userIdClaim = User.FindFirstValue(JwtRegisteredClaimNames.Sub);
        if (string.IsNullOrEmpty(userIdClaim)) return Unauthorized();
        var userId = int.Parse(userIdClaim);
        
        // Persistence First: Update read status in database
        await _chatService.MarkChatAsReadAsync(userId, chatId);


        return NoContent();
    }

    /// <summary>
    /// Join a chat group
    /// </summary>
    [Authorize]
    [HttpPost("{chatId}/join")]
    public async Task<IActionResult> JoinChat(int chatId)
    {
        var userIdClaim = User.FindFirstValue(JwtRegisteredClaimNames.Sub);
        if (string.IsNullOrEmpty(userIdClaim)) return Unauthorized();
        var userId = int.Parse(userIdClaim);
        
        await _chatService.JoinChatGroupAsync(userId, chatId);
        
        return NoContent();
    }

    /// <summary>
    /// Leave a chat group
    /// </summary>
    [Authorize]
    [HttpPost("{chatId}/leave")]
    public async Task<IActionResult> LeaveChat(int chatId)
    {
        var userIdClaim = User.FindFirstValue(JwtRegisteredClaimNames.Sub);
        if (string.IsNullOrEmpty(userIdClaim)) return Unauthorized();
        var userId = int.Parse(userIdClaim);
        
        await _chatService.LeaveChatGroupAsync(userId, chatId);
        return NoContent();
    }
}

