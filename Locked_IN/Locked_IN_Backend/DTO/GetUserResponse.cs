namespace Locked_IN_Backend.DTO;

public class GetUserResponse
{
    public int Id { get; set; }
    
    public string Email { get; set; } = null!;
    
    public string Nickname { get; set; } = null!;
    
    public string Availability { get; set; } = null!;
}