using Locked_IN_Backend.DTOs.GameProfile;

namespace Locked_IN_Backend.Services;

public interface IGameProfileService
{
    Task<List<GameProfileDto>> GetUserFavoritesAsync(int userId);
    Task<GameProfileDto> AddGameToFavoritesAsync(int userId, int gameId);
    Task<GameProfileDto> RemoveGameFromFavoritesAsync(int userId, int gameId);
    Task<GameProfileDto> CreateGameProfileAsync(int userId, CreateGameProfileDto dto);
    Task<GameProfileDto> UpdateGameProfileAsync(int userId, int profileId, UpdateGameProfileDto dto);
    Task DeleteGameProfileAsync(int userId, int profileId);
    Task<List<GameProfileDto>> GetUserGameProfilesAsync(int userId);
}