using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Locked_IN_Backend.Data.Entities;

[Table("team_communication_service")]
public class TeamCommunicationService
{
    [Column("team_id")]
    public int TeamId { get; set; }
    
    [Column("communication_service_id")]
    public int CommunicationServiceId { get; set; }
    
    [Column("link")]
    [StringLength(256)]
    public string Link { get; set; }

    [ForeignKey("TeamId")]
    [InverseProperty("TeamCommunicationService")]
    public virtual Team Team { get; set; } = null!;

    [ForeignKey("CommunicationServiceId")]
    [InverseProperty("TeamCommunicationServices")]
    public virtual CommunicationService CommunicationService { get; set; } = null!;
}