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
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Column("content")]
    [StringLength(2000)]
    public string Content { get; set; } = null!;

    [Column("sent_at", TypeName = "timestamp without time zone")]
    public DateTime SentAt { get; set; }

    [Column("edited_at", TypeName = "timestamp without time zone")]
    public DateTime? EditedAt { get; set; }

    [Column("is_deleted")]
    public bool IsDeleted { get; set; } = false;

    [Column("deleted_at", TypeName = "timestamp without time zone")]
    public DateTime? DeletedAt { get; set; }

    [Column("attachment_url")]
    [StringLength(500)]
    public string? AttachmentUrl { get; set; }

    [Column("chatparticipant_chatparticipant_id")]
    public int ChatparticipantChatparticipantId { get; set; }

    [ForeignKey("ChatparticipantChatparticipantId")]
    [InverseProperty("Messages")]
    public virtual Chatparticipant ChatparticipantChatparticipant { get; set; } = null!;
}
