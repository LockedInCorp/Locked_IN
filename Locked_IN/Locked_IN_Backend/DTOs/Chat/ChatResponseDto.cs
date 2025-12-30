namespace Locked_IN_Backend.DTOs.Chat;

public class ChatResponseDto
{
    public int Id { get; set; }
    public string Type { get; set; } = null!;
    public string? Name { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? LastMessageAt { get; set; }
    public int? TeamId { get; set; }
    public int UnreadCount { get; set; }
    public List<ChatParticipantDto> Participants { get; set; } = new();
    public MessageResponseDto? LastMessage { get; set; }
}

public class ChatParticipantDto
{
    public int UserId { get; set; }
    public string Nickname { get; set; } = null!;
    public string? AvatarUrl { get; set; }
    public int RoleId { get; set; }
    public DateTime JoinedAt { get; set; }
}

