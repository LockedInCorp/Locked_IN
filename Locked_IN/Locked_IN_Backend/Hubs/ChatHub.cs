using Locked_IN_Backend.DTO;
using Microsoft.AspNetCore.SignalR;
using Locked_IN_Backend.DTOs.Chat;
using Locked_IN_Backend.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace Locked_IN_Backend.Hubs;

/// <summary>
/// SignalR Hub for real-time chat communication.
/// Uses Groups feature to manage chat rooms efficiently.
/// </summary>
[Authorize]
public class ChatHub : Hub<IChatHub>
{
    public static string GetGroupName(int chatId) => $"Chat_{chatId}";
    public static string GetGroupName(string chatId) => $"Chat_{chatId}";

    /// <summary>
    /// Join a chat group when user connects to a chat room.
    /// Best Practice: Use Groups instead of sending to everyone.
    /// </summary>
    public async Task JoinChatGroup(string chatId, GetUserForTeamViewDto getUserForTeamViewDto)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, GetGroupName(chatId));
        await Clients.Group(GetGroupName(chatId)).UserJoined(getUserForTeamViewDto);
    }

    /// <summary>
    /// Leave a chat group when user disconnects from a chat room.
    /// </summary>
    public async Task LeaveChatGroup(string chatId, int userId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, GetGroupName(chatId));
        await Clients.Group(GetGroupName(chatId)).UserLeft(userId);
    }

    /// <summary>
    /// Notify others when a user leaves the chat via API call.
    /// </summary>
    public async Task SendUserLeftChat(string chatId, int userId)
    {
        await Clients.Group(GetGroupName(chatId)).UserLeft(userId);
    }

    /// <summary>
    /// Broadcast a new message to all participants in a chat group.
    /// Note: This is called AFTER the message is persisted to the database.
    /// </summary>
    public async Task SendMessageToGroup(string chatId, GetMessageDto getMessage)
    {
        await Clients.Group(GetGroupName(chatId)).ReceiveMessage(getMessage);
    }

    /// <summary>
    /// Broadcast an edited message to all participants in a chat group.
    /// </summary>
    public async Task SendEditedMessageToGroup(string chatId, GetMessageDto getMessage)
    {
        await Clients.Group(GetGroupName(chatId)).MessageEdited(getMessage);
    }

    /// <summary>
    /// Broadcast a deleted message notification to all participants in a chat group.
    /// </summary>
    public async Task SendDeletedMessageToGroup(string chatId, int messageId)
    {
        await Clients.Group(GetGroupName(chatId)).MessageDeleted(messageId);
    }

    /// <summary>
    /// Broadcast read receipt to all participants in a chat group.
    /// </summary>
    public async Task SendReadReceiptToGroup(string chatId, int userId, DateTime readAt)
    {
        await Clients.Group(GetGroupName(chatId)).MessageRead(new { UserId = userId, ReadAt = readAt });
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await base.OnDisconnectedAsync(exception);
    }
}

