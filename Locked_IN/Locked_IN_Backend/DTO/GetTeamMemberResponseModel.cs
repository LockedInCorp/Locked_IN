namespace Locked_IN_Backend.DTO;

public class GetTeamMemberResponseModel
{
    public int Id { get; set; }
    
    public bool IsLeader { get; set; }
    
    public DateTime JoinTimestamp { get; set; }
    
    public int TeamId { get; set; }
    
    public int UserId { get; set; }
    
    public int MemberStatusId { get; set; }
    
    public string? MemberStatusName { get; set; }
    
    public GetUserResponseModel? User { get; set; }
}