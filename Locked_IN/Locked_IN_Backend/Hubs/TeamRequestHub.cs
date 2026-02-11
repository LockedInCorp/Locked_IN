using Locked_IN_Backend.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Locked_IN_Backend.Hubs;

[Authorize]
public class TeamRequestHub : Hub<IJoinRequestHub>
{
    
}