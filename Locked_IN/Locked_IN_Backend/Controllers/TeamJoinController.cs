using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Locked_IN_Backend.DTOs;
using Locked_IN_Backend.Interfaces;
using Locked_IN_Backend.Services;
using Locked_IN_Backend.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Locked_IN_Backend.Controllers;

[ApiController]
[Route("api")]
public class TeamJoinController : ControllerBase
{
    private readonly ITeamMemberService _teamMemberService;
    private readonly IInviteTokenService _inviteTokenService;
    private readonly IChatService _chatService;

    public TeamJoinController(ITeamMemberService teamMemberService, IInviteTokenService inviteTokenService, IChatService chatService)
    {
        _teamMemberService = teamMemberService;
        _inviteTokenService = inviteTokenService;
        _chatService = chatService;
    }

    /// <summary>
    /// Request to join a specific team.
    /// </summary>
    /// <param name="teamId">The ID of the team to join.</param>
    /// <returns>Confirmation message.</returns>
    [Authorize]
    [HttpPost("teams/{teamId}/join")]
    public async Task<IActionResult> RequestToJoinTeam(int teamId)
    {
        var userIdClaim = User.FindFirstValue(JwtRegisteredClaimNames.Sub);
        if (string.IsNullOrEmpty(userIdClaim)) return Unauthorized();
        var userId = int.Parse(userIdClaim);

        await _teamMemberService.RequestToJoinTeamAsync(teamId, userId);
        
        return Ok(new { Message = "Action completed successfully." });
    }

    /// <summary>
    /// Get all pending join requests for a team (Leader only).
    /// </summary>
    /// <param name="teamId">The ID of the team.</param>
    /// <returns>A list of pending join requests.</returns>
    [Authorize]
    [HttpGet("teams/{teamId}/join-requests")]
    public async Task<ActionResult<List<TeamJoinResponceDto>>> GetJoinRequests(int teamId)
    {
        var userIdClaim = User.FindFirstValue(JwtRegisteredClaimNames.Sub);
        if (string.IsNullOrEmpty(userIdClaim)) return Unauthorized();
        var userId = int.Parse(userIdClaim);

        var requests = await _teamMemberService.GetJoinRequestsAsync(teamId, userId);
        return Ok(requests);
    }

    /// <summary>
    /// Accept a pending join request (Leader only).
    /// </summary>
    /// <param name="teamId">The ID of the team.</param>
    /// <param name="userId">The ID of the user to accept.</param>
    /// <returns>Confirmation message.</returns>
    [Authorize]
    [HttpPut("teams/{teamId}/users/{userId}/accept")]
    public async Task<IActionResult> AcceptJoinRequest(int teamId, int userId)
    {
        var currentUserIdClaim = User.FindFirstValue(JwtRegisteredClaimNames.Sub);
        if (string.IsNullOrEmpty(currentUserIdClaim)) return Unauthorized();
        var currentUserId = int.Parse(currentUserIdClaim);

        await _teamMemberService.AcceptJoinRequestAsync(currentUserId, teamId, userId);

        return Ok(new { Message = "User approved and added to team." });
    }

    /// <summary>
    /// Decline a pending join request (Leader only).
    /// </summary>
    /// <param name="teamId">The ID of the team.</param>
    /// <param name="userId">The ID of the user to decline.</param>
    /// <returns>Confirmation message.</returns>
    [Authorize]
    [HttpPut("teams/{teamId}/users/{userId}/decline")]
    public async Task<IActionResult> DeclineJoinRequest(int teamId, int userId)
    {
        var currentUserIdClaim = User.FindFirstValue(JwtRegisteredClaimNames.Sub);
        if (string.IsNullOrEmpty(currentUserIdClaim)) return Unauthorized();
        var currentUserId = int.Parse(currentUserIdClaim);

        await _teamMemberService.DeclineJoinRequestAsync(currentUserId, teamId, userId);

        return Ok(new { Message = "Join request declined." });
    }

    /// <summary>
    /// Cancel a user's own pending join request.
    /// </summary>
    /// <param name="teamId">The ID of the team.</param>
    /// <returns>Confirmation message.</returns>
    [Authorize]
    [HttpDelete("teams/{teamId}/cancel-join")]
    public async Task<IActionResult> CancelJoinRequest(int teamId)
    {
        var userIdClaim = User.FindFirstValue(JwtRegisteredClaimNames.Sub);
        if (string.IsNullOrEmpty(userIdClaim)) return Unauthorized();
        var userId = int.Parse(userIdClaim);

        await _teamMemberService.CancelJoinRequestAsync(teamId, userId);

        return Ok(new { Message = "Join request successfully cancelled." });
    }

    /// <summary>
    /// Leave a team.
    /// </summary>
    /// <param name="teamId">The ID of the team to leave.</param>
    /// <returns>Confirmation message.</returns>
    [Authorize]
    [HttpPost("teams/{teamId}/leave")]
    public async Task<IActionResult> LeaveTeam(int teamId)
    {
        var userIdClaim = User.FindFirstValue(JwtRegisteredClaimNames.Sub);
        if (string.IsNullOrEmpty(userIdClaim)) return Unauthorized();
        var userId = int.Parse(userIdClaim);

        await _teamMemberService.LeaveTeamAsync(teamId, userId);

        return Ok(new { Message = "Successfully left the team and its associated chat." });
    }

    /// <summary>
    /// Kick a member from a team (Leader only).
    /// </summary>
    /// <param name="teamId">The ID of the team.</param>
    /// <param name="userId">The ID of the user to kick.</param>
    /// <returns>Confirmation message.</returns>
    [Authorize]
    [HttpDelete("teams/{teamId}/users/{userId}/kick")]
    public async Task<IActionResult> KickMember(int teamId, int userId)
    {
        var leaderIdClaim = User.FindFirstValue(JwtRegisteredClaimNames.Sub);
        if (string.IsNullOrEmpty(leaderIdClaim)) return Unauthorized();
        var leaderId = int.Parse(leaderIdClaim);

        await _teamMemberService.KickMemberAsync(leaderId, teamId, userId);

        return Ok(new { Message = "User successfully kicked from the team." });
    }

    [HttpGet("teams/{teamId}/members")]
    public async Task<IActionResult> GetActiveTeamMembers(int teamId)
    {
        var members = await _teamMemberService.GetActiveTeamMembersAsync(teamId);
        return Ok(members);
    }

    /// <summary>
    /// Generate an invite token for a team (Leader only).
    /// </summary>
    [Authorize]
    [HttpGet("teams/{teamId}/invite-token")]
    public async Task<IActionResult> GetInviteToken(int teamId)
    {
        var userIdClaim = User.FindFirstValue(JwtRegisteredClaimNames.Sub);
        if (string.IsNullOrEmpty(userIdClaim)) return Unauthorized();
        var userId = int.Parse(userIdClaim);
        
        await _teamMemberService.CheckInviteAvailablitity(teamId, userId);
        
        var token = _inviteTokenService.GenerateInviteToken(teamId);
        return Ok(new { Token = token });
    }

    /// <summary>
    /// Join a team using an invite token.
    /// </summary>
    [Authorize]
    [HttpPost("teams/join-with-token")]
    public async Task<IActionResult> JoinTeamWithToken([FromQuery] string token)
    {
        var userIdClaim = User.FindFirstValue(JwtRegisteredClaimNames.Sub);
        if (string.IsNullOrEmpty(userIdClaim)) return Unauthorized();
        var userId = int.Parse(userIdClaim);

        var teamId = _inviteTokenService.DecodeInviteToken(token);
        var chatId = await _teamMemberService.JoinTeamDirectlyAsync(teamId, userId);

        return Ok(new { Message = "Successfully joined the team via invite token.", TeamId = teamId, ChatId = chatId });
    }
}