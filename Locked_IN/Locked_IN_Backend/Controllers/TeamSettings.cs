namespace Locked_IN_Backend.Controllers;

public class TeamSettings
{
    public const string SectionName = "TeamSettings";
    
    public int MaxActiveJoinRequestsPerUser { get; set; } = 5;
    public int DefaultMaxTeamSize { get; set; } = 20;
}
