using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Locked_IN_Backend.Data.Entities;

[Table("friendship")]
public partial class Friendship
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("friendssince_timestamp", TypeName = "timestamp without time zone")]
    public DateTime FriendssinceTimestamp { get; set; }

    [Column("user_id")]
    public int UserId { get; set; }

    [Column("user_2_id")]
    public int User2Id { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("FriendshipUsers")]
    public virtual User User { get; set; } = null!;

    [ForeignKey("User2Id")]
    [InverseProperty("FriendshipUser2s")]
    public virtual User User2 { get; set; } = null!;
}
