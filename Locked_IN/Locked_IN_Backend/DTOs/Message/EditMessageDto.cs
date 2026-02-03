using System.ComponentModel.DataAnnotations;
using Locked_IN_Backend.Misc;

namespace Locked_IN_Backend.DTOs.Chat;

public class EditMessageDto
{
    [Required]
    public int MessageId { get; set; }
    
    [Required]
    [StringLength(ValidationConstraints.MaxMessageLenght)]
    public string Content { get; set; } = null!;
}

