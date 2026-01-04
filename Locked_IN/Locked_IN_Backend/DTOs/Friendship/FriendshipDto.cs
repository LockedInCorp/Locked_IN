namespace Locked_IN_Backend.DTOs.Friendship
{
    public class FriendshipDto
    {
        public int FriendshipId { get; set; }
        public int FriendId { get; set; }
        public string FriendNickname { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime Since { get; set; }
    }
}