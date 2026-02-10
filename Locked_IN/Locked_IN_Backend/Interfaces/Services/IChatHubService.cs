using Locked_IN_Backend.DTO;
using Locked_IN_Backend.DTOs.Chat;

namespace Locked_IN_Backend.Interfaces.Services;

public interface IChatHubService
{
    Task SendMessageToGroupAsync(int chatId, GetMessageDto message);
    Task SendEditedMessageToGroupAsync(int chatId, GetMessageDto message);
    Task SendDeletedMessageToGroupAsync(int chatId, int messageId);
    Task SendReadReceiptToGroupAsync(int chatId, int userId, DateTime readAt);
    Task SendUserJoinedAsync(int chatId, GetUserForTeamViewDto user);
    Task SendUserLeftChatAsync(int chatId, int userId);
}
