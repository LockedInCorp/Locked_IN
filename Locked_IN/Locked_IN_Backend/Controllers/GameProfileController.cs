using Locked_IN_Backend.DTOs.GameProfile;
using Locked_IN_Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Locked_IN_Backend.Controllers
{
    [ApiController]
    [Route("api/game-profile")]
    public class GameProfileController : ControllerBase
    {
        private readonly IGameProfileService _gameProfileService;

        public GameProfileController(IGameProfileService gameProfileService)
        {
            _gameProfileService = gameProfileService;
        }

        /// <summary>
        /// Get list of user's favorite games
        /// </summary>
        /// <param name="userId">The ID of the user</param>
        /// <returns>List of favorite games</returns>
        [HttpGet("favorites/{userId}")]
        public async Task<IActionResult> GetFavorites(int userId)
        {
            var result = await _gameProfileService.GetUserFavoritesAsync(userId);
            return result.Success ? Ok(result) : NotFound(result);
        }

        /// <summary>
        /// Add a game to the user's favorites list
        /// </summary>
        /// <param name="userId">The ID of the user</param>
        /// <param name="gameId">The ID of the game</param>
        /// <returns>Updated game profile</returns>
        [HttpPost("unfavorite/{userId}/{gameId}")]
        public async Task<IActionResult> AddToFavorite(int userId, int gameId)
        {
            var result = await _gameProfileService.AddGameToFavoritesAsync(userId, gameId);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// Remove a game from the user's favorites list
        /// </summary>
        /// <param name="userId">The ID of the user</param>
        /// <param name="gameId">The ID of the game to remove</param>
        /// <returns>Updated game profile</returns>
        [HttpDelete("favorite/{userId}/{gameId}")]
        public async Task<IActionResult> RemoveFromFavorite(int userId, int gameId)
        {
            var result = await _gameProfileService.RemoveGameFromFavoritesAsync(userId, gameId);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}