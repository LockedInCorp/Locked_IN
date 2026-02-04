using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using FluentValidation;
using Locked_IN_Backend.DTOs.GameProfile;
using Locked_IN_Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Locked_IN_Backend.Controllers;

[ApiController]
[Route("api/game-profile")]
public class GameProfileController : ControllerBase
{
    private readonly IGameProfileService _gameProfileService;
    private readonly IValidator<CreateGameProfileDto> _createProfileValidator;

    public GameProfileController(
        IGameProfileService gameProfileService,
        IValidator<CreateGameProfileDto> createProfileValidator)
    {
        _gameProfileService = gameProfileService;
        _createProfileValidator = createProfileValidator;
    }

    /// <summary>
    /// Get list of user's favorite games
    /// </summary>
    /// <param name="userId">The ID of the user</param>
    /// <returns>List of favorite games</returns>
    [HttpGet("favorites/{userId}")]
    public async Task<ActionResult<List<GameProfileDto>>> GetFavorites(int userId)
    {
        var result = await _gameProfileService.GetUserFavoritesAsync(userId);
        return Ok(result);
    }

    /// <summary>
    /// Add a game to the user's favorites list
    /// </summary>
    /// <param name="gameId">The ID of the game</param>
    /// <returns>Updated game profile</returns>
    [Authorize]
    [HttpPut("favorite/{gameId}")]
    public async Task<ActionResult<GameProfileDto>> AddToFavorite(int gameId)
    {
        var userIdClaim = User.FindFirstValue(JwtRegisteredClaimNames.Sub);
        if (string.IsNullOrEmpty(userIdClaim)) return Unauthorized();
        var userId = int.Parse(userIdClaim);

        var result = await _gameProfileService.AddGameToFavoritesAsync(userId, gameId);
        return Ok(result);
    }

    /// <summary>
    /// Remove a game from the user's favorites list
    /// </summary>
    /// <param name="gameId">The ID of the game to remove</param>
    /// <returns>Updated game profile</returns>
    [Authorize]
    [HttpPut("unfavorite/{gameId}")]
    public async Task<ActionResult<GameProfileDto>> RemoveFromFavorite(int gameId)
    {
        var userIdClaim = User.FindFirstValue(JwtRegisteredClaimNames.Sub);
        if (string.IsNullOrEmpty(userIdClaim)) return Unauthorized();
        var userId = int.Parse(userIdClaim);

        var result = await _gameProfileService.RemoveGameFromFavoritesAsync(userId, gameId);
        return Ok(result);
    }

    /// <summary>
    /// Create a full game profile for the user
    /// </summary>
    /// <param name="dto">Profile creation data</param>
    /// <returns>Created game profile</returns>
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<GameProfileDto>> CreateGameProfile([FromBody] CreateGameProfileDto dto)
    {
        var userIdClaim = User.FindFirstValue(JwtRegisteredClaimNames.Sub);
        if (string.IsNullOrEmpty(userIdClaim)) return Unauthorized();
        var userId = int.Parse(userIdClaim);

        var validationResult = await _createProfileValidator.ValidateAsync(dto);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors.First().ErrorMessage);
        }

        var result = await _gameProfileService.CreateGameProfileAsync(userId, dto);
        return Ok(result);
    }

    /// <summary>
    /// Completely delete a game profile
    /// </summary>
    /// <param name="gameId">The ID of the game profile to delete</param>
    [Authorize]
    [HttpDelete("{gameId}")]
    public async Task<IActionResult> DeleteGameProfile(int gameId)
    {
        var userIdClaim = User.FindFirstValue(JwtRegisteredClaimNames.Sub);
        if (string.IsNullOrEmpty(userIdClaim)) return Unauthorized();
        var userId = int.Parse(userIdClaim);

        await _gameProfileService.DeleteGameProfileAsync(userId, gameId);
        return Ok(new { Message = "Game profile deleted successfully." });
    }
    
    /// <summary>
    /// Get all game profiles for a user
    /// </summary>
    /// <param name="userId">The ID of the user</param>
    /// <returns>List of all game profiles</returns>
    [HttpGet("user/{userId}")]
    public async Task<ActionResult<List<GameProfileDto>>> GetUserGameProfiles(int userId)
    {
        var result = await _gameProfileService.GetUserGameProfilesAsync(userId);
        return Ok(result);
    }
}