namespace Locked_IN_Backend.DTOs.Friendship
{
    public class PendingFriendshipRequestDto
    {
        public int FriendshipId { get; set; }
        public int RequesterId { get; set; }
        public string RequesterUsername { get; set; } = string.Empty;
        
        public string? RequesterAvatarUrl { get; set; }
        public DateTime RequestTimestamp { get; set; }
    }
}