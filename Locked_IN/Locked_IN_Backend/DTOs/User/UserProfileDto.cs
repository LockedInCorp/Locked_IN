namespace Locked_IN_Backend.DTOs.User
{
    public class UserProfileDto
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string AvatarURL { get; set; }
        public Dictionary<string, List<string>>? Availability { get; set; }
    }
}