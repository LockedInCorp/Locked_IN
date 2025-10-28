using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Locked_IN_Backend.Data.Entities;

[Table("team")]
public partial class Team
{
    [Key]
    [Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Column("name")]
    [StringLength(20)]
    public string Name { get; set; } = null!;

    [Column("min_comp_score")]
    public int? MinCompScore { get; set; }

    [Column("max_player_count")]
    public int MaxPlayerCount { get; set; }

    [Column("description")]
    public int Description { get; set; }

    [Column("game_id")]
    public int GameId { get; set; }

    [Column("isprivate")]
    public bool Isprivate { get; set; }

    [Column("isblitz")]
    public bool Isblitz { get; set; }

    [Column("experience_tag_id")]
    public int ExperienceTagId { get; set; }

    [InverseProperty("Team")]
    public virtual ICollection<Chat> Chats { get; set; } = new List<Chat>();

    [ForeignKey("ExperienceTagId")]
    [InverseProperty("Teams")]
    public virtual ExperienceTag ExperienceTag { get; set; } = null!;

    [ForeignKey("GameId")]
    [InverseProperty("Teams")]
    public virtual Game Game { get; set; } = null!;

    [InverseProperty("Team")]
    public virtual ICollection<TeamMember> TeamMembers { get; set; } = new List<TeamMember>();

    [InverseProperty("Team")]
    public virtual ICollection<TeamPreferencetagRelation> TeamPreferencetagRelations { get; set; } = new List<TeamPreferencetagRelation>();
}
