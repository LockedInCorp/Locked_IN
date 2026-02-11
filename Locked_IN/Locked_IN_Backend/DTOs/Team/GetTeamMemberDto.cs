namespace Locked_IN_Backend.DTO;

public class GetTeamMemberDto
{
    
    public bool IsLeader { get; set; }
    
    public DateTime JoinTimestamp { get; set; }
    
    public int TeamId { get; set; }
    
    public int UserId { get; set; }
    
    public int MemberStatusId { get; set; }
    
    public GetUserForTeamViewDto? User { get; set; }
}