using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Locked_IN_Backend.Data.Entities;

[Table("message")]
public partial class Message
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("content")]
    [StringLength(150)]
    public string Content { get; set; } = null!;

    [Column("sent_at", TypeName = "timestamp without time zone")]
    public DateTime SentAt { get; set; }

    [Column("chatparticipant_chatparticipant_id")]
    public int ChatparticipantChatparticipantId { get; set; }

    [ForeignKey("ChatparticipantChatparticipantId")]
    [InverseProperty("Messages")]
    public virtual Chatparticipant ChatparticipantChatparticipant { get; set; } = null!;
}
