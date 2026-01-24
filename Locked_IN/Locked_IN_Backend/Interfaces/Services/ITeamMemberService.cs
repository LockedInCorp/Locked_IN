using Locked_IN_Backend.DTOs;

namespace Locked_IN_Backend.Services
{
    public enum TeamJoinResultStatus
    {
        Success,
        NotFound,
        BadRequest,
        Conflict
    }

    public record TeamJoinResult(TeamJoinResultStatus Status, string Message);

    public interface ITeamMemberService
    {
        Task<TeamJoinResult> RequestToJoinTeamAsync(int teamId, int userId);
        Task<List<TeamJoinResponceDto>> GetJoinRequestsAsync(int teamId);
        Task<TeamJoinResult> AcceptJoinRequestAsync(int teamId, int userId);
        Task<TeamJoinResult> DeclineJoinRequestAsync(int teamId, int userId);
        Task<TeamJoinResult> CancelJoinRequestAsync(int teamId, int userId);
    }
}