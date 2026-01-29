using Locked_IN_Backend.DTOs.User;

namespace Locked_IN_Backend.Interfaces.Services
{
    public record UserResult(bool Success, string Message, UserProfileDto? Data = null);

    public interface IUserService
    {
        Task<UserResult> GetUserProfileAsync(int userId);
        Task<UserResult> RegisterAsync(RegisterDto dto);
        Task<UserResult> LoginAsync(LoginDto dto);
        Task<UserResult> UpdateUserProfileAsync(int userId, UpdateUserProfileDto dto);
        Task<UserResult> UpdateAvailabilityAsync(int userId, UpdateAvailabilityDto dto);
        Task<UserResult> LogoutAsync();
    }
}