namespace Locked_IN_Backend.DTOs;

public class GetTeamsCardDto
{
    public int Id { get; set; }
    
    public string Name { get; set; } = null!;
    
    public int? MinCompScore { get; set; }
    
    public int MaxPlayerCount { get; set; }

    public string Description { get; set; } = null!;
    
    public int GameId { get; set; }
    
    public string? GameName { get; set; }
    
    public bool IsPrivate { get; set; }
    
    public bool AutoAccept { get; set; }
    
    public int ExperienceTagId { get; set; }
    
    public string? ExperienceLevel { get; set; }
    
    public int CurrentMemberCount { get; set; }
    
    public List<string>? PreferenceTags { get; set; }
    
    public DateTime CreationTimestamp { get; set; }
    public string? IconUrl { get; set; }
    public float? SearchRank { get; set; }
    public string? TeamLeaderUsername { get; set; }
}