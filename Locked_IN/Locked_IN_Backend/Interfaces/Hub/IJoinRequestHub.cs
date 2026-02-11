using Locked_IN_Backend.DTOs;

namespace Locked_IN_Backend.Interfaces;

public interface IJoinRequestHub
{
    Task ReceiveNewJoinRequest(TeamJoinResponceDto status);
    Task ReceiveCanceledJoinRequest(int userId, int teamId);
}