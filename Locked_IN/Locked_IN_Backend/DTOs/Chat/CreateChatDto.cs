using System.ComponentModel.DataAnnotations;
using Locked_IN_Backend.Data.Enums;

namespace Locked_IN_Backend.DTOs.Chat;

public class CreateChatDto
{
    [Required]
    public ChatType Type { get; set; }
    
    [StringLength(100)]
    public string? Name { get; set; }
    
    public int? TeamId { get; set; }
    
    [Required]
    public List<int> ParticipantUserIds { get; set; } = new();
}

