using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Locked_IN_Backend.Data.Entities;

[Table("gameprofile_preferencetag_relation")]
public partial class GameprofilePreferencetagRelation
{
    [Key]
    [Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Column("game_profile_id")]
    public int GameProfileId { get; set; }

    [Column("preference_tag_id")]
    public int PreferenceTagId { get; set; }

    [ForeignKey("GameProfileId")]
    [InverseProperty("GameprofilePreferencetagRelations")]
    public virtual GameProfile GameProfile { get; set; } = null!;

    [ForeignKey("PreferenceTagId")]
    [InverseProperty("GameprofilePreferencetagRelations")]
    public virtual PreferenceTag PreferenceTag { get; set; } = null!;
}
