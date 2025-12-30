using Locked_IN_Backend.DTOs.Chat;
using Locked_IN_Backend.Interfaces;
using Locked_IN_Backend.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Locked_IN_Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChatController : ControllerBase
{
    private readonly IChatService _chatService;
    private readonly IHubContext<ChatHub> _hubContext;

    public ChatController(IChatService chatService, IHubContext<ChatHub> hubContext)
    {
        _chatService = chatService;
        _hubContext = hubContext;
    }

    /// <summary>
    /// Create a new chat (Direct, Team, or Group)
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateChat([FromBody] CreateChatDto createChatDto, [FromQuery] int userId)
    {
        try
        {
            var result = await _chatService.CreateChatAsync(userId, createChatDto);
            
            if (!result.Success)
            {
                return BadRequest(new { Message = result.Message });
            }

            return Ok(result.Data);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while creating the chat.", Error = ex.Message });
        }
    }

    /// <summary>
    /// Send a message to a chat.
    /// Best Practice: Persistence First - saves to DB, then broadcasts via SignalR.
    /// </summary>
    [HttpPost("messages")]
    public async Task<IActionResult> SendMessage([FromBody] SendMessageDto sendMessageDto, [FromQuery] int userId)
    {
        try
        {
            // Persistence First: Save message to database
            var result = await _chatService.SendMessageAsync(userId, sendMessageDto);
            
            if (!result.Success)
            {
                return BadRequest(new { Message = result.Message });
            }

            // Then broadcast via SignalR to the chat group
            if (result.Data is MessageResponseDto messageDto)
            {
                await _hubContext.Clients.Group($"Chat_{sendMessageDto.ChatId}")
                    .SendAsync("ReceiveMessage", messageDto);
            }

            return Ok(result.Data);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while sending the message.", Error = ex.Message });
        }
    }

    /// <summary>
    /// Get messages for a specific chat with pagination
    /// </summary>
    [HttpGet("{chatId}/messages")]
    public async Task<IActionResult> GetChatMessages(int chatId, [FromQuery] int userId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 50)
    {
        var result = await _chatService.GetChatMessagesAsync(userId, chatId, pageNumber, pageSize);
        
        if (!result.Success)
        {
            return BadRequest(new { Message = result.Message });
        }

        return Ok(result.Data);
    }

    /// <summary>
    /// Get all chats for a user
    /// </summary>
    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetUserChats(int userId)
    {
        var result = await _chatService.GetUserChatsAsync(userId);
        
        if (!result.Success)
        {
            return BadRequest(new { Message = result.Message });
        }

        return Ok(result.Data);
    }

    /// <summary>
    /// Get a specific chat by ID
    /// </summary>
    [HttpGet("{chatId}")]
    public async Task<IActionResult> GetChat(int chatId, [FromQuery] int userId)
    {
        var result = await _chatService.GetChatByIdAsync(userId, chatId);
        
        if (!result.Success)
        {
            return BadRequest(new { Message = result.Message });
        }

        return Ok(result.Data);
    }

    /// <summary>
    /// Edit a message.
    /// Best Practice: Persistence First - updates DB, then broadcasts via SignalR.
    /// </summary>
    [HttpPut("messages")]
    public async Task<IActionResult> EditMessage([FromBody] EditMessageDto editMessageDto, [FromQuery] int userId)
    {
        // Persistence First: Update message in database
        var result = await _chatService.EditMessageAsync(userId, editMessageDto);
        
        if (!result.Success)
        {
            return BadRequest(new { Message = result.Message });
        }

        // Then broadcast via SignalR
        if (result.Data is MessageResponseDto messageDto)
        {
            await _hubContext.Clients.Group($"Chat_{messageDto.ChatId}")
                .SendAsync("MessageEdited", messageDto);
        }

        return Ok(result.Data);
    }

    /// <summary>
    /// Delete a message (soft delete).
    /// Best Practice: Persistence First - updates DB, then broadcasts via SignalR.
    /// </summary>
    [HttpDelete("messages/{messageId}")]
    public async Task<IActionResult> DeleteMessage(int messageId, [FromQuery] int userId)
    {
        // Persistence First: Soft delete in database
        var result = await _chatService.DeleteMessageAsync(userId, messageId);
        
        if (!result.Success)
        {
            return BadRequest(new { Message = result.Message });
        }

        // Get chat ID from message to broadcast
        // Note: In production, you might want to return chatId from DeleteMessageAsync
        // For now, we'll need to query it or modify the service to return it
        
        return Ok(result.Data);
    }

    /// <summary>
    /// Mark a chat as read.
    /// Best Practice: Persistence First - updates DB, then broadcasts via SignalR.
    /// </summary>
    [HttpPost("{chatId}/read")]
    public async Task<IActionResult> MarkChatAsRead(int chatId, [FromQuery] int userId)
    {
        // Persistence First: Update read status in database
        var result = await _chatService.MarkChatAsReadAsync(userId, chatId);
        
        if (!result.Success)
        {
            return BadRequest(new { Message = result.Message });
        }

        // Then broadcast read receipt via SignalR
        await _hubContext.Clients.Group($"Chat_{chatId}")
            .SendAsync("MessageRead", new { UserId = userId, ReadAt = DateTime.UtcNow });

        return Ok(result.Data);
    }

    /// <summary>
    /// Join a chat group
    /// </summary>
    [HttpPost("{chatId}/join")]
    public async Task<IActionResult> JoinChat(int chatId, [FromQuery] int userId)
    {
        var result = await _chatService.JoinChatGroupAsync(userId, chatId);
        
        if (!result.Success)
        {
            return BadRequest(new { Message = result.Message });
        }

        return Ok(result.Data);
    }

    /// <summary>
    /// Leave a chat group
    /// </summary>
    [HttpPost("{chatId}/leave")]
    public async Task<IActionResult> LeaveChat(int chatId, [FromQuery] int userId)
    {
        var result = await _chatService.LeaveChatGroupAsync(userId, chatId);
        
        if (!result.Success)
        {
            return BadRequest(new { Message = result.Message });
        }

        return Ok(result.Data);
    }
}

