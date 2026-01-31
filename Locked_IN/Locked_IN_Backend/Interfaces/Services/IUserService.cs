using Locked_IN_Backend.DTOs.User;

namespace Locked_IN_Backend.Interfaces.Services
{
    public record UserResult(bool Success, string Message, UserProfileDto? Data = null);

    public interface IUserService
    {
        Task<UserProfileDto> GetUserProfileAsync(int userId);
        Task<UserProfileDto> RegisterAsync(RegisterDto dto);
        Task<UserProfileDto> LoginAsync(LoginDto dto);
        Task<UserProfileDto> UpdateUserProfileAsync(int userId, UpdateUserProfileDto dto);
        Task<UserProfileDto> UpdateAvailabilityAsync(int userId, UpdateAvailabilityDto dto);
        Task LogoutAsync();
    }
}