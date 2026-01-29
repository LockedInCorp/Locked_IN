namespace Locked_IN_Backend.DTOs.Chat;

public class GetMessageDto
{
    public int Id { get; set; }
    public string Content { get; set; } = null!;
    public DateTime SentAt { get; set; }
    public DateTime? EditedAt { get; set; }
    public bool IsDeleted { get; set; }
    public string? AttachmentUrl { get; set; }
    public int SenderId { get; set; }
    public string SenderUsername { get; set; } = null!;
    public string? SenderAvatarUrl { get; set; }
    public int ChatId { get; set; }
}

