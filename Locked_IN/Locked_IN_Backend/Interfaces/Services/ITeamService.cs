using Locked_IN_Backend.DTO;
using Locked_IN_Backend.DTOs;
using Locked_IN_Backend.DTOs.Team;

namespace Locked_IN_Backend.Interfaces;

public interface ITeamService
{
    Task<GetTeamDto?> GetTeamByIdAsync(int teamId);
    Task<List<GetTeamDto>> GetTeamsByGameIdAsync(int gameId);
    Task<List<GetTeamDto>> GetTeamsByNameSearchAsync(string searchTerm);
    Task<PagedResult<GetTeamsCardDto>> SearchTeamsAdvancedAsync(List<int> gameIds, List<int> preferenceTagIds, string searchTerm, int page, int pageSize, string sortBy, int userId, bool OnlyShowPending);
    Task<GetTeamDto> CreateTeamAsync(CreateTeamDto dto, int creatorId);
    Task<GetTeamDto> UpdateTeamAsync(int teamId, UpdateTeamDto dto, int userId);
}