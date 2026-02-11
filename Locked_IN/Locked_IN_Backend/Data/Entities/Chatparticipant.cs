using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Locked_IN_Backend.Data.Entities;

[Table("chatparticipant")]
public partial class Chatparticipant
{
    [Key]
    [Column("chatparticipant_id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ChatparticipantId { get; set; }

    [Column("user_id")]
    public int UserId { get; set; }

    [Column("chat_id")]
    public int ChatId { get; set; }

    [Column("joined_at", TypeName = "timestamp without time zone")]
    public DateTime JoinedAt { get; set; }

    [Column("last_read_at", TypeName = "timestamp without time zone")]
    public DateTime? LastReadAt { get; set; }

    [Column("unread_count")]
    public int UnreadCount { get; set; } = 0;
    [Column("has_Left")]
    public bool HasLeft { get; set; }

    [Column("role_id")]
    public int RoleId { get; set; }

    [ForeignKey("ChatId")]
    [InverseProperty("Chatparticipants")]
    public virtual Chat Chat { get; set; } = null!;

    [InverseProperty("ChatparticipantChatparticipant")]
    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();

    [ForeignKey("RoleId")]
    [InverseProperty("Chatparticipants")]
    public virtual Role Role { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("Chatparticipants")]
    public virtual User User { get; set; } = null!;
}
