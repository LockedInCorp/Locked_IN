using Locked_IN_Backend.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Locked_IN_Backend.Hubs;

/// <summary>
/// SignalR Hub for team join request status notifications.
/// </summary>
[Authorize]
public class TeamJoinHub : Hub<ITeamMemberHub>
{
    // This hub is used to notify users about their team join request status.
}
