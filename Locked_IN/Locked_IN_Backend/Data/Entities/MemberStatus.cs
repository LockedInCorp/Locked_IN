using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Locked_IN_Backend.Data.Entities;

[Table("member_status")]
public partial class MemberStatus
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("statusname")]
    [StringLength(20)]
    public string Statusname { get; set; } = null!;

    [InverseProperty("MemberStatus")]
    public virtual ICollection<TeamMember> TeamMembers { get; set; } = new List<TeamMember>();
}
