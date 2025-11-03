using Locked_IN_Backend.Data;
using Locked_IN_Backend.Data.Entities;
using Locked_IN_Backend.DTOs;
using Locked_IN_Backend.Misc.Enum;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Locked_IN_Backend.Controllers
{
    [ApiController] [Route("api")]
    public class TeamJoinController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly TeamSettings _teamSettings;

        public TeamJoinController(AppDbContext context, IOptions<TeamSettings> teamSettings)
        {
            _context = context;
            _teamSettings = teamSettings.Value;

        }
        
        [HttpPost("teams/{teamId}/join")]
        public async Task<IActionResult> RequestToJoinTeam(int teamId, [FromBody] JoinRequestDto joinRequest)
        {
            var team = await _context.Teams.FindAsync(teamId);
            if (team == null)
            {
                return NotFound(new { Message = "Team not found" });
            }
            
            var user = await _context.Users.FindAsync(joinRequest.UserId);
            if (user == null)
            {
                return NotFound(new { Message = "User not found" });
            }
            
            TeamMember? preexistentTeamMember = await _context.TeamMembers
                .Where(tm => tm.TeamId == teamId && tm.UserId == joinRequest.UserId).FirstOrDefaultAsync();
            
            if (preexistentTeamMember is not null)
            {
                Console.WriteLine(preexistentTeamMember.MemberStatusId);
                if (preexistentTeamMember.MemberStatusId== (int)TeamMemberStatus.STATUS_PENDING)
                {
                    return BadRequest(new { Message = "User already has a pending join request" });
                }
                if (preexistentTeamMember.MemberStatusId == (int)TeamMemberStatus.STATUS_REJECTED)
                {
                    return BadRequest(new { Message = "User has been rejected from the team" });
                }
                if (preexistentTeamMember.MemberStatusId is (int)TeamMemberStatus.STATUS_MEMBER or (int)TeamMemberStatus.STATUS_LEADER)
                {
                    return BadRequest(new { Message = "User is already a member of that team" });
                }
            }
            bool isTeamFull = team.MaxPlayerCount <= team.TeamMembers.Count;
            if (isTeamFull)
            {
                return BadRequest(new { Message = "Team is already full" });
            }
            bool hasUserReachedMaxCountJoinRequest = _context.TeamMembers.Count(tm => tm.UserId == joinRequest.UserId) >= _teamSettings.MaxActiveJoinRequestsPerUser;
            if (hasUserReachedMaxCountJoinRequest)
            {
                return BadRequest(new { Message = "User has reached the maximum number of active join requests" });
            }
            
            int newMemberStatusId = team.Isprivate ? (int)TeamMemberStatus.STATUS_PENDING : (int)TeamMemberStatus.STATUS_MEMBER;

            var newTeamMember = new TeamMember
            {
                TeamId = teamId,
                UserId = joinRequest.UserId,
                MemberStatusId = newMemberStatusId,
                Jointimestamp = DateTime.UtcNow,
                Isleader = false 
            };

            _context.TeamMembers.Add(newTeamMember);
            await _context.SaveChangesAsync();
            
            if (newMemberStatusId.Equals(TeamMemberStatus.STATUS_PENDING))
            {
                return Ok(new { Message = "Join request sent successfully. Awaiting admin approval." });
            }
            else
            {
                return Ok(new { Message = "Successfully joined public team." });
            }
        }
        
        [HttpGet("teams/{teamId}/join-requests")]
        public async Task<IActionResult> GetJoinRequests(int teamId)
        {
            var requests = await _context.TeamMembers
                .Where(tm => tm.TeamId == teamId && tm.MemberStatusId.Equals(TeamMemberStatus.STATUS_PENDING))
                .Select(tm => new TeamJoinResponceDto() 
                {
                    TeamMemberId = tm.Id,
                    UserId = tm.UserId,
                    Nickname = tm.User.Nickname, 
                    RequestTimestamp = tm.Jointimestamp
                })
                .ToListAsync();

            return Ok(requests);
        }
        
        [HttpPost("join-requests/{teamMemberId}/accept")]
        public async Task<IActionResult> AcceptJoinRequest(int teamMemberId)
        {
            var request = await _context.TeamMembers
                .FirstOrDefaultAsync(tm => tm.Id == teamMemberId && tm.MemberStatusId.Equals(TeamMemberStatus.STATUS_PENDING));

            if (request == null)
            {
                return NotFound(new { Message = "Join request not found or already handled" });
            }
            
  
            request.MemberStatusId = (int)TeamMemberStatus.STATUS_MEMBER;;
            request.Jointimestamp = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified); 

            _context.TeamMembers.Update(request);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "User approved and added to team" });
        }
        
        [HttpDelete("join-requests/{teamMemberId}/decline")]
        public async Task<IActionResult> DeclineJoinRequest(int teamMemberId)
        {
            var request = await _context.TeamMembers
                .FirstOrDefaultAsync(tm => tm.Id == teamMemberId && tm.MemberStatusId.Equals(TeamMemberStatus.STATUS_MEMBER));

            if (request == null)
            {
                return NotFound(new { Message = "Join request not found or already handled" });
            }
            
            _context.TeamMembers.Remove(request);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Join request declined" });
        }
    }
}