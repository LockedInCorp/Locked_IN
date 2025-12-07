namespace Locked_IN_Backend.DTOs.User
{
    public class UpdateUserProfileDto
    {
        public string Nickname { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? AvatarUrl { get; set; }
    }
}