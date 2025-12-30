using System.ComponentModel.DataAnnotations;

namespace Locked_IN_Backend.DTOs.Chat;

public class SendMessageDto
{
    [Required]
    public int ChatId { get; set; }
    
    [Required]
    [StringLength(2000)]
    public string Content { get; set; } = null!;
    
    [StringLength(500)]
    public string? AttachmentUrl { get; set; }
}

