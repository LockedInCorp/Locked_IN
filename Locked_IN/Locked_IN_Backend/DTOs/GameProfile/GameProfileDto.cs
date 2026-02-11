namespace Locked_IN_Backend.DTOs.GameProfile
{
    public class GameProfileDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int GameId { get; set; }
        public string GameName { get; set; } = string.Empty;
        public bool IsFavorite { get; set; }
        public string? Rank { get; set; }
        public string? Role { get; set; }
        public string? InGameNickname { get; set; }
        public int ExperienceTagId { get; set; }
        public int GameExpId { get; set; }
        public List<string> Preferences { get; set; } = new List<string>();
    }
}