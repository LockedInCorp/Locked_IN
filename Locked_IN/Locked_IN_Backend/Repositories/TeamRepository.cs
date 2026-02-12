using Locked_IN_Backend.Data;
using Locked_IN_Backend.Data.Entities;
using Locked_IN_Backend.DTOs;
using Locked_IN_Backend.Interfaces.Repositories;
using Locked_IN_Backend.Misc;
using Locked_IN_Backend.Misc.Enum;
using Microsoft.EntityFrameworkCore;

using Microsoft.EntityFrameworkCore.Storage;

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
        return await _context.Teams.Include(t => t.Chats).FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task AddTeam(Team team)
    {
         await _context.Teams.AddAsync(team);
         await Task.CompletedTask;
    }

    public async Task UpdateTeam(Team team)
    {
        _context.Teams.Update(team);
        await Task.CompletedTask;
    }

    public async Task DeleteTeam(Team team)
    {
        _context.Teams.Remove(team);
        await Task.CompletedTask;
    }
    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        return await _context.Database.BeginTransactionAsync();
    }

    public async Task RemoveTeamCommunicationService(TeamCommunicationService tcs)
    {
        _context.TeamCommunicationServices.Remove(tcs);
        await Task.CompletedTask;
    }

    public async Task RemoveTeamPreferencetagRelations(IEnumerable<TeamPreferencetagRelation> relations)
    {
        _context.TeamPreferencetagRelations.RemoveRange(relations);
        await Task.CompletedTask;
    }

    public async Task<Team?> GetTeamWithDetailsByIdAsync(int teamId)
    {
        return await _context.Teams
            .Include(t => t.Game)
            .Include(t => t.Chats)
            .Include(t => t.TeamCommunicationService)
            .ThenInclude(tcs => tcs!.CommunicationService)
            .Include(t => t.ExperienceTag)
            .Include(t => t.TeamMembers.Where(tm => tm.MemberStatusId == (int)TeamMemberStatus.STATUS_LEADER || tm.MemberStatusId == (int)TeamMemberStatus.STATUS_MEMBER))
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
            
            query = query.Where(t => !t.Isprivate).Where(t => EF.Functions.ToTsVector("english", t.Name).Matches(EF.Functions.ToTsQuery("english", searchWithStar)));
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
    
    public async Task<PagedResult<TeamSearchResult>> GetTeamsAdvancedAsync(List<int> gameIds, List<int> preferenceTagIds, string searchTerm, int page, int pageSize, string sortBy, int userId, bool OnlyShowPending)
    {
        var query = _context.Teams.AsQueryable();

        query = ApplyBaseFilters(query, userId, OnlyShowPending);
        query = ApplyGameFilters(query, gameIds);
        query = ApplyPreferenceTagFilters(query, preferenceTagIds);
        query = ApplySearchFilter(query, searchTerm, out var searchWithStar);

        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = ValidationConstraints.DefaultPageSize;

        var totalCount = await query.CountAsync();

        var hasSearch = !string.IsNullOrWhiteSpace(searchTerm);
        var intermediate = query.Select(t => new TeamIntermediateResult
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

        intermediate = ApplySorting(intermediate, sortBy, hasSearch);

        var pagedResults = await intermediate
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        
        var teamIntermediateIds = pagedResults.Select(r => r.TeamId).ToList();

        var teams = await _context.Teams
            .Include(t => t.Game)
            .Include(t => t.ExperienceTag)
            .Include(t => t.TeamMembers)
            .ThenInclude(tm => tm.User)
            .Include(t => t.TeamPreferencetagRelations)
            .ThenInclude(r => r.PreferenceTag)
            .Include(team => team.TeamMembers)
            .ThenInclude(teamMember => teamMember.MemberStatus)
            .Include(t => t.Chats)
            .Where(t => teamIntermediateIds.Contains(t.Id))
            .ToListAsync();
        
        var teamMap = teams.ToDictionary(t => t.Id);

        var items = pagedResults.Select(r =>
        {
            var team = teamMap[r.TeamId];
            var currentUserMember = team.TeamMembers.FirstOrDefault(u => u.UserId == userId);
            
            return new TeamSearchResult
            {
                Team = team,
                SearchRank = r.SearchRank,
                TeamLeaderUsername = r.TeamLeaderUsername,
                TeamMemberStatus = currentUserMember != null 
                    ? (TeamMemberStatus)currentUserMember.MemberStatusId 
                    : TeamMemberStatus.STATUS_NONE
            };
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

    private IQueryable<Team> ApplyBaseFilters(IQueryable<Team> query, int userId, bool onlyShowPending)
    {
        query = query.Where(t => t.TeamMembers.Count(tm => 
            tm.MemberStatusId == (int)TeamMemberStatus.STATUS_LEADER || 
            tm.MemberStatusId == (int)TeamMemberStatus.STATUS_MEMBER) < t.MaxPlayerCount);
        
        return query.Where(t => !t.Isprivate)
            .Where(t => (!onlyShowPending || t.TeamMembers.Any(tm => tm.MemberStatusId == (int)TeamMemberStatus.STATUS_PENDING && tm.UserId == userId)) && !t.TeamMembers.Any(tm => tm.MemberStatusId == (int)TeamMemberStatus.STATUS_REJECTED && tm.UserId == userId));
    }

    private IQueryable<Team> ApplyGameFilters(IQueryable<Team> query, List<int> gameIds)
    {
        if (gameIds.Any())
        {
            query = query.Where(t => gameIds.Contains(t.GameId));
        }
        return query;
    }

    private IQueryable<Team> ApplyPreferenceTagFilters(IQueryable<Team> query, List<int> preferenceTagIds)
    {
        if (preferenceTagIds.Any())
        {
            query = query.Where(t => t.TeamPreferencetagRelations
                .Any(rel => preferenceTagIds.Contains(rel.PreferenceTagId)));
        }
        return query;
    }

    private IQueryable<Team> ApplySearchFilter(IQueryable<Team> query, string searchTerm, out string searchWithStar)
    {
        searchWithStar = string.Empty;
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var trimmedSearch = searchTerm.Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var localSearchWithStar = string.Join(" & ", trimmedSearch) + ":*";
            searchWithStar = localSearchWithStar;
            
            query = query.Where(t => EF.Functions.ToTsVector("english", t.Name)
                .Matches(EF.Functions.ToTsQuery("english", localSearchWithStar)));
        }
        return query;
    }

    private IQueryable<TeamIntermediateResult> ApplySorting(IQueryable<TeamIntermediateResult> intermediate, string sortBy, bool hasSearch)
    {
        var sort = (sortBy ?? string.Empty).Trim().ToLower();
        if (sort == "relevance")
        {
            return intermediate
                .OrderByDescending(x => x.SearchRank)
                .ThenByDescending(x => x.CreationTimestamp);
        }
        
        if (sort == "popular")
        {
            return intermediate
                .OrderByDescending(x => x.Team.TeamMembers.Count)
                .ThenByDescending(x => x.CreationTimestamp);
        }
        
        if (sort == "newest")
        {
            return intermediate
                .OrderByDescending(x => x.CreationTimestamp);
        }
        
        if (hasSearch)
        {
            return intermediate
                .OrderByDescending(x => x.SearchRank)
                .ThenByDescending(x => x.CreationTimestamp);
        }
        
        return intermediate
            .OrderByDescending(x => x.CreationTimestamp);
    }

    private class TeamIntermediateResult
    {
        public int TeamId { get; set; }
        public float SearchRank { get; set; }
        public DateTime? LatestMemberJoin { get; set; }
        public string? TeamLeaderUsername { get; set; }
        public DateTime CreationTimestamp { get; set; }
        public Team Team { get; set; } = null!;
    }
}