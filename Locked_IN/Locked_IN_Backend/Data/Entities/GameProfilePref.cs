using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Locked_IN_Backend.Data.Entities;

[Table("game_profile_pref")]
public partial class GameProfilePref
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("gameplay_pref_id")]
    public int GameplayPrefId { get; set; }

    [Column("game_profile_id")]
    public int GameProfileId { get; set; }

    [ForeignKey("GameProfileId")]
    [InverseProperty("GameProfilePrefs")]
    public virtual GameProfile GameProfile { get; set; } = null!;

    [ForeignKey("GameplayPrefId")]
    [InverseProperty("GameProfilePrefs")]
    public virtual GameplayPref GameplayPref { get; set; } = null!;
}
