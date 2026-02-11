using Locked_IN_Backend.DTO;
using Microsoft.AspNetCore.SignalR;
using Locked_IN_Backend.DTOs.Chat;
using Locked_IN_Backend.Interfaces;
using Locked_IN_Backend.Interfaces.Repositories;
using Locked_IN_Backend.Services;
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

    public override async Task OnConnectedAsync()
    {
        var userIdClaim = Context.User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier) 
                          ?? Context.User?.FindFirst("sub");
        
        if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
        {
            ChatHubService.AddConnection(userId, Context.ConnectionId);

            using var scope = Context.GetHttpContext()?.RequestServices.CreateScope();
            var participantRepository = scope?.ServiceProvider.GetRequiredService<IChatParticipantRepository>();
            if (participantRepository != null)
            {
                var participants = await participantRepository.GetUserChatParticipantsAsync(userId);
                foreach (var participant in participants)
                {
                    await Groups.AddToGroupAsync(Context.ConnectionId, GetGroupName(participant.ChatId.ToString()));
                }
            }
        }
        
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userIdClaim = Context.User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier) 
                          ?? Context.User?.FindFirst("sub");
        
        if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
        {
            ChatHubService.RemoveConnection(userId, Context.ConnectionId);
        }

        await base.OnDisconnectedAsync(exception);
    }
}

