using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Locked_IN_Backend.Data.Entities;

[Table("preference_tag")]
public partial class PreferenceTag
{
    [Key]
    [Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Column("preferencename")]
    [StringLength(20)]
    public string Preferencename { get; set; } = null!;

    [InverseProperty("PreferenceTag")]
    public virtual ICollection<GameprofilePreferencetagRelation> GameprofilePreferencetagRelations { get; set; } = new List<GameprofilePreferencetagRelation>();

    [InverseProperty("PreferenceTag")]
    public virtual ICollection<TeamPreferencetagRelation> TeamPreferencetagRelations { get; set; } = new List<TeamPreferencetagRelation>();
}
