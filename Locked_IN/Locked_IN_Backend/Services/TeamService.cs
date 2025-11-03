using Microsoft.EntityFrameworkCore;
using Locked_IN_Backend.Data;
using Locked_IN_Backend.DTO;
using Locked_IN_Backend.Data.Entities;

namespace Locked_IN_Backend.Services;

public class TeamService : ITeamService
{
    private readonly AppDbContext _context;

    public TeamService(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<GetTeamResponseModel?> GetTeamByIdAsync(int teamId)
    {
        var team = await _context.Teams
            .Where(b => b.Id == teamId)
            .Select(t => new GetTeamResponseModel
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
                Members = t.TeamMembers.Select(tm => new GetTeamMemberResponseModel
                {
                    Id = tm.Id,
                    IsLeader = tm.Isleader,
                    JoinTimestamp = tm.Jointimestamp,
                    TeamId = tm.TeamId,
                    UserId = tm.UserId,
                    MemberStatusId = tm.MemberStatusId,
                    MemberStatusName = tm.MemberStatus.Statusname,
                    User = tm.User != null
                        ? new GetUserResponseModel
                        {
                            Id = tm.User.Id,
                            Email = tm.User.Email,
                            Nickname = tm.User.Nickname,
                            Availability = tm.User.Availability
                        }
                        : null
                }).ToList(),
                PreferenceTags = t.TeamPreferencetagRelations
                    .Select(tpr => tpr.PreferenceTag.Preferencename)
                    .Where(name => !string.IsNullOrEmpty(name))
                    .ToList(),
                CreationTimestamp = t.CreationTimestamp
            })
            .FirstOrDefaultAsync();


        if (team == null)
            return null;
        return team;
    }

    //#TODO replace all the nested TeamMember and GameTag calls with a different service ??
    public async Task<List<GetTeamResponseModel>> GetAllTeamsAsync()
    {
        var teams = await _context.Teams
            .Select(t => new GetTeamResponseModel
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
                Members = t.TeamMembers.Select(tm => new GetTeamMemberResponseModel
                {
                    Id = tm.Id,
                    IsLeader = tm.Isleader,
                    JoinTimestamp = tm.Jointimestamp,
                    TeamId = tm.TeamId,
                    UserId = tm.UserId,
                    MemberStatusId = tm.MemberStatusId,
                    MemberStatusName = tm.MemberStatus.Statusname,
                    User = tm.User != null ? new GetUserResponseModel
                    {
                        Id = tm.User.Id,
                        Email = tm.User.Email,
                        Nickname = tm.User.Nickname,
                        Availability = tm.User.Availability
                    } : null
                }).ToList(),
                PreferenceTags = t.TeamPreferencetagRelations
                    .Select(tpr => tpr.PreferenceTag.Preferencename)
                    .Where(name => !string.IsNullOrEmpty(name))
                    .ToList(),
                CreationTimestamp = t.CreationTimestamp
            })
            .ToListAsync();

        return teams;
    }

    public async Task<List<GetTeamResponseModel>> GetTeamsByGameIdAsync(int gameId)
    {
        var teams = await _context.Teams
            .Where(t => t.GameId == gameId)
            .Select(t => new GetTeamResponseModel
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
                Members = t.TeamMembers.Select(tm => new GetTeamMemberResponseModel
                {
                    Id = tm.Id,
                    IsLeader = tm.Isleader,
                    JoinTimestamp = tm.Jointimestamp,
                    TeamId = tm.TeamId,
                    UserId = tm.UserId,
                    MemberStatusId = tm.MemberStatusId,
                    MemberStatusName = tm.MemberStatus.Statusname,
                    User = tm.User != null
                        ? new GetUserResponseModel
                        {
                            Id = tm.User.Id,
                            Email = tm.User.Email,
                            Nickname = tm.User.Nickname,
                            Availability = tm.User.Availability
                        }
                        : null
                }).ToList(),
                PreferenceTags = t.TeamPreferencetagRelations
                    .Select(tpr => tpr.PreferenceTag.Preferencename)
                    .Where(name => !string.IsNullOrEmpty(name))
                    .ToList(),
                CreationTimestamp = t.CreationTimestamp
            })
            .ToListAsync();

        return teams;
    }
    public async Task<List<GetTeamResponseModel>> GetTeamsByNameSearchAsync(string searchTerm)
    {
        var teams = await _context.Teams
            .Where(t => EF.Functions.ToTsVector("english",t.Name).Matches(EF.Functions.WebSearchToTsQuery(searchTerm)))
            .Select(t => new GetTeamResponseModel
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
                Members = t.TeamMembers.Select(tm => new GetTeamMemberResponseModel
                {
                    Id = tm.Id,
                    IsLeader = tm.Isleader,
                    JoinTimestamp = tm.Jointimestamp,
                    TeamId = tm.TeamId,
                    UserId = tm.UserId,
                    MemberStatusId = tm.MemberStatusId,
                    MemberStatusName = tm.MemberStatus.Statusname,
                    User = tm.User != null
                        ? new GetUserResponseModel
                        {
                            Id = tm.User.Id,
                            Email = tm.User.Email,
                            Nickname = tm.User.Nickname,
                            Availability = tm.User.Availability
                        }
                        : null
                }).ToList(),
                PreferenceTags = t.TeamPreferencetagRelations
                    .Select(tpr => tpr.PreferenceTag.Preferencename)
                    .Where(name => !string.IsNullOrEmpty(name))
                    .ToList(),
                CreationTimestamp = t.CreationTimestamp,
                SearchRank = EF.Functions.ToTsVector("english",t.Name).Rank(EF.Functions.WebSearchToTsQuery(searchTerm))
            })
            .OrderByDescending(t => t.SearchRank)
            .ToListAsync();

        return teams;
    }
}