namespace Locked_IN_Backend.DTOs.Team;

public class TeamJoinStatusDto
{
    public int TeamId { get; set; }
    public string TeamName { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
}
