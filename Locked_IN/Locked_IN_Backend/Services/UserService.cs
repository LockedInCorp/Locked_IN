using System.Text.Json;
using Locked_IN_Backend.Data.Entities;
using Locked_IN_Backend.DTOs.User;
using Locked_IN_Backend.Interfaces;
using Locked_IN_Backend.Interfaces.Repositories;
using Microsoft.AspNetCore.Identity;

namespace Locked_IN_Backend.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IFileUploadService _fileUploadService;

        public UserService(IUserRepository userRepository, IFileUploadService fileUploadService)
        {
            _userRepository = userRepository;
            _fileUploadService = fileUploadService;
        }

        public async Task<UserResult> GetUserProfileAsync(int userId)
        {
            var user = await _userRepository.GetUserById(userId);

            if (user == null)
            {
                return new UserResult(false, "User not found.");
            }

            Dictionary<string, List<string>>? availabilityDict = null;
            try 
            {
                if (!string.IsNullOrEmpty(user.Availability))
                {
                    availabilityDict = JsonSerializer.Deserialize<Dictionary<string, List<string>>>(user.Availability);
                }
            }
            catch
            {
                availabilityDict = new Dictionary<string, List<string>>();
            }

            IFormFile? avatarFile = null;
            if (!string.IsNullOrEmpty(user.AvatarUrl))
            {
                try
                {
                    avatarFile = await _fileUploadService.GetUserAvatarAsync(user.AvatarUrl);
                }
                catch
                {
                    // If image retrieval fails, we just return null for Avatar
                }
            }

            var response = new UserProfileDto
            {
                Id = user.Id,
                Email = user.Email,
                Username = user.UserName,
                Avatar = avatarFile,
                Availability = availabilityDict
            };

            return new UserResult(true, "User profile retrieved.", response);
        }

        public async Task<UserResult> RegisterAsync(RegisterDto dto)
        {
            string? avatarUrl = null;
            if (dto.Avatar != null)
            {
                try
                {
                    avatarUrl = await _fileUploadService.UploadUserAvatarAsync(dto.Avatar);
                }
                catch (Exception ex)
                {
                    return new UserResult(false, $"Avatar upload failed: {ex.Message}");
                }
            }

            var user = new User
            {
                UserName = dto.Username,
                Email = dto.Email,
                AvatarUrl = avatarUrl,
                Availability = "{}"
            };

            var result = await _userRepository.CreateUserAsync(user, dto.Password);
            
            if (!result.Succeeded)
            {
                if (avatarUrl != null)
                {
                    await _fileUploadService.DeleteUserAvatarAsync(avatarUrl);
                }
                
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return new UserResult(false, errors);
            }

            return await GetUserProfileAsync(user.Id);
        }

        public async Task<UserResult> UpdateUserProfileAsync(int userId, UpdateUserProfileDto dto)
        {
            var user = await _userRepository.FindByIdAsync(userId.ToString());

            if (user == null)
            {
                return new UserResult(false, "User not found.");
            }

            user.UserName = dto.Username;
            user.Email = dto.Email;
            user.AvatarUrl = dto.AvatarUrl;

            var result = await _userRepository.UpdateUserAsync(user);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return new UserResult(false, errors);
            }

            return await GetUserProfileAsync(userId);
        }

        public async Task<UserResult> UpdateAvailabilityAsync(int userId, UpdateAvailabilityDto dto)
        {
            var user = await _userRepository.GetUserById(userId);

            if (user == null)
            {
                return new UserResult(false, "User not found.");
            }

            string jsonString = JsonSerializer.Serialize(dto.Availability);
            user.Availability = jsonString;

            await _userRepository.UpdateUser(user);

            return await GetUserProfileAsync(userId);
        }
    }
}