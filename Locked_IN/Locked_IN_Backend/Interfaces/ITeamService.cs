using Locked_IN_Backend.DTO;
using Locked_IN_Backend.Services;

namespace Locked_IN_Backend.Services;

public interface ITeamService
{
    Task<GetTeamResponse?> GetTeamByIdAsync(int teamId);
    Task<List<GetTeamResponse>> GetAllTeamsAsync();
    Task<List<GetTeamResponse>> GetTeamsByGameIdAsync(int gameId);
}