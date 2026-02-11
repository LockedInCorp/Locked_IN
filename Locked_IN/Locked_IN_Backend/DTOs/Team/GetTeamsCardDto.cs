using Locked_IN_Backend.DTOs.ExperienceTag;
using Locked_IN_Backend.Misc.Enum;

namespace Locked_IN_Backend.DTOs;

public class GetTeamsCardDto
{
    public int Id { get; set; }
    
    public string Name { get; set; } = null!;
    
    public int? MinCompScore { get; set; }
    
    public int MaxPlayerCount { get; set; }

    public string Description { get; set; } = null!;
    
    public GetGameDto Game { get; set; }
    
    public bool IsPrivate { get; set; }
    
    public bool AutoAccept { get; set; }
    
    public int ExperienceTagId { get; set; }
    
    public GetExperienceTagDto? ExperienceLevel { get; set; }
    
    public int CurrentMemberCount { get; set; }
    
    public List<GetPreferanceTagsDto>? PreferenceTags { get; set; }
    
    public DateTime CreationTimestamp { get; set; }
    public string? IconUrl { get; set; }
    public float? SearchRank { get; set; }
    public string? TeamLeaderUsername { get; set; }
    public TeamMemberStatus TeamMemberStatus { get; set; }
    
    public int ChatId { get; set; }
}