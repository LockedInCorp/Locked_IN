using Locked_IN_Backend.DTO;
using Locked_IN_Backend.DTOs;

namespace Locked_IN_Backend.Services;

public interface ITeamMemberService
{
    Task RequestToJoinTeamAsync(int teamId, int userId);
    Task<List<GetTeamMemberDto>> GetActiveTeamMembersAsync(int teamId);
    Task<List<TeamJoinResponceDto>> GetJoinRequestsAsync(int teamId, int requesterId);
    Task AcceptJoinRequestAsync(int leaderId, int teamId, int userIdToAccept);
    Task DeclineJoinRequestAsync(int leaderId, int teamId, int userIdToDecline);
    Task CancelJoinRequestAsync(int teamId, int userId);
    Task LeaveTeamAsync(int teamId, int userId);
}