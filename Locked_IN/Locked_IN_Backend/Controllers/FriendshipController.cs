using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using FluentValidation;
using Locked_IN_Backend.DTOs.Friendship;
using Locked_IN_Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Locked_IN_Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FriendshipController : ControllerBase
{
    private readonly IFriendshipService _friendshipService;
    private readonly IValidator<SendFriendRequestDto> _sendRequestValidator;

    public FriendshipController(
        IFriendshipService friendshipService,
        IValidator<SendFriendRequestDto> sendRequestValidator)
    {
        _friendshipService = friendshipService;
        _sendRequestValidator = sendRequestValidator;
    }

    /// <summary>
    /// Send a new friend request
    /// </summary>
    [Authorize]
    [HttpPost("request")]
    public async Task<IActionResult> SendRequest([FromBody] SendFriendRequestDto dto)
    {
        var requesterId = int.Parse(User.FindFirstValue(JwtRegisteredClaimNames.Sub)!);

        var validationResult = await _sendRequestValidator.ValidateAsync(dto);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors.First().ErrorMessage);
        }

        await _friendshipService.SendFriendRequestAsync(requesterId, dto.ReceiverId);
        return Ok(new { Message = "Friend request sent successfully." });
    }

    /// <summary>
    /// Accept a pending friend request
    /// </summary>
    [Authorize]
    [HttpPut("requests/{friendshipId}/accept")]
    public async Task<IActionResult> AcceptRequest(int friendshipId)
    {
        var currentUserId = int.Parse(User.FindFirstValue(JwtRegisteredClaimNames.Sub)!);
        await _friendshipService.AcceptFriendRequestAsync(currentUserId, friendshipId);
        return Ok(new { Message = "Friend request accepted." });
    }

    /// <summary>
    /// Decline a pending friend request
    /// </summary>
    [Authorize]
    [HttpPut("requests/{friendshipId}/decline")]
    public async Task<IActionResult> DeclineRequest(int friendshipId)
    {
        var currentUserId = int.Parse(User.FindFirstValue(JwtRegisteredClaimNames.Sub)!);
        await _friendshipService.DeclineFriendRequestAsync(currentUserId, friendshipId);
        return Ok(new { Message = "Friend request declined." });
    }

    /// <summary>
    /// Cancel an outgoing (pending) friend request
    /// </summary>
    [Authorize]
    [HttpDelete("requests/{friendshipId}/cancel")]
    public async Task<IActionResult> CancelRequest(int friendshipId)
    {
        var currentUserId = int.Parse(User.FindFirstValue(JwtRegisteredClaimNames.Sub)!);
        await _friendshipService.CancelFriendRequestAsync(currentUserId, friendshipId);
        return Ok(new { Message = "Friend request cancelled." });
    }

    /// <summary>
    /// Remove a user from friends
    /// </summary>
    [Authorize]
    [HttpDelete("remove/{friendId}")]
    public async Task<IActionResult> RemoveFriend(int friendId)
    {
        var currentUserId = int.Parse(User.FindFirstValue(JwtRegisteredClaimNames.Sub)!);
        await _friendshipService.RemoveFriendAsync(currentUserId, friendId);
        return Ok(new { Message = "User removed from friends." });
    }

    /// <summary>
    /// Get the list of accepted friends
    /// </summary>
    [Authorize]
    [HttpGet("list")]
    public async Task<ActionResult<List<FriendshipDto>>> GetFriendsList()
    {
        var currentUserId = int.Parse(User.FindFirstValue(JwtRegisteredClaimNames.Sub)!);
        var result = await _friendshipService.GetFriendsListAsync(currentUserId);
        return Ok(result);
    }

    /// <summary>
    /// Get pending incoming friend requests
    /// </summary>
    [Authorize]
    [HttpGet("pending-requests")]
    public async Task<ActionResult<List<PendingFriendshipRequestDto>>> GetPendingRequests()
    {
        var currentUserId = int.Parse(User.FindFirstValue(JwtRegisteredClaimNames.Sub)!);
        var result = await _friendshipService.GetPendingRequestsAsync(currentUserId);
        return Ok(result);
    }
    
    /// <summary>
    /// Checks the friendship status between current user and another
    /// </summary>
    [Authorize]
    [HttpGet("status/{targetUserId}")]
    public async Task<IActionResult> GetStatus(int targetUserId)
    {
        var currentUserId = int.Parse(User.FindFirstValue(JwtRegisteredClaimNames.Sub)!);
        var status = await _friendshipService.GetFriendshipStatusAsync(currentUserId, targetUserId);
        return Ok(new { status = status });
    }
    
    /// <summary>
    /// Block another user
    /// </summary>
    [Authorize]
    [HttpPost("block/{userToBlockId}")]
    public async Task<IActionResult> BlockUser(int userToBlockId)
    {
        var currentUserId = int.Parse(User.FindFirstValue(JwtRegisteredClaimNames.Sub)!);
        await _friendshipService.BlockUserAsync(currentUserId, userToBlockId);
        return Ok(new { Message = "User successfully blocked." });
    }
    
    /// <summary>
    /// Unblock another user
    /// </summary>
    [Authorize]
    [HttpDelete("unblock/{userToUnblockId}")]
    public async Task<IActionResult> UnblockUser(int userToUnblockId)
    {
        var currentUserId = int.Parse(User.FindFirstValue(JwtRegisteredClaimNames.Sub)!);
        await _friendshipService.UnblockUserAsync(currentUserId, userToUnblockId);
        return Ok(new { Message = "User successfully unblocked." });
    }
}