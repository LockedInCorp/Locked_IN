namespace Locked_IN_Backend.DTOs.Team;

public class TeamJoinStatusDto
{
    public int TeamId { get; set; }
    public string TeamName { get; set; } = string.Empty;
    public int Status { get; set; } = 5;
}
