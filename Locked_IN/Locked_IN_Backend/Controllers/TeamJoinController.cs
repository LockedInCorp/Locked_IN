using Locked_IN_Backend.DTOs;
using Locked_IN_Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Locked_IN_Backend.Controllers
{
    [ApiController]
    [Route("api")]
    public class TeamJoinController : ControllerBase
    {
        private readonly ITeamJoinService _teamJoinService;

        public TeamJoinController(ITeamJoinService teamJoinService)
        {
            _teamJoinService = teamJoinService;
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
            var result = await _teamJoinService.RequestToJoinTeamAsync(teamId, joinRequest.UserId);
            
            return result.Status switch
            {
                TeamJoinResultStatus.Success => Ok(new { Message = result.Message }),
                TeamJoinResultStatus.NotFound => NotFound(new { Message = result.Message }),
                TeamJoinResultStatus.BadRequest => BadRequest(new { Message = result.Message }),
                TeamJoinResultStatus.Conflict => Conflict(new { Message = result.Message }),
                _ => StatusCode(500, "An unexpected error occurred")
            };
        }

        /// <summary>
        /// Get all pending join requests for a team
        /// </summary>
        /// <param name="teamId">The ID of the team</param>
        /// <returns>A list of pending join requests</returns>
        [HttpGet("teams/{teamId}/join-requests")]
        public async Task<IActionResult> GetJoinRequests(int teamId)
        {
            var requests = await _teamJoinService.GetJoinRequestsAsync(teamId);
            return Ok(requests);
        }

        /// <summary>
        /// Accept a pending join request
        /// </summary>
        /// <param name="teamMemberId">The ID of the TeamMember record (join request)</param>
        /// <returns>Confirmation or error message</returns>
        [HttpPost("join-requests/{teamMemberId}/accept")]
        public async Task<IActionResult> AcceptJoinRequest(int teamMemberId)
        {
            var result = await _teamJoinService.AcceptJoinRequestAsync(teamMemberId);

            return result.Status switch
            {
                TeamJoinResultStatus.Success => Ok(new { Message = result.Message }),
                TeamJoinResultStatus.NotFound => NotFound(new { Message = result.Message }),
                TeamJoinResultStatus.BadRequest => BadRequest(new { Message = result.Message }),
                _ => StatusCode(500, "An unexpected error occurred")
            };
        }

        /// <summary>
        /// Decline a pending join request
        /// </summary>
        /// <param name="teamMemberId">The ID of the TeamMember record (join request)</param>
        /// <returns>Confirmation or error message</returns>
        [HttpPost("join-requests/{teamMemberId}/decline")]
        public async Task<IActionResult> DeclineJoinRequest(int teamMemberId)
        {
            var result = await _teamJoinService.DeclineJoinRequestAsync(teamMemberId);

            return result.Status switch
            {
                TeamJoinResultStatus.Success => Ok(new { Message = result.Message }),
                TeamJoinResultStatus.NotFound => NotFound(new { Message = result.Message }),
                _ => StatusCode(500, "An unexpected error occurred")
            };
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
            var result = await _teamJoinService.CancelJoinRequestAsync(teamId, cancelRequest.UserId);

            return result.Status switch
            {
                TeamJoinResultStatus.Success => Ok(new { Message = result.Message }),
                TeamJoinResultStatus.NotFound => NotFound(new { Message = result.Message }),
                TeamJoinResultStatus.BadRequest => BadRequest(new { Message = result.Message }),
                _ => StatusCode(500, "An unexpected error occurred")
            };
        }
    }
}