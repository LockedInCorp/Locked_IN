using Microsoft.AspNetCore.SignalR;
using Locked_IN_Backend.DTOs.Chat;

namespace Locked_IN_Backend.Hubs;

/// <summary>
/// SignalR Hub for real-time chat communication.
/// Uses Groups feature to manage chat rooms efficiently.
/// </summary>
public class ChatHub : Hub
{
    /// <summary>
    /// Join a chat group when user connects to a chat room.
    /// Best Practice: Use Groups instead of sending to everyone.
    /// </summary>
    public async Task JoinChatGroup(string chatId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"Chat_{chatId}");
        await Clients.Group($"Chat_{chatId}").SendAsync("UserJoined", Context.ConnectionId);
    }

    /// <summary>
    /// Leave a chat group when user disconnects from a chat room.
    /// </summary>
    public async Task LeaveChatGroup(string chatId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"Chat_{chatId}");
        await Clients.Group($"Chat_{chatId}").SendAsync("UserLeft", Context.ConnectionId);
    }

    /// <summary>
    /// Broadcast a new message to all participants in a chat group.
    /// Note: This is called AFTER the message is persisted to the database.
    /// </summary>
    public async Task SendMessageToGroup(string chatId, MessageResponseDto message)
    {
        await Clients.Group($"Chat_{chatId}").SendAsync("ReceiveMessage", message);
    }

    /// <summary>
    /// Broadcast an edited message to all participants in a chat group.
    /// </summary>
    public async Task SendEditedMessageToGroup(string chatId, MessageResponseDto message)
    {
        await Clients.Group($"Chat_{chatId}").SendAsync("MessageEdited", message);
    }

    /// <summary>
    /// Broadcast a deleted message notification to all participants in a chat group.
    /// </summary>
    public async Task SendDeletedMessageToGroup(string chatId, int messageId)
    {
        await Clients.Group($"Chat_{chatId}").SendAsync("MessageDeleted", messageId);
    }

    /// <summary>
    /// Broadcast read receipt to all participants in a chat group.
    /// </summary>
    public async Task SendReadReceiptToGroup(string chatId, int userId, DateTime readAt)
    {
        await Clients.Group($"Chat_{chatId}").SendAsync("MessageRead", new { UserId = userId, ReadAt = readAt });
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        // Clean up groups when user disconnects
        await base.OnDisconnectedAsync(exception);
    }
}

