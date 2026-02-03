using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Locked_IN_Backend.Data.Entities;

[Table("chat")]
public partial class Chat
{
    [Key]
    [Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Column("type")]
    [StringLength(20)]
    public string Type { get; set; } = null!;

    [Column("name")]
    [StringLength(100)]
    public string? Name { get; set; }

    [Column("created_at", TypeName = "timestamp without time zone")]
    public DateTime CreatedAt { get; set; }

    [Column("last_message_at", TypeName = "timestamp without time zone")]
    public DateTime? LastMessageAt { get; set; }

    [Column("team_id")]
    public int? TeamId { get; set; }
    
    [InverseProperty("Chat")]
    public virtual ICollection<Chatparticipant> Chatparticipants { get; set; } = new List<Chatparticipant>();

    [ForeignKey("TeamId")]
    [InverseProperty("Chats")]
    public virtual Team? Team { get; set; }
}
