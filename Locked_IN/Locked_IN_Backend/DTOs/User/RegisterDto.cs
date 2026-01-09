using Microsoft.AspNetCore.Http;

namespace Locked_IN_Backend.DTOs.User
{
    public class RegisterDto
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public IFormFile? Avatar { get; set; }
    }
}
