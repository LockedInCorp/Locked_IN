using Locked_IN_Backend.DTO;
using Locked_IN_Backend.DTOs;

namespace Locked_IN_Backend.Interfaces;

public interface ITeamService
{
    Task<GetTeamResponseModel?> GetTeamByIdAsync(int teamId);
    Task<List<GetTeamResponseModel>> GetTeamsByGameIdAsync(int gameId);
    Task<List<GetTeamResponseModel>> GetTeamsByNameSearchAsync(string searchTerm);
    Task<PagedResult<GetTeamsCardResponceModel>> SearchTeamsAdvancedAsync(List<int> gameIds, List<int> preferenceTagIds, string searchTerm, int page, int pageSize, string sortBy);
}