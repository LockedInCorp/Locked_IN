using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Locked_IN_Backend.Data.Entities;

[Table("role")]
public partial class Role
{
    [Key]
    [Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Column("rolename")]
    [StringLength(20)]
    public string Rolename { get; set; } = null!;

    [InverseProperty("Role")]
    public virtual ICollection<Chatparticipant> Chatparticipants { get; set; } = new List<Chatparticipant>();
}
