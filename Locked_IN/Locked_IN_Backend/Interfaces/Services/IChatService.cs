using Locked_IN_Backend.DTOs.Chat;

namespace Locked_IN_Backend.Interfaces;


public interface IChatService
{
    Task<GetChatDetails> CreateDirectChatAsync(int creatorId, int targetUserId);
    Task<GetChatDetails> CreateTeamChatAsync(int creatorId, int teamId, bool saveChanges = true);
    
    Task<GetChatDetails> GetChatByIdAsync(int userId, int chatId);
    Task MarkChatAsReadAsync(int userId, int chatId);
    Task JoinChatGroupAsync(int userId, int chatId);
    Task LeaveChatGroupAsync(int userId, int chatId);
    
    Task<List<GetUserChatsDto>> GetUserChatsAsync(int userId);
}

