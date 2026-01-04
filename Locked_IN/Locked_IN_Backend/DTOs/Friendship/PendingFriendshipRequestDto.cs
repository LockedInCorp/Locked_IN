namespace Locked_IN_Backend.DTOs.Friendship
{
    public class PendingFriendshipRequestDto
    {
        public int FriendshipId { get; set; }
        public int RequesterId { get; set; }
        public string RequesterNickname { get; set; } = string.Empty;
        public DateTime RequestTimestamp { get; set; }
    }
}