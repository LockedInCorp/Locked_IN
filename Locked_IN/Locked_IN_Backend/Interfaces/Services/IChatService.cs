using Locked_IN_Backend.DTOs.Chat;

namespace Locked_IN_Backend.Interfaces;

public record ChatResult(bool Success, string Message, object? Data = null);

public interface IChatService
{
    Task<ChatResult> CreateDirectChatAsync(int creatorId, int targetUserId);
    Task<ChatResult> CreateTeamChatAsync(int creatorId, int teamId);
    Task<ChatResult> CreateChatAsync(int creatorId, CreateChatDto createChatDto);
    Task<ChatResult> SendMessageAsync(int userId, SendMessageDto sendMessageDto);
    Task<ChatResult> GetChatMessagesAsync(int userId, int chatId, int pageNumber = 1, int pageSize = 50);
    Task<ChatResult> GetUserChatsAsync(int userId);
    Task<ChatResult> GetChatByIdAsync(int userId, int chatId);
    Task<ChatResult> EditMessageAsync(int userId, EditMessageDto editMessageDto);
    Task<ChatResult> DeleteMessageAsync(int userId, int messageId);
    Task<ChatResult> MarkChatAsReadAsync(int userId, int chatId);
    Task<ChatResult> JoinChatGroupAsync(int userId, int chatId);
    Task<ChatResult> LeaveChatGroupAsync(int userId, int chatId);
}

