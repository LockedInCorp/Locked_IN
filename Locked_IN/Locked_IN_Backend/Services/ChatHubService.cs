using Locked_IN_Backend.DTO;
using Locked_IN_Backend.DTOs.Chat;
using Locked_IN_Backend.Hubs;
using Locked_IN_Backend.Interfaces;
using Locked_IN_Backend.Interfaces.Services;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace Locked_IN_Backend.Services;

public class ChatHubService : IChatHubService
{
    private readonly IHubContext<ChatHub, IChatHub> _hubContext;
    private static readonly ConcurrentDictionary<int, HashSet<string>> UserConnections = new();

    public ChatHubService(IHubContext<ChatHub, IChatHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public static void AddConnection(int userId, string connectionId)
    {
        var connections = UserConnections.GetOrAdd(userId, _ => new HashSet<string>());
        lock (connections)
        {
            connections.Add(connectionId);
        }
    }

    public static void RemoveConnection(int userId, string connectionId)
    {
        if (UserConnections.TryGetValue(userId, out var connections))
        {
            lock (connections)
            {
                connections.Remove(connectionId);
                if (connections.Count == 0)
                {
                    UserConnections.TryRemove(userId, out _);
                }
            }
        }
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

    public async Task AddUserToGroupAsync(int userId, int chatId)
    {
        if (UserConnections.TryGetValue(userId, out var connections))
        {
            string[] connectionIds;
            lock (connections)
            {
                connectionIds = connections.ToArray();
            }

            foreach (var connectionId in connectionIds)
            {
                await _hubContext.Groups.AddToGroupAsync(connectionId, ChatHub.GetGroupName(chatId));
            }
        }
    }

    public async Task RemoveUserFromGroupAsync(int userId, int chatId)
    {
        if (UserConnections.TryGetValue(userId, out var connections))
        {
            string[] connectionIds;
            lock (connections)
            {
                connectionIds = connections.ToArray();
            }

            foreach (var connectionId in connectionIds)
            {
                await _hubContext.Groups.RemoveFromGroupAsync(connectionId, ChatHub.GetGroupName(chatId));
            }
        }
    }
}
