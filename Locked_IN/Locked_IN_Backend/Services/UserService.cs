using System.Text.Json;
using AutoMapper;
using Locked_IN_Backend.Data.Entities;
using Locked_IN_Backend.DTOs.User;
using Locked_IN_Backend.Interfaces;
using Locked_IN_Backend.Interfaces.Repositories;
using Locked_IN_Backend.Interfaces.Services;
using Microsoft.AspNetCore.Identity;
using Locked_IN_Backend.Exceptions;

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

        public async Task<UserProfileDto> GetUserProfileAsync(int userId)
        {
            var user = await _userRepository.GetUserById(userId);

            if (user == null)
            {
                throw new NotFoundException("User not found.");
            }

            return _mapper.Map<UserProfileDto>(user);
        }

        public async Task<UserProfileDto> RegisterAsync(RegisterDto dto)
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
                    throw new BadRequestException($"Avatar upload failed: {ex.Message}");
                }
            }

            var user = _mapper.Map<User>(dto);
            user.AvatarUrl = avatarUrl;

            var result = await _userRepository.CreateUserAsync(user, dto.Password);
            
            if (!result.Succeeded)
            {
                if (avatarUrl != null)
                {
                    var bucket = avatarUrl.Split("/")[0];
                    var fileName = avatarUrl.Split("/")[1];
                    await _fileUploadService.DeleteFileAsync(bucket, fileName);
                }
                
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new BadRequestException(errors);
            }

            var userProfile = await GetUserProfileAsync(user.Id);
            userProfile.Token = _jwtService.GenerateToken(user);

            return userProfile;
        }

        public async Task<UserProfileDto> LoginAsync(LoginDto dto)
        {
            var user = await _userRepository.FindByNameAsync(dto.Username) ?? await _userRepository.FindByEmailAsync(dto.Username);

            if (user == null)
            {
                throw new UnauthorizedException("Invalid username or password.");
            }

            var result = await _userRepository.PasswordSignInAsync(user, dto.Password, false, false);

            if (!result.Succeeded)
            {
                throw new UnauthorizedException("Invalid username or password.");
            }

            var userProfile = await GetUserProfileAsync(user.Id);
            userProfile.Token = _jwtService.GenerateToken(user);
            
            return userProfile;
        }

        public async Task<UserProfileDto> UpdateUserProfileAsync(int userId, UpdateUserProfileDto dto)
        {
            var user = await _userRepository.FindByIdAsync(userId.ToString());

            if (user == null)
            {
                throw new NotFoundException("User not found.");
            }

            user.UserName = dto.Username;
            user.Email = dto.Email;
            if (user.AvatarUrl != null && dto.AvatarFile != null)
            {
                var bucket = user.AvatarUrl.Split("/")[0];
                var fileName = user.AvatarUrl.Split("/")[1];
                await _fileUploadService.DeleteFileAsync(bucket, fileName);
                user.AvatarUrl = await _fileUploadService.UploadUserAvatarAsync(dto.AvatarFile);
            }

            var result = await _userRepository.UpdateUserAsync(user);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new BadRequestException(errors);
            }

            return await GetUserProfileAsync(userId);
        }

        public async Task<UserProfileDto> UpdateAvailabilityAsync(int userId, UpdateAvailabilityDto dto)
        {
            var user = await _userRepository.GetUserById(userId);

            if (user == null)
            {
                throw new NotFoundException("User not found.");
            }

            string jsonString = JsonSerializer.Serialize(dto.Availability);
            user.Availability = jsonString;

            await _userRepository.UpdateUser(user);

            return await GetUserProfileAsync(userId);
        }

        public async Task LogoutAsync()
        {
            await _userRepository.SignOutAsync();
        }
    }
}