using Microsoft.EntityFrameworkCore;
using Locked_IN_Backend.Data;
using Locked_IN_Backend.DTO;
using Locked_IN_Backend.Data.Entities;
using Locked_IN_Backend.DTOs;
using Locked_IN_Backend.Interfaces;

namespace Locked_IN_Backend.Services;

public class TeamService : ITeamService
{
    private readonly AppDbContext _context;

    public TeamService(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<GetTeamDto?> GetTeamByIdAsync(int teamId)
    {
        var team = await _context.Teams
            .Where(b => b.Id == teamId)
            .Select(t => new GetTeamDto
            {
                Id = t.Id,
                Name = t.Name,
                MinCompScore = t.MinCompScore,
                MaxPlayerCount = t.MaxPlayerCount,
                Description = t.Description,
                GameId = t.GameId,
                GameName = t.Game.Name,
                IsPrivate = t.Isprivate,
                IsBlitz = t.Isblitz,
                ExperienceTagId = t.ExperienceTagId,
                ExperienceLevel = t.ExperienceTag.Experiencelevel,
                CurrentMemberCount = t.TeamMembers.Count,
                Members = t.TeamMembers.Select(tm => new GetTeamMemberDto
                {
                    Id = tm.Id,
                    IsLeader = tm.Isleader,
                    JoinTimestamp = tm.Jointimestamp,
                    TeamId = tm.TeamId,
                    UserId = tm.UserId,
                    MemberStatusId = tm.MemberStatusId,
                    MemberStatusName = tm.MemberStatus.Statusname,
                    User = tm.User != null
                        ? new GetUserForTeamViewDto
                        {
                            Id = tm.User.Id,
                            Email = tm.User.Email,
                            Nickname = tm.User.Nickname,
                            Availability = tm.User.Availability
                        }
                        : null
                }).ToList(),
                PreferenceTags = t.TeamPreferencetagRelations
                    .Select(tpr => tpr.PreferenceTag.Name)
                    .Where(name => !string.IsNullOrEmpty(name))
                    .ToList(),
                CreationTimestamp = t.CreationTimestamp,
                IconUrl = t.IconUrl
            })
            .FirstOrDefaultAsync();


        if (team == null)
            return null;
        return team;
    }

    //#TODO replace all the nested TeamMember and GameTag calls with a different service ??

    public async Task<List<GetTeamDto>> GetTeamsByGameIdAsync(int gameId)
    {
        var teams = await _context.Teams
            .Where(t => t.GameId == gameId)
            .Select(t => new GetTeamDto
            {
                Id = t.Id,
                Name = t.Name,
                MinCompScore = t.MinCompScore,
                MaxPlayerCount = t.MaxPlayerCount,
                Description = t.Description,
                GameId = t.GameId,
                GameName = t.Game.Name,
                IsPrivate = t.Isprivate,
                IsBlitz = t.Isblitz,
                ExperienceTagId = t.ExperienceTagId,
                ExperienceLevel = t.ExperienceTag.Experiencelevel,
                CurrentMemberCount = t.TeamMembers.Count,
                Members = t.TeamMembers.Select(tm => new GetTeamMemberDto
                {
                    Id = tm.Id,
                    IsLeader = tm.Isleader,
                    JoinTimestamp = tm.Jointimestamp,
                    TeamId = tm.TeamId,
                    UserId = tm.UserId,
                    MemberStatusId = tm.MemberStatusId,
                    MemberStatusName = tm.MemberStatus.Statusname,
                    User = tm.User != null
                        ? new GetUserForTeamViewDto
                        {
                            Id = tm.User.Id,
                            Email = tm.User.Email,
                            Nickname = tm.User.Nickname,
                            Availability = tm.User.Availability
                        }
                        : null
                }).ToList(),
                PreferenceTags = t.TeamPreferencetagRelations
                    .Select(tpr => tpr.PreferenceTag.Name)
                    .Where(name => !string.IsNullOrEmpty(name))
                    .ToList(),
                CreationTimestamp = t.CreationTimestamp,
                IconUrl = t.IconUrl
            })
            .ToListAsync();

        return teams;
    }
    public async Task<List<GetTeamDto>> GetTeamsByNameSearchAsync(string searchTerm = "")
    {
    var query = _context.Teams.AsQueryable();
    
    if (!string.IsNullOrWhiteSpace(searchTerm))
    {
        query = query.Where(t => EF.Functions.ToTsVector("english", t.Name).Matches(EF.Functions.WebSearchToTsQuery(searchTerm)));
    }
    
    var teams = await query
        .Select(t => new GetTeamDto
        {
            Id = t.Id,
            Name = t.Name,
            MinCompScore = t.MinCompScore,
            MaxPlayerCount = t.MaxPlayerCount,
            Description = t.Description,
            GameId = t.GameId,
            GameName = t.Game.Name,
            IsPrivate = t.Isprivate,
            IsBlitz = t.Isblitz,
            ExperienceTagId = t.ExperienceTagId,
            ExperienceLevel = t.ExperienceTag.Experiencelevel,
            CurrentMemberCount = t.TeamMembers.Count,
            Members = t.TeamMembers.Select(tm => new GetTeamMemberDto
            {
                Id = tm.Id,
                IsLeader = tm.Isleader,
                JoinTimestamp = tm.Jointimestamp,
                TeamId = tm.TeamId,
                UserId = tm.UserId,
                MemberStatusId = tm.MemberStatusId,
                MemberStatusName = tm.MemberStatus.Statusname,
                User = tm.User != null
                    ? new GetUserForTeamViewDto
                    {
                        Id = tm.User.Id,
                        Email = tm.User.Email,
                        Nickname = tm.User.Nickname,
                        Availability = tm.User.Availability
                    }
                    : null
            }).ToList(),
            PreferenceTags = t.TeamPreferencetagRelations
                .Select(tpr => tpr.PreferenceTag.Name)
                .Where(name => !string.IsNullOrEmpty(name))
                .ToList(),
            CreationTimestamp = t.CreationTimestamp,
            IconUrl = t.IconUrl,
            SearchRank = !string.IsNullOrWhiteSpace(searchTerm) 
                ? EF.Functions.ToTsVector("english", t.Name).Rank(EF.Functions.WebSearchToTsQuery(searchTerm))
                : 0
        })
        .OrderByDescending(t => t.SearchRank)
        .ToListAsync();

    return teams;
}

    public async Task<PagedResult<GetTeamsCardDto>> SearchTeamsAdvancedAsync(List<int> gameIds, List<int> preferenceTagIds, string searchTerm, int page, int pageSize, string sortBy)
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
            Team = t,
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
                .FirstOrDefault()
        });

        var sort = (sortBy ?? string.Empty).Trim().ToLower();
        if (sort == "relevance")
        {
            intermediate = intermediate
                .OrderByDescending(x => x.SearchRank)
                .ThenByDescending(x => x.Team.CreationTimestamp);
        }
        else if (sort == "popular")
        {
            intermediate = intermediate
                .OrderByDescending(x => x.LatestMemberJoin)
                .ThenByDescending(x => x.Team.CreationTimestamp);
        }
        else if (sort == "newest")
        {
            intermediate = intermediate
                .OrderByDescending(x => x.Team.CreationTimestamp);
        }
        else
        {
            if (hasSearch)
            {
                intermediate = intermediate
                    .OrderByDescending(x => x.SearchRank)
                    .ThenByDescending(x => x.Team.CreationTimestamp);
            }
            else
            {
                intermediate = intermediate
                    .OrderByDescending(x => x.Team.CreationTimestamp);
            }
        }

        var items = await intermediate
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new GetTeamsCardDto
            {
                Id = x.Team.Id,
                Name = x.Team.Name,
                MinCompScore = x.Team.MinCompScore,
                MaxPlayerCount = x.Team.MaxPlayerCount,
                Description = x.Team.Description,
                GameId = x.Team.GameId,
                GameName = x.Team.Game.Name,
                IsPrivate = x.Team.Isprivate,
                IsBlitz = x.Team.Isblitz,
                ExperienceTagId = x.Team.ExperienceTagId,
                ExperienceLevel = x.Team.ExperienceTag.Experiencelevel,
                CurrentMemberCount = x.Team.TeamMembers.Count,
                PreferenceTags = x.Team.TeamPreferencetagRelations
                    .Select(tpr => tpr.PreferenceTag.Name)
                    .Where(name => !string.IsNullOrEmpty(name))
                    .ToList(),
                CreationTimestamp = x.Team.CreationTimestamp,
                IconUrl = x.Team.IconUrl,
                SearchRank = x.SearchRank,
                TeamLeaderNickname = x.LeaderNickname
            })
            .ToListAsync();

        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        return new PagedResult<GetTeamsCardDto>
        {
            Items = items,
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize,
            TotalPages = totalPages
        };
    }

}