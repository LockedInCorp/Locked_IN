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
            .Select(b => new GetTeamResponseModel
            {
                Id = b.Id,
                Name = b.Name,
                MinCompScore = b.MinCompScore,
                MaxPlayerCount = b.MaxPlayerCount,
                Description = b.Description,
                GameId = b.GameId,
                GameName = b.Game.Name,
                IsPrivate = b.Isprivate,
                IsBlitz = b.Isblitz,
                ExperienceTagId = b.ExperienceTagId,
                ExperienceLevel = b.ExperienceTag.Experiencelevel,
                CurrentMemberCount = b.TeamMembers.Count,
                Members = b.TeamMembers.Select(tm => new GetTeamMemberResponseModel
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
                PreferenceTags = b.TeamPreferencetagRelations
                    .Select(tpr => tpr.PreferenceTag.Preferencename)
                    .Where(name => !string.IsNullOrEmpty(name))
                    .ToList()
            })
            .FirstOrDefaultAsync();


        if (team == null)
            return null;
        return team;
    }

    public async Task<List<GetTeamResponseModel>> GetAllTeamsAsync()
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

    public async Task<List<GetTeamResponseModel>> GetTeamsByGameIdAsync(int gameId)
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
}