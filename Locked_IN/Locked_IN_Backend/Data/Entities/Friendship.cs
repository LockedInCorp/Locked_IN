using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Locked_IN_Backend.Data.Entities;

[Table("friendship")]
public partial class Friendship
{
    [Key]
    [Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Column("request_timestamp", TypeName = "timestamp with time zone")]
    public DateTime RequestTimestamp { get; set; }

    [Column("user_id")]
    public int UserId { get; set; }

    [Column("user_2_id")]
    public int User2Id { get; set; }

    [Column("status_id")]
    public int StatusId { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("FriendshipUsers")]
    public virtual User User { get; set; } = null!;

    [ForeignKey("User2Id")]
    [InverseProperty("FriendshipUser2s")]
    public virtual User User2 { get; set; } = null!;

    [ForeignKey("StatusId")]
    [InverseProperty("Friendships")]
    public virtual FriendshipStatus Status { get; set; } = null!;
}