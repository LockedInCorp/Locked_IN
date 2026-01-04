using System.ComponentModel.DataAnnotations;

namespace Locked_IN_Backend.DTOs.Chat;

public class MarkAsReadDto
{
    [Required]
    public int ChatId { get; set; }
}

