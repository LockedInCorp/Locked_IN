using System.ComponentModel.DataAnnotations;
using Locked_IN_Backend.Misc;

namespace Locked_IN_Backend.DTOs.Chat;

public class SendMessageDto
{
    [Required]
    public int ChatId { get; set; }
    
    [Required]
    [StringLength(ValidationConstraints.MaxMessageLenght)]
    public string Content { get; set; } = null!;
    
    public IFormFile? AttachmentFile { get; set; }
}

