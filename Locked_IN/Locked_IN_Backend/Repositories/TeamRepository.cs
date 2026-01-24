using Locked_IN_Backend.Data;
using Locked_IN_Backend.Data.Entities;
using Locked_IN_Backend.DTOs;
using Locked_IN_Backend.Interfaces.Repositories;
using Locked_IN_Backend.Misc;
using Locked_IN_Backend.Misc.Enum;
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
            var trimmedSearch = searchTerm.Trim();
            var searchArray = trimmedSearch.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var searchWithStar = string.Join(" & ", searchArray) + ":*";
            
            query = query.Where(t => EF.Functions.ToTsVector("english", t.Name).Matches(EF.Functions.ToTsQuery("english", searchWithStar)));
        }

        var results = await query
            .Select(t => new
            {
                Id = t.Id,
                Rank = !string.IsNullOrWhiteSpace(searchTerm)
                    ? EF.Functions.ToTsVector("english", t.Name).Rank(EF.Functions.ToTsQuery("english", string.Join(" & ", searchTerm.Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)) + ":*"))
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
    
    public async Task<PagedResult<TeamSearchResult>> GetTeamsAdvancedAsync(List<int> gameIds, List<int> preferenceTagIds, string searchTerm, int page, int pageSize, string sortBy, List<int> teamIds)
    {
        var query = _context.Teams.AsQueryable();

        query = query.Where(t => t.TeamMembers.Count(tm => 
            tm.MemberStatusId == (int)TeamMemberStatus.STATUS_LEADER || 
            tm.MemberStatusId == (int)TeamMemberStatus.STATUS_MEMBER) < t.MaxPlayerCount);

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
        string searchWithStar = "";
        if (hasSearch)
        {
            var trimmedSearch = searchTerm.Trim();
            var searchArray = trimmedSearch.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            searchWithStar = string.Join(" & ", searchArray) + ":*";
            
            query = query.Where(t => EF.Functions.ToTsVector("english", t.Name)
                .Matches(EF.Functions.ToTsQuery("english", searchWithStar)));
        }

        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = ValidationConstraints.DefaultPageSize;

        var totalCount = await query.CountAsync();

        var intermediate = query.Select(t => new
        {
            TeamId = t.Id,
            SearchRank = hasSearch
                ? EF.Functions.ToTsVector("english", t.Name)
                    .Rank(EF.Functions.ToTsQuery("english", searchWithStar))
                : 0,
            LatestMemberJoin = t.TeamMembers
                .OrderByDescending(tm => tm.Jointimestamp)
                .Select(tm => (DateTime?)tm.Jointimestamp)
                .FirstOrDefault(),
            TeamLeaderUsername = t.TeamMembers
                .Where(tm => tm.Isleader)
                .Select(tm => tm.User.UserName)
                .FirstOrDefault(),
            CreationTimestamp = t.CreationTimestamp,
            Team = t
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
                .OrderByDescending(x => x.Team.TeamMembers.Count)
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
        
        var teamIntermediateIds = pagedResults.Select(r => r.TeamId).ToList();
        if (teamIds.Count > 0)
        {
            teamIntermediateIds = teamIntermediateIds.Intersect(teamIds).ToList();
        }

        var teams = await _context.Teams
            .Include(t => t.Game)
            .Include(t => t.ExperienceTag)
            .Include(t => t.TeamMembers)
                .ThenInclude(tm => tm.User)
            .Include(t => t.TeamPreferencetagRelations)
                .ThenInclude(r => r.PreferenceTag)
            .Where(t => teamIntermediateIds.Contains(t.Id))
            .ToListAsync();
        
        var teamMap = teams.ToDictionary(t => t.Id);

        var items = pagedResults.Select(r => new TeamSearchResult
        {
            Team = teamMap[r.TeamId],
            SearchRank = r.SearchRank,
            TeamLeaderUsername = r.TeamLeaderUsername
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