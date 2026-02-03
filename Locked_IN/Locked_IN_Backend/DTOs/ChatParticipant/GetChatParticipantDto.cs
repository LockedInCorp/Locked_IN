namespace Locked_IN_Backend.DTOs.ChatParticipant;

public class GetChatParticipantDto
{
    public int UserId { get; set; }
    public string Username { get; set; } = null!;
    public string? AvatarUrl { get; set; }
    public int RoleId { get; set; }
    public DateTime JoinedAt { get; set; }
}