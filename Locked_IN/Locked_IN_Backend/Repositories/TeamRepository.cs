using Locked_IN_Backend.Data;
using Locked_IN_Backend.Data.Entities;
using Locked_IN_Backend.DTOs;
using Locked_IN_Backend.Interfaces.Repositories;
using Locked_IN_Backend.Misc;
using Microsoft.EntityFrameworkCore;

namespace Locked_IN_Backend.Repositories;

public class TeamRepository : ITeamRepository
{
    private readonly AppDbContext _context;

    public TeamRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Team>> GetTeams()
    {
        return await _context.Teams.ToListAsync();
    }

    public async Task<Team?> GetTeamById(int id)
    {
        return await _context.Teams.FindAsync(id);
    }

    public async Task AddTeam(Team team)
    {
         await _context.Teams.AddAsync(team);
         await _context.SaveChangesAsync();
    }

    public async Task UpdateTeam(Team team)
    {
        _context.Teams.Update(team);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteTeam(Team team)
    {
        _context.Teams.Remove(team);
        await _context.SaveChangesAsync();
    }
    

    public async Task<Team?> GetTeamWithDetailsByIdAsync(int teamId)
    {
        return await _context.Teams
            .Include(t => t.Game)
            .Include(t => t.ExperienceTag)
            .Include(t => t.TeamMembers)
                .ThenInclude(tm => tm.User)
            .Include(t => t.TeamPreferencetagRelations)
                .ThenInclude(r => r.PreferenceTag)
            .FirstOrDefaultAsync(t => t.Id == teamId);
    }

    public async Task<List<Team>> GetTeamsByGameIdAsync(int gameId)
    {
        return await _context.Teams
            .Include(t => t.Game)
            .Include(t => t.ExperienceTag)
            .Include(t => t.TeamMembers)
                .ThenInclude(tm => tm.User)
            .Include(t => t.TeamPreferencetagRelations)
                .ThenInclude(r => r.PreferenceTag)
            .Where(t => t.GameId == gameId)
            .ToListAsync();
    }

    public async Task<List<TeamSearchResult>> GetTeamsByNameSearchAsync(string searchTerm)
    {
        var query = _context.Teams.AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(t => EF.Functions.ToTsVector("english", t.Name).Matches(EF.Functions.WebSearchToTsQuery(searchTerm)));
        }

        var results = await query
            .Select(t => new
            {
                Id = t.Id,
                Rank = !string.IsNullOrWhiteSpace(searchTerm)
                    ? EF.Functions.ToTsVector("english", t.Name).Rank(EF.Functions.WebSearchToTsQuery(searchTerm))
                    : 0
            })
            .OrderByDescending(x => x.Rank)
            .ToListAsync();

        var teamIds = results.Select(r => r.Id).ToList();

        var teams = await _context.Teams
            .Include(t => t.Game)
            .Include(t => t.ExperienceTag)
            .Include(t => t.TeamMembers)
                .ThenInclude(tm => tm.User)
            .Include(t => t.TeamPreferencetagRelations)
                .ThenInclude(r => r.PreferenceTag)
            .Where(t => teamIds.Contains(t.Id))
            .ToListAsync();

        var teamMap = teams.ToDictionary(t => t.Id);

        return results.Select(r => new TeamSearchResult
        {
            Team = teamMap[r.Id],
            SearchRank = r.Rank
        }).ToList();
    }
    
    public async Task<PagedResult<TeamSearchResult>> GetTeamsAdvancedAsync(List<int> gameIds, List<int> preferenceTagIds, string searchTerm, int page, int pageSize, string sortBy)
    {
        var query = _context.Teams.AsQueryable();

        if (gameIds != null && gameIds.Count > 0)
        {
            query = query.Where(t => gameIds.Contains(t.GameId));
        }

        if (preferenceTagIds != null && preferenceTagIds.Count > 0)
        {
            query = query.Where(t => t.TeamPreferencetagRelations
                .Any(rel => preferenceTagIds.Contains(rel.PreferenceTagId)));
        }

        var hasSearch = !string.IsNullOrWhiteSpace(searchTerm);
        if (hasSearch)
        {
            query = query.Where(t => EF.Functions.ToTsVector("english", t.Name)
                .Matches(EF.Functions.WebSearchToTsQuery(searchTerm)));
        }

        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 12;

        var totalCount = await query.CountAsync();

        var intermediate = query.Select(t => new
        {
            TeamId = t.Id,
            SearchRank = hasSearch
                ? EF.Functions.ToTsVector("english", t.Name)
                    .Rank(EF.Functions.WebSearchToTsQuery(searchTerm))
                : 0,
            LatestMemberJoin = t.TeamMembers
                .OrderByDescending(tm => tm.Jointimestamp)
                .Select(tm => (DateTime?)tm.Jointimestamp)
                .FirstOrDefault(),
            LeaderNickname = t.TeamMembers
                .Where(tm => tm.Isleader)
                .Select(tm => tm.User.Nickname)
                .FirstOrDefault(),
            CreationTimestamp = t.CreationTimestamp
        });

        var sort = (sortBy ?? string.Empty).Trim().ToLower();
        if (sort == "relevance")
        {
            intermediate = intermediate
                .OrderByDescending(x => x.SearchRank)
                .ThenByDescending(x => x.CreationTimestamp);
        }
        else if (sort == "popular")
        {
            intermediate = intermediate
                .OrderByDescending(x => x.LatestMemberJoin)
                .ThenByDescending(x => x.CreationTimestamp);
        }
        else if (sort == "newest")
        {
            intermediate = intermediate
                .OrderByDescending(x => x.CreationTimestamp);
        }
        else
        {
            if (hasSearch)
            {
                intermediate = intermediate
                    .OrderByDescending(x => x.SearchRank)
                    .ThenByDescending(x => x.CreationTimestamp);
            }
            else
            {
                intermediate = intermediate
                    .OrderByDescending(x => x.CreationTimestamp);
            }
        }

        var pagedResults = await intermediate
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var teamIds = pagedResults.Select(r => r.TeamId).ToList();

        var teams = await _context.Teams
            .Include(t => t.Game)
            .Include(t => t.ExperienceTag)
            .Include(t => t.TeamMembers)
                .ThenInclude(tm => tm.User)
            .Include(t => t.TeamPreferencetagRelations)
                .ThenInclude(r => r.PreferenceTag)
            .Where(t => teamIds.Contains(t.Id))
            .ToListAsync();
        
        var teamMap = teams.ToDictionary(t => t.Id);

        var items = pagedResults.Select(r => new TeamSearchResult
        {
            Team = teamMap[r.TeamId],
            SearchRank = r.SearchRank,
            TeamLeaderNickname = r.LeaderNickname
        }).ToList();

        return new PagedResult<TeamSearchResult>
        {
            Items = items,
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize,
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
        };
    }
}