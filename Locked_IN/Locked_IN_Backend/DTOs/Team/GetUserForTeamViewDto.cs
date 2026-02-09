namespace Locked_IN_Backend.DTO;

public class GetUserForTeamViewDto
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string AvatarUrl { get; set; } = null!;

}