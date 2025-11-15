using Locked_IN_Backend.DTOs.Friendship;
using Locked_IN_Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Locked_IN_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FriendshipController : ControllerBase
    {
        private readonly IFriendshipService _friendshipService;

        public FriendshipController(IFriendshipService friendshipService)
        {
            _friendshipService = friendshipService;
        }

        /// <summary>
        /// Send a new friend request
        /// </summary>
        /// <param name="requesterId">The ID of the user sending the request</param>
        /// <param name="dto">DTO containing the Receiver ID</param>
        /// <returns>Result message</returns>
        [HttpPost("send/{requesterId}")]
        public async Task<IActionResult> SendRequest(int requesterId, [FromBody] SendFriendRequestDto dto)
        {
            var result = await _friendshipService.SendFriendRequestAsync(requesterId, dto.ReceiverId);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// Accept a pending friend request
        /// </summary>
        /// <param name="friendshipId">The ID of the friendship record (request)</param>
        /// <param name="currentUserId">The ID of the user accepting the request (must be the receiver)</param>
        /// <returns>Result message</returns>
        [HttpPost("{friendshipId}/accept/{currentUserId}")]
        public async Task<IActionResult> AcceptRequest(int friendshipId, int currentUserId)
        {
            var result = await _friendshipService.AcceptFriendRequestAsync(friendshipId, currentUserId);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// Decline a pending friend request
        /// </summary>
        /// <param name="friendshipId">The ID of the friendship record (request)</param>
        /// <param name="currentUserId">The ID of the user declining the request (must be the receiver)</param>
        /// <returns>Result message</returns>
        [HttpPost("{friendshipId}/decline/{currentUserId}")]
        public async Task<IActionResult> DeclineRequest(int friendshipId, int currentUserId)
        {
            var result = await _friendshipService.DeclineFriendRequestAsync(friendshipId, currentUserId);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// Cancel an outgoing (pending) friend request
        /// </summary>
        /// <param name="friendshipId">The ID of the friendship record (request)</param>
        /// <param name="currentUserId">The ID of the user cancelling the request (must be the sender)</param>
        /// <returns>Result message</returns>
        [HttpDelete("{friendshipId}/cancel/{currentUserId}")]
        public async Task<IActionResult> CancelRequest(int friendshipId, int currentUserId)
        {
            var result = await _friendshipService.CancelFriendRequestAsync(friendshipId, currentUserId);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// Get the list of accepted friends for a user
        /// </summary>
        /// <param name="userId">The ID of the user</param>
        /// <returns>A list of friends</returns>
        [HttpGet("list/{userId}")]
        public async Task<IActionResult> GetFriendsList(int userId)
        {
            var result = await _friendshipService.GetFriendsListAsync(userId);
            return Ok(result);
        }

        /// <summary>
        /// Get pending incoming friend requests for a user
        /// </summary>
        /// <param name="userId">The ID of the user</param>
        /// <returns>A list of pending requests</returns>
        [HttpGet("pending-requests/{userId}")]
        public async Task<IActionResult> GetPendingRequests(int userId)
        {
            var result = await _friendshipService.GetPendingRequestsAsync(userId);
            return Ok(result);
        }
        
        /// <summary>
        /// Checks the friendship status between two users
        /// </summary>
        /// <param name="userId1">The ID of the first user</param>
        /// <param name="userId2">The ID of the second user</param>
        /// <returns>The friendship status (e.g., Accepted, Pending, None)</returns>
        [HttpGet("status/{userId1}/{userId2}")]
        public async Task<IActionResult> GetStatus(int userId1, int userId2)
        {
            var result = await _friendshipService.GetFriendshipStatusAsync(userId1, userId2);
            return Ok(result);
        }
        
        /// <summary>
        /// Block another user
        /// </summary>
        /// <param name="blockerId">The ID of the user initiating the block</param>
        /// <param name="userToBlockId">The ID of the user to be blocked</param>
        /// <returns>Result message</returns>
        [HttpPost("block/{blockerId}/{userToBlockId}")]
        public async Task<IActionResult> BlockUser(int blockerId, int userToBlockId)
        {
            var result = await _friendshipService.BlockUserAsync(blockerId, userToBlockId);
            return result.Success ? Ok(result) : BadRequest(result);
        }
        
        /// <summary>
        /// Unblock another user
        /// </summary>
        /// <param name="blockerId">The ID of the user initiating the unblock</param>
        /// <param name="userToUnblockId">The ID of the user to be unblocked</param>
        /// <returns>Result message</returns>
        [HttpDelete("unblock/{blockerId}/{userToUnblockId}")]
        public async Task<IActionResult> UnblockUser(int blockerId, int userToUnblockId)
        {
            var result = await _friendshipService.UnblockUserAsync(blockerId, userToUnblockId);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}