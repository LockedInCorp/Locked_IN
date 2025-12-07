namespace Locked_IN_Backend.DTOs.User
{
    public class UserProfileResponseDto
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Nickname { get; set; } = string.Empty;
        public string? AvatarUrl { get; set; }
        public Dictionary<string, List<string>>? Availability { get; set; }
    }
}