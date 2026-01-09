namespace Locked_IN_Backend.DTO;

public class GetUserForTeamViewDto
{
    public int Id { get; set; }
    
    public string Email { get; set; } = null!;
    
    public string Username { get; set; } = null!;
    
    public string Availability { get; set; } = null!;
}