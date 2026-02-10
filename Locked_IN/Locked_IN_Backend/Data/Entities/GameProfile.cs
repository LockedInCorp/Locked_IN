using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Locked_IN_Backend.Data.Entities;

[Table("game_profile")]
public partial class GameProfile
{
    [Key]
    [Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Column("user_id")]
    public int UserId { get; set; }

    [Column("game_id")]
    public int GameId { get; set; }

    [Column("rank")]
    public int? Rank { get; set; }
    
    [Column("role")]
    [StringLength(50)]
    public string? Role { get; set; }

    [Column("in_game_nickname")]
    [StringLength(50)]
    public string? InGameNickname { get; set; }

    [Column("isfavorite")]
    public bool Isfavorite { get; set; }

    [Column("experience_tag_id")]
    public int ExperienceTagId { get; set; }

    [Column("game_exp_id")]
    public int GameExpId { get; set; }

    [ForeignKey("ExperienceTagId")]
    [InverseProperty("GameProfiles")]
    public virtual ExperienceTag ExperienceTag { get; set; } = null!;

    [ForeignKey("GameId")]
    [InverseProperty("GameProfiles")]
    public virtual Game Game { get; set; } = null!;

    [InverseProperty("GameProfile")]
    public virtual ICollection<GameprofilePreferencetagRelation> GameprofilePreferencetagRelations { get; set; } = new List<GameprofilePreferencetagRelation>();

    [ForeignKey("UserId")]
    [InverseProperty("GameProfiles")]
    public virtual User User { get; set; } = null!;
}