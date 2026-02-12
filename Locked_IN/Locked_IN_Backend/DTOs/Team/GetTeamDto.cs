using Locked_IN_Backend.DTOs;
using Locked_IN_Backend.DTOs.ExperienceTag;

namespace Locked_IN_Backend.DTO;

public class GetTeamDto
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
    
    public List<GetUserForTeamViewDto>? Members { get; set; }
    public GetUserForTeamViewDto Leader { get; set; }
    
    public List<GetPreferanceTagsDto>? PreferenceTags { get; set; }
    
    public DateTime CreationTimestamp { get; set; }
    public string? IconUrl { get; set; }
    public CommunicationServiceDto? CommunicationService { get; set; }
    public string? CommunicationServiceLink { get; set; }
    public float? SearchRank { get; set; }
    public int ChatId { get; set; }
}