using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Locked_IN_Backend.Data.Entities;

[Table("team_preferencetag_relation")]
public partial class TeamPreferencetagRelation
{
    [Key]
    [Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Column("team_id")]
    public int TeamId { get; set; }

    [Column("preference_tag_id")]
    public int PreferenceTagId { get; set; }

    [ForeignKey("PreferenceTagId")]
    [InverseProperty("TeamPreferencetagRelations")]
    public virtual PreferenceTag PreferenceTag { get; set; } = null!;

    [ForeignKey("TeamId")]
    [InverseProperty("TeamPreferencetagRelations")]
    public virtual Team Team { get; set; } = null!;
}
