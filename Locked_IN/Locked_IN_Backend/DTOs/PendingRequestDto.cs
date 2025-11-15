namespace Locked_IN_Backend.DTOs.Friendship
{
    public class PendingRequestDto
    {
        public int FriendshipId { get; set; }
        public int RequesterId { get; set; }
        public string RequesterNickname { get; set; } = string.Empty;
        public DateTime RequestTimestamp { get; set; }
    }
}