using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Locked_IN_Backend.Data.Entities;

[Table( "communication_service")]
public class CommunicationService
{
    [Key]
    [Column( "id" )]
    [DatabaseGenerated( DatabaseGeneratedOption.Identity )]
    public int Id { get; set; }
    
    [Column( "name" )]
    [StringLength(256)]
    public string Name { get; set; }

    [InverseProperty("CommunicationService")]
    public virtual ICollection<TeamCommunicationService> TeamCommunicationServices { get; set; } = new List<TeamCommunicationService>();
}