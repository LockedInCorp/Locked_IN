using System.Text.Json;
using Locked_IN_Backend.Data;
using Locked_IN_Backend.DTOs.User;
using Microsoft.EntityFrameworkCore;

namespace Locked_IN_Backend.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<UserResult> GetUserProfileAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);

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

            var response = new UserProfileResponseDto
            {
                Id = user.Id,
                Email = user.Email,
                Nickname = user.Nickname,
                AvatarUrl = user.AvatarUrl,
                Availability = availabilityDict
            };

            return new UserResult(true, "User profile retrieved.", response);
        }

        public async Task<UserResult> UpdateUserProfileAsync(int userId, UpdateUserProfileDto dto)
        {
            var user = await _context.Users.FindAsync(userId);

            if (user == null)
            {
                return new UserResult(false, "User not found.");
            }

            if (user.Email != dto.Email)
            {
                var emailExists = await _context.Users.AnyAsync(u => u.Email == dto.Email && u.Id != userId);
                if (emailExists)
                {
                    return new UserResult(false, "Email is already taken by another user.");
                }
            }

            user.Nickname = dto.Nickname;
            user.Email = dto.Email;
            user.AvatarUrl = dto.AvatarUrl;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return await GetUserProfileAsync(userId);
        }

        public async Task<UserResult> UpdateAvailabilityAsync(int userId, UpdateAvailabilityDto dto)
        {
            var user = await _context.Users.FindAsync(userId);

            if (user == null)
            {
                return new UserResult(false, "User not found.");
            }

            string jsonString = JsonSerializer.Serialize(dto.Availability);
            user.Availability = jsonString;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return await GetUserProfileAsync(userId);
        }
    }
}