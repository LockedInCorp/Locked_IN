using System.Text.Json;
using AutoMapper;
using Locked_IN_Backend.Data.Entities;
using Locked_IN_Backend.DTOs.User;
using Locked_IN_Backend.Interfaces;
using Locked_IN_Backend.Interfaces.Repositories;
using Locked_IN_Backend.Interfaces.Services;
using Microsoft.AspNetCore.Identity;

namespace Locked_IN_Backend.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IFileUploadService _fileUploadService;
        private readonly IJwtService _jwtService;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IFileUploadService fileUploadService, IJwtService jwtService, IMapper mapper)
        {
            _userRepository = userRepository;
            _fileUploadService = fileUploadService;
            _jwtService = jwtService;
            _mapper = mapper;
        }

        public async Task<UserResult> GetUserProfileAsync(int userId)
        {
            var user = await _userRepository.GetUserById(userId);

            if (user == null)
            {
                return new UserResult(false, "User not found.");
            }

            var response = _mapper.Map<UserProfileDto>(user);

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

            var user = _mapper.Map<User>(dto);
            user.AvatarUrl = avatarUrl;

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

            var userResult = await GetUserProfileAsync(user.Id);
            if (userResult.Success && userResult.Data != null)
            {
                userResult.Data.Token = _jwtService.GenerateToken(user);
            }

            return userResult;
        }

        public async Task<UserResult> LoginAsync(LoginDto dto)
        {
            var user = await _userRepository.FindByNameAsync(dto.Username) ?? await _userRepository.FindByEmailAsync(dto.Username);

            if (user == null)
            {
                return new UserResult(false, "Invalid username or password.");
            }

            var result = await _userRepository.PasswordSignInAsync(user, dto.Password, false, false);

            if (!result.Succeeded)
            {
                return new UserResult(false, "Invalid username or password.");
            }

            var userResult = await GetUserProfileAsync(user.Id);
            if (userResult.Success && userResult.Data != null)
            {
                userResult.Data.Token = _jwtService.GenerateToken(user);
            }
            
            return userResult;
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

        public async Task<UserResult> LogoutAsync()
        {
            await _userRepository.SignOutAsync();
            return new UserResult(true, "Logged out successfully.");
        }
    }
}