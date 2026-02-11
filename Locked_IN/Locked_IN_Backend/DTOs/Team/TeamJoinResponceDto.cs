using Locked_IN_Backend.Misc.Enum;

namespace Locked_IN_Backend.DTOs
{
    public class TeamJoinResponceDto
    {
        public int TeamId { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string AvatarUrl { get; set; } = string.Empty;
        public TeamMemberStatus Status { get; set; }
        public DateTime RequestTimestamp { get; set; }
    }
}