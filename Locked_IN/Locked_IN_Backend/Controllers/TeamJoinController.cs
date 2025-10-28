using Locked_IN_Backend.Data;
using Locked_IN_Backend.Data.Entities;
using Locked_IN_Backend.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Locked_IN_Backend.Controllers
{
    [ApiController]
    [Route("api")]
    public class TeamJoinController : ControllerBase
    {
        private readonly AppDbContext _context;

        private const int STATUS_LEADER = 1;
        private const int STATUS_MEMBER = 2;
        private const int STATUS_PENDING = 3;

        public TeamJoinController(AppDbContext context)
        {
            _context = context;
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

            bool isAlreadyAffiliated = await _context.TeamMembers
                .AnyAsync(tm => tm.TeamId == teamId && tm.UserId == joinRequest.UserId);

            if (isAlreadyAffiliated)
            {
                return BadRequest(new { Message = "User is already a member or has a pending request" });
            }
            
            int newMemberStatusId = team.Isprivate ? STATUS_PENDING : STATUS_MEMBER;

            var newTeamMember = new TeamMember
            {
                TeamId = teamId,
                UserId = joinRequest.UserId,
                MemberStatusId = newMemberStatusId,
                Jointimestamp = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified),
                Isleader = false 
            };

            _context.TeamMembers.Add(newTeamMember);
            await _context.SaveChangesAsync();
            
            if (newMemberStatusId == STATUS_PENDING)
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
                .Where(tm => tm.TeamId == teamId && tm.MemberStatusId == STATUS_PENDING)
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
                .FirstOrDefaultAsync(tm => tm.Id == teamMemberId && tm.MemberStatusId == STATUS_PENDING);

            if (request == null)
            {
                return NotFound(new { Message = "Join request not found or already handled" });
            }
            
  
            request.MemberStatusId = STATUS_MEMBER;
            request.Jointimestamp = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified); 

            _context.TeamMembers.Update(request);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "User approved and added to team" });
        }
        
        [HttpDelete("join-requests/{teamMemberId}/decline")]
        public async Task<IActionResult> DeclineJoinRequest(int teamMemberId)
        {
            var request = await _context.TeamMembers
                .FirstOrDefaultAsync(tm => tm.Id == teamMemberId && tm.MemberStatusId == STATUS_PENDING);

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