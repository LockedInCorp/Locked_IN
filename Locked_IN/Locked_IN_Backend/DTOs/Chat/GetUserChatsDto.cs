namespace Locked_IN_Backend.DTOs.Chat;

public class GetUserChatsDto
{
    public int Id { get; set; }
    public string? ChatName { get; set; }
    public string? LastMessageUsername { get; set; }
    public string? LastMessageContent { get; set; }
    public DateTime? LastMessageTime { get; set; }
    public int UnreadMessageCount { get; set; }
    public string? ChatIconUrl { get; set; }
    public DateTime CreationTimestamp { get; set; }
}