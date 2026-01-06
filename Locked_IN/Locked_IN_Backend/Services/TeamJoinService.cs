using Locked_IN_Backend.Data;
using Locked_IN_Backend.Data.Entities;
using Locked_IN_Backend.DTOs;
using Locked_IN_Backend.Misc.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Locked_IN_Backend.Controllers;

namespace Locked_IN_Backend.Services
{
    public class TeamJoinService : ITeamJoinService
    {
        private readonly AppDbContext _context;
        private readonly TeamSettings _teamSettings;

        public TeamJoinService(AppDbContext context, IOptions<TeamSettings> teamSettings)
        {
            _context = context;
            _teamSettings = teamSettings.Value;
        }

        public async Task<TeamJoinResult> RequestToJoinTeamAsync(int teamId, int userId)
        {
            var team = await _context.Teams.FindAsync(teamId);
            if (team == null)
            {
                return new TeamJoinResult(TeamJoinResultStatus.NotFound, "Team not found");
            }

            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return new TeamJoinResult(TeamJoinResultStatus.NotFound, "User not found");
            }

            var activeMemberCount = await _context.TeamMembers
                .CountAsync(tm => tm.TeamId == teamId &&
                                 (tm.MemberStatusId == (int)TeamMemberStatus.STATUS_MEMBER ||
                                  tm.MemberStatusId == (int)TeamMemberStatus.STATUS_LEADER));

            if (activeMemberCount >= team.MaxPlayerCount)
            {
                return new TeamJoinResult(TeamJoinResultStatus.BadRequest, "Team is already full");
            }

            var preexistentTeamMember = await _context.TeamMembers
                .FirstOrDefaultAsync(tm => tm.TeamId == teamId && tm.UserId == userId);

            if (preexistentTeamMember != null)
            {
                if (preexistentTeamMember.MemberStatusId == (int)TeamMemberStatus.STATUS_PENDING)
                {
                    return new TeamJoinResult(TeamJoinResultStatus.Conflict, "User already has a pending join request");
                }
                if (preexistentTeamMember.MemberStatusId == (int)TeamMemberStatus.STATUS_REJECTED)
                {
                    return new TeamJoinResult(TeamJoinResultStatus.Conflict, "User has been rejected from the team");
                }
                if (preexistentTeamMember.MemberStatusId == (int)TeamMemberStatus.STATUS_MEMBER ||
                    preexistentTeamMember.MemberStatusId == (int)TeamMemberStatus.STATUS_LEADER)
                {
                    return new TeamJoinResult(TeamJoinResultStatus.Conflict, "User is already a member of that team");
                }
            }

            var pendingRequestsCount = await _context.TeamMembers
                .CountAsync(tm => tm.UserId == userId && tm.MemberStatusId == (int)TeamMemberStatus.STATUS_PENDING);

            if (pendingRequestsCount >= _teamSettings.MaxActiveJoinRequestsPerUser)
            {
                return new TeamJoinResult(TeamJoinResultStatus.BadRequest, "User has reached the maximum number of active join requests");
            }

            int newMemberStatusId = team.Isprivate ? (int)TeamMemberStatus.STATUS_PENDING : (int)TeamMemberStatus.STATUS_MEMBER;

            var newTeamMember = new TeamMember
            {
                TeamId = teamId,
                UserId = userId,
                MemberStatusId = newMemberStatusId,
                Jointimestamp = DateTime.UtcNow,
                Isleader = false
            };

            _context.TeamMembers.Add(newTeamMember);
            await _context.SaveChangesAsync();

            string message = newMemberStatusId == (int)TeamMemberStatus.STATUS_PENDING
                ? "Join request sent successfully. Awaiting admin approval."
                : "Successfully joined public team.";
                
            return new TeamJoinResult(TeamJoinResultStatus.Success, message);
        }

        public async Task<List<TeamJoinResponceDto>> GetJoinRequestsAsync(int teamId)
        {
            return await _context.TeamMembers
                .Where(tm => tm.TeamId == teamId && tm.MemberStatusId == (int)TeamMemberStatus.STATUS_PENDING)
                .Select(tm => new TeamJoinResponceDto
                {
                    TeamMemberId = tm.Id,
                    UserId = tm.UserId,
                    Username = tm.User.UserName,
                    RequestTimestamp = tm.Jointimestamp
                })
                .ToListAsync();
        }

        public async Task<TeamJoinResult> AcceptJoinRequestAsync(int teamMemberId)
        {
            var request = await _context.TeamMembers
                .Include(tm => tm.Team)
                .FirstOrDefaultAsync(tm => tm.Id == teamMemberId && tm.MemberStatusId == (int)TeamMemberStatus.STATUS_PENDING);

            if (request == null)
            {
                return new TeamJoinResult(TeamJoinResultStatus.NotFound, "Join request not found or already handled");
            }

            var activeMemberCount = await _context.TeamMembers
                .CountAsync(tm => tm.TeamId == request.TeamId &&
                                 (tm.MemberStatusId == (int)TeamMemberStatus.STATUS_MEMBER ||
                                  tm.MemberStatusId == (int)TeamMemberStatus.STATUS_LEADER));

            if (activeMemberCount >= request.Team.MaxPlayerCount)
            {
                return new TeamJoinResult(TeamJoinResultStatus.BadRequest, "Team is already full");
            }
            
            request.MemberStatusId = (int)TeamMemberStatus.STATUS_MEMBER;
            request.Jointimestamp = DateTime.UtcNow;

            _context.TeamMembers.Update(request);
            await _context.SaveChangesAsync();

            return new TeamJoinResult(TeamJoinResultStatus.Success, "User approved and added to team");
        }

        public async Task<TeamJoinResult> DeclineJoinRequestAsync(int teamMemberId)
        {
            var request = await _context.TeamMembers
                .FirstOrDefaultAsync(tm => tm.Id == teamMemberId && tm.MemberStatusId == (int)TeamMemberStatus.STATUS_PENDING);

            if (request == null)
            {
                return new TeamJoinResult(TeamJoinResultStatus.NotFound, "Join request not found or already handled");
            }

            request.MemberStatusId = (int)TeamMemberStatus.STATUS_REJECTED;

            _context.TeamMembers.Update(request);
            await _context.SaveChangesAsync();

            return new TeamJoinResult(TeamJoinResultStatus.Success, "Join request declined");
        }

        public async Task<TeamJoinResult> CancelJoinRequestAsync(int teamId, int userId)
        {
            var request = await _context.TeamMembers
                .FirstOrDefaultAsync(tm => tm.TeamId == teamId &&
                                            tm.UserId == userId &&
                                            tm.MemberStatusId == (int)TeamMemberStatus.STATUS_PENDING);

            if (request == null)
            {
                return new TeamJoinResult(TeamJoinResultStatus.NotFound, "Pending join request not found for this user and team");
            }

            _context.TeamMembers.Remove(request);
            await _context.SaveChangesAsync();

            return new TeamJoinResult(TeamJoinResultStatus.Success, "Join request successfully cancelled");
        }
    }
}