using Locked_IN_Backend.DTOs;
using Locked_IN_Backend.Services;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;

namespace Locked_IN_Backend.Controllers
{
    [ApiController]
    [Route("api")]
    public class TeamJoinController : ControllerBase
    {
        private readonly ITeamMemberService _teamMemberService;
        private readonly IValidator<JoinRequestDto> _joinRequestValidator;

        public TeamJoinController(ITeamMemberService teamMemberService, IValidator<JoinRequestDto> joinRequestValidator)
        {
            _teamMemberService = teamMemberService;
            _joinRequestValidator = joinRequestValidator;
        }

        /// <summary>
        /// Request to join a specific team
        /// </summary>
        /// <param name="teamId">The ID of the team to join</param>
        /// <param name="joinRequest">DTO containing the User ID</param>
        /// <returns>Confirmation or error message</returns>
        [HttpPost("teams/{teamId}/join")]
        public async Task<IActionResult> RequestToJoinTeam(int teamId, [FromBody] JoinRequestDto joinRequest)
        {
            var validationResult = await _joinRequestValidator.ValidateAsync(joinRequest);
            if (!validationResult.IsValid)
            {
                return BadRequest(new { Message = validationResult.Errors.First().ErrorMessage });
            }

            await _teamMemberService.RequestToJoinTeamAsync(teamId, joinRequest.UserId);
            
            return Ok(new { Message = "Action completed successfully" });
        }

        /// <summary>
        /// Get all pending join requests for a team
        /// </summary>
        /// <param name="teamId">The ID of the team</param>
        /// <returns>A list of pending join requests</returns>
        [HttpGet("teams/{teamId}/join-requests")]
        public async Task<IActionResult> GetJoinRequests(int teamId)
        {
            var requests = await _teamMemberService.GetJoinRequestsAsync(teamId);
            return Ok(requests);
        }

        /// <summary>
        /// Accept a pending join request
        /// </summary>
        /// <param name="teamId">The ID of the team</param>
        /// <param name="userId">The ID of the user</param>
        /// <returns>Confirmation or error message</returns>
        [HttpPost("teams/{teamId}/users/{userId}/accept")]
        public async Task<IActionResult> AcceptJoinRequest(int teamId, int userId)
        {
            await _teamMemberService.AcceptJoinRequestAsync(teamId, userId);

            return Ok(new { Message = "User approved and added to team" });
        }

        /// <summary>
        /// Decline a pending join request
        /// </summary>
        /// <param name="teamId">The ID of the team</param>
        /// <param name="userId">The ID of the user</param>
        /// <returns>Confirmation or error message</returns>
        [HttpPost("teams/{teamId}/users/{userId}/decline")]
        public async Task<IActionResult> DeclineJoinRequest(int teamId, int userId)
        {
            await _teamMemberService.DeclineJoinRequestAsync(teamId, userId);

            return Ok(new { Message = "Join request declined" });
        }

        /// <summary>
        /// Cancel a user's own pending join request
        /// </summary>
        /// <param name="teamId">The ID of the team</param>
        /// <param name="cancelRequest">DTO containing the User ID</param>
        /// <returns>Confirmation or error message</returns>
        [HttpDelete("teams/{teamId}/cancel-join")]
        public async Task<IActionResult> CancelJoinRequest(int teamId, [FromBody] JoinRequestDto cancelRequest)
        {
            var validationResult = await _joinRequestValidator.ValidateAsync(cancelRequest);
            if (!validationResult.IsValid)
            {
                return BadRequest(new { Message = validationResult.Errors.First().ErrorMessage });
            }

            await _teamMemberService.CancelJoinRequestAsync(teamId, cancelRequest.UserId);

            return Ok(new { Message = "Join request successfully cancelled" });
        }
    }
}