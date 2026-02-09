using Locked_IN_Backend.Data.Entities;
using Locked_IN_Backend.DTOs;
using Locked_IN_Backend.Misc;

using Microsoft.EntityFrameworkCore.Storage;

namespace Locked_IN_Backend.Interfaces.Repositories;

public interface ITeamRepository
{
    Task<IEnumerable<Team>> GetTeams();
    Task<Team?> GetTeamById(int id);
    Task AddTeam(Team team);
    Task UpdateTeam(Team team);
    Task DeleteTeam(Team team);
    Task SaveChangesAsync();
    Task<IDbContextTransaction> BeginTransactionAsync();
    
    Task<List<Team>> GetTeamsByGameIdAsync(int gameId);
    Task<List<TeamSearchResult>> GetTeamsByNameSearchAsync(string searchTerm);
    Task<PagedResult<TeamSearchResult>> GetTeamsAdvancedAsync(List<int> gameIds, List<int> preferenceTagIds, string searchTerm, int page, int pageSize, string sortBy, int userId, bool OnlyShowPending = false);
}