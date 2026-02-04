using Locked_IN_Backend.Data.Entities;

namespace Locked_IN_Backend.Interfaces.Repositories;

public interface ITeamMemberRepository
{
    Task<IEnumerable<TeamMember>> GetTeamMembersAsync();
    Task<TeamMember?> GetTeamMemberByIdAsync(int teamId, int userId);
    Task AddTeamMemberAsync(TeamMember teamMember);
    Task UpdateTeamMemberAsync(TeamMember teamMember);
    Task DeleteTeamMemberAsync(TeamMember teamMember);

    Task<int> GetActiveMemberCountAsync(int teamId);
    Task<TeamMember?> GetTeamMemberAsync(int teamId, int userId);
    Task<List<TeamMember>> GetPendingRequestsAsync(int userId);
    Task<List<TeamMember>> GetPendingRequestsByTeamIdAsync(int teamId);
    Task<TeamMember?> GetTeamMemberWithTeamByIdAsync(int teamId, int userId);
    Task<List<TeamMember>> GetTeamMembersByTeamIdAsync(int teamId);
    Task<TeamMember?> GetTeamLeaderAsync(int teamId);
}