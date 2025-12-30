using System.ComponentModel.DataAnnotations;

namespace Locked_IN_Backend.DTOs.Chat;

public class CreateChatDto
{
    [Required]
    [StringLength(20)]
    public string Type { get; set; } = null!; // "Direct", "Team", "Group"
    
    [StringLength(100)]
    public string? Name { get; set; }
    
    public int? TeamId { get; set; }
    
    [Required]
    public List<int> ParticipantUserIds { get; set; } = new();
}

