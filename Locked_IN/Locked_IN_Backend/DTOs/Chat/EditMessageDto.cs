using System.ComponentModel.DataAnnotations;

namespace Locked_IN_Backend.DTOs.Chat;

public class EditMessageDto
{
    [Required]
    public int MessageId { get; set; }
    
    [Required]
    [StringLength(2000)]
    public string Content { get; set; } = null!;
}

