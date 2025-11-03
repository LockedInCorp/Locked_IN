using Locked_IN_Backend.DTO;
using Locked_IN_Backend.Services;

namespace Locked_IN_Backend.Services;

public interface ITeamService
{
    Task<GetTeamResponseModel?> GetTeamByIdAsync(int teamId);
    Task<List<GetTeamResponseModel>> GetAllTeamsAsync();
    Task<List<GetTeamResponseModel>> GetTeamsByGameIdAsync(int gameId);
    Task<List<GetTeamResponseModel>> GetTeamsByNameSearchAsync(string searchTerm);
}