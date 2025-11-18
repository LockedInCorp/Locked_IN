using Locked_IN_Backend.DTO;

namespace Locked_IN_Backend.Interfaces;

public interface ITeamService
{
    Task<GetTeamResponseModel?> GetTeamByIdAsync(int teamId);
    Task<List<GetTeamResponseModel>> GetTeamsByGameIdAsync(int gameId);
    Task<List<GetTeamResponseModel>> GetTeamsByNameSearchAsync(string searchTerm);
}