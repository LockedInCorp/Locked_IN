namespace Locked_IN_Backend.DTO;

public class GetTeamResponse
{
    public int Id { get; set; }
    
    public string Name { get; set; } = null!;
    
    public int? MinCompScore { get; set; }
    
    public int MaxPlayerCount { get; set; }
    
    public int Description { get; set; }
    
    public int GameId { get; set; }
    
    public string? GameName { get; set; }
    
    public bool IsPrivate { get; set; }
    
    public bool IsBlitz { get; set; }
    
    public int ExperienceTagId { get; set; }
    
    public string? ExperienceLevel { get; set; }
    
    public int CurrentMemberCount { get; set; }
    
    public List<GetTeamMemberResponse>? Members { get; set; }
    
    public List<string>? PreferenceTags { get; set; }
}