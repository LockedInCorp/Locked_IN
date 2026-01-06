namespace Locked_IN_Backend.DTOs
{
    public class TeamJoinResponceDto
    {
        public int TeamMemberId { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public DateTime RequestTimestamp { get; set; }
    }
}