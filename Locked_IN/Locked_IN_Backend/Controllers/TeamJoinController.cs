using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Locked_IN_Backend.DTOs;
using Locked_IN_Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Locked_IN_Backend.Controllers;

[ApiController]
[Route("api")]
public class TeamJoinController : ControllerBase
{
    private readonly ITeamMemberService _teamMemberService;

    public TeamJoinController(ITeamMemberService teamMemberService)
    {
        _teamMemberService = teamMemberService;
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
}