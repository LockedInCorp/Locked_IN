using Locked_IN_Backend.DTO;
using Locked_IN_Backend.DTOs.Chat;
using Locked_IN_Backend.Hubs;
using Locked_IN_Backend.Interfaces;
using Locked_IN_Backend.Interfaces.Services;
using Microsoft.AspNetCore.SignalR;

namespace Locked_IN_Backend.Services;

public class ChatHubService : IChatHubService
{
    private readonly IHubContext<ChatHub, IChatHub> _hubContext;

    public ChatHubService(IHubContext<ChatHub, IChatHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task SendMessageToGroupAsync(int chatId, GetMessageDto message)
    {
        await _hubContext.Clients.Group(ChatHub.GetGroupName(chatId)).ReceiveMessage(message);
    }

    public async Task SendEditedMessageToGroupAsync(int chatId, GetMessageDto message)
    {
        await _hubContext.Clients.Group(ChatHub.GetGroupName(chatId)).MessageEdited(message);
    }

    public async Task SendDeletedMessageToGroupAsync(int chatId, int messageId)
    {
        await _hubContext.Clients.Group(ChatHub.GetGroupName(chatId)).MessageDeleted(messageId);
    }

    public async Task SendReadReceiptToGroupAsync(int chatId, int userId, DateTime readAt)
    {
        await _hubContext.Clients.Group(ChatHub.GetGroupName(chatId)).MessageRead(new { UserId = userId, ReadAt = readAt });
    }

    public async Task SendUserJoinedAsync(int chatId, GetUserForTeamViewDto user)
    {
        await _hubContext.Clients.Group(ChatHub.GetGroupName(chatId)).UserJoined(user);
    }

    public async Task SendUserLeftChatAsync(int chatId, int userId)
    {
        await _hubContext.Clients.Group(ChatHub.GetGroupName(chatId)).UserLeft(userId);
    }
}
