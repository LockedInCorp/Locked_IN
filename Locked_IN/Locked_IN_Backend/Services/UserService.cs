using System.Text.Json;
using AutoMapper;
using Locked_IN_Backend.Data.Entities;
using Locked_IN_Backend.DTOs.User;
using Locked_IN_Backend.Interfaces.Services;
using Locked_IN_Backend.Interfaces.Repositories;
using Locked_IN_Backend.Exceptions;
using Locked_IN_Backend.Interfaces;

namespace Locked_IN_Backend.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IFileUploadService _fileUploadService;
    private readonly IJwtService _jwtService;
    private readonly IMapper _mapper;

    public UserService(
        IUserRepository userRepository, 
        IFileUploadService fileUploadService, 
        IJwtService jwtService, 
        IMapper mapper)
    {
<<<<<<< Updated upstream
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
            throw new NotFoundException($"User with ID {userId} not found.");
=======
        private readonly IUserRepository _userRepository;
        private readonly IFileUploadService _fileUploadService;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IFileUploadService fileUploadService, IMapper mapper)
        {
            _userRepository = userRepository;
            _fileUploadService = fileUploadService;
            _mapper = mapper;
>>>>>>> Stashed changes
        }

        return _mapper.Map<UserProfileDto>(user);
    }

    public async Task<UserProfileDto> RegisterAsync(RegisterDto dto)
    {
        var existingUser = await _userRepository.FindByNameAsync(dto.Username) ?? await _userRepository.FindByEmailAsync(dto.Email);
        if (existingUser != null)
        {
            throw new ConflictException("Username or Email is already taken.");
        }

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
                try 
                {
                    var parts = avatarUrl.Split('/');
                    if (parts.Length >= 2)
                    {
                        await _fileUploadService.DeleteFileAsync(parts[0], parts[1]);
                    }
                }
                catch 
                { 
                }
            }
            
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new BadRequestException(errors);
        }

        var userProfile = _mapper.Map<UserProfileDto>(user);
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

        var userProfile = _mapper.Map<UserProfileDto>(user);
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

        if (dto.Username != user.UserName)
        {
            var existingUser = await _userRepository.FindByNameAsync(dto.Username);
            if (existingUser != null) throw new ConflictException("Username is already taken.");
        }
        
        if (dto.Email != user.Email)
        {
            var existingEmail = await _userRepository.FindByEmailAsync(dto.Email);
            if (existingEmail != null) throw new ConflictException("Email is already registered.");
        }

        user.UserName = dto.Username;
        user.Email = dto.Email;

        if (dto.AvatarFile != null)
        {
            if (!string.IsNullOrEmpty(user.AvatarUrl))
            {
                try
                {
                    var parts = user.AvatarUrl.Split('/');
                    if (parts.Length >= 2)
                    {
                        await _fileUploadService.DeleteFileAsync(parts[0], parts[1]);
                    }
                }
                catch
                {
                }
            }
            
<<<<<<< Updated upstream
            user.AvatarUrl = await _fileUploadService.UploadUserAvatarAsync(dto.AvatarFile);
=======
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

            return userProfile;
>>>>>>> Stashed changes
        }

        var result = await _userRepository.UpdateUserAsync(user);

        if (!result.Succeeded)
        {
<<<<<<< Updated upstream
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new BadRequestException(errors);
=======
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
            
            return userProfile;
>>>>>>> Stashed changes
        }

        return _mapper.Map<UserProfileDto>(user);
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

        return _mapper.Map<UserProfileDto>(user);
    }

    public async Task LogoutAsync()
    {
        await _userRepository.SignOutAsync();
    }
}