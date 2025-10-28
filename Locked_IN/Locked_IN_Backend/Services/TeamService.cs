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

    public async Task<GetTeamResponse?> GetTeamByIdAsync(int teamId)
    {
        var team = await _context.Teams
            .Include(t => t.Game)
            .Include(t => t.ExperienceTag)
            .Include(t => t.TeamMembers)
                .ThenInclude(tm => tm.User)
            .Include(t => t.TeamMembers)
                .ThenInclude(tm => tm.MemberStatus)
            .Include(t => t.TeamPreferencetagRelations)
                .ThenInclude(tpr => tpr.PreferenceTag)
            .FirstOrDefaultAsync(t => t.Id == teamId);

        if (team == null)
            return null;

        
        return MapToGetTeamResponse(team);
    }

    public async Task<List<GetTeamResponse>> GetAllTeamsAsync()
    {
        var teams = await _context.Teams
            .Include(t => t.Game)
            .Include(t => t.ExperienceTag)
            .Include(t => t.TeamMembers)
                .ThenInclude(tm => tm.User)
            .Include(t => t.TeamMembers)
                .ThenInclude(tm => tm.MemberStatus)
            .Include(t => t.TeamPreferencetagRelations)
                .ThenInclude(tpr => tpr.PreferenceTag)
            .ToListAsync();

        return teams.Select(MapToGetTeamResponse).ToList();
    }

    public async Task<List<GetTeamResponse>> GetTeamsByGameIdAsync(int gameId)
    {
        var teams = await _context.Teams
            .Where(t => t.GameId == gameId)
            .Include(t => t.Game)
            .Include(t => t.ExperienceTag)
            .Include(t => t.TeamMembers)
                .ThenInclude(tm => tm.User)
            .Include(t => t.TeamMembers)
                .ThenInclude(tm => tm.MemberStatus)
            .Include(t => t.TeamPreferencetagRelations)
                .ThenInclude(tpr => tpr.PreferenceTag)
            .ToListAsync();

        return teams.Select(MapToGetTeamResponse).ToList();
    }

    private static GetTeamResponse MapToGetTeamResponse(Team team)
    {
        return new GetTeamResponse
        {
            Id = team.Id,
            Name = team.Name,
            MinCompScore = team.MinCompScore,
            MaxPlayerCount = team.MaxPlayerCount,
            Description = team.Description,
            GameId = team.GameId,
            GameName = team.Game?.Name,
            IsPrivate = team.Isprivate,
            IsBlitz = team.Isblitz,
            ExperienceTagId = team.ExperienceTagId,
            ExperienceLevel = team.ExperienceTag.Experiencelevel,
            CurrentMemberCount = team.TeamMembers.Count,
            Members = team.TeamMembers.Select(tm => new GetTeamMemberResponse
            {
                Id = tm.Id,
                IsLeader = tm.Isleader,
                JoinTimestamp = tm.Jointimestamp,
                TeamId = tm.TeamId,
                UserId = tm.UserId,
                MemberStatusId = tm.MemberStatusId,
                MemberStatusName = tm.MemberStatus.Statusname,
                User = tm.User != null ? new GetUserResponse
                {
                    Id = tm.User.Id,
                    Email = tm.User.Email,
                    Nickname = tm.User.Nickname,
                    Availability = tm.User.Availability
                } : null
            }).ToList(),
            PreferenceTags = team.TeamPreferencetagRelations
                .Select(tpr => tpr.PreferenceTag.Preferencename)
                .Where(name => !string.IsNullOrEmpty(name))
                .ToList()!
        };
    }
}