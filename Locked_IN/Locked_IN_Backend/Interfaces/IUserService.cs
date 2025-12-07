using Locked_IN_Backend.DTOs.User;

namespace Locked_IN_Backend.Services
{
    public record UserResult(bool Success, string Message, UserProfileResponseDto? Data = null);

    public interface IUserService
    {
        Task<UserResult> GetUserProfileAsync(int userId);
        Task<UserResult> UpdateUserProfileAsync(int userId, UpdateUserProfileDto dto);
        Task<UserResult> UpdateAvailabilityAsync(int userId, UpdateAvailabilityDto dto);
    }
}