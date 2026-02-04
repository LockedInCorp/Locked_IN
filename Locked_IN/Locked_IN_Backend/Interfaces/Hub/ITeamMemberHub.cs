using Locked_IN_Backend.DTOs.Team;

namespace Locked_IN_Backend.Interfaces;

public interface ITeamMemberHub
{
    Task ReceiveJoinRequestStatus(TeamJoinStatusDto status);
}