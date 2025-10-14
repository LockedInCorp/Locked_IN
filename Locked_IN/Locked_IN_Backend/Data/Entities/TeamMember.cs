using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Locked_IN_Backend.Data.Entities;

[Table("team_member")]
public partial class TeamMember
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("isleader")]
    public bool Isleader { get; set; }

    [Column("jointimestamp", TypeName = "timestamp without time zone")]
    public DateTime Jointimestamp { get; set; }

    [Column("team_id")]
    public int TeamId { get; set; }

    [Column("user_id")]
    public int UserId { get; set; }

    [Column("member_status_id")]
    public int MemberStatusId { get; set; }

    [ForeignKey("MemberStatusId")]
    [InverseProperty("TeamMembers")]
    public virtual MemberStatus MemberStatus { get; set; } = null!;

    [ForeignKey("TeamId")]
    [InverseProperty("TeamMembers")]
    public virtual Team Team { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("TeamMembers")]
    public virtual User User { get; set; } = null!;
}
