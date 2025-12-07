using Locked_IN_Backend.DTOs.GameProfile;

namespace Locked_IN_Backend.Services
{
    public record GameProfileResult(bool Success, string Message, object? Data = null);

    public interface IGameProfileService
    {
        Task<GameProfileResult> AddGameToFavoritesAsync(int userId, int gameId);
        Task<GameProfileResult> RemoveGameFromFavoritesAsync(int userId, int gameId);
        
        Task<GameProfileResult> GetUserFavoritesAsync(int userId);
    }
}