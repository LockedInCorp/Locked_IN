using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Locked_IN_Backend.Data.Entities;

[Table("gameplay_pref")]
public partial class GameplayPref
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("preference")]
    [StringLength(20)]
    public string Preference { get; set; } = null!;

    [InverseProperty("GameplayPref")]
    public virtual ICollection<GameProfilePref> GameProfilePrefs { get; set; } = new List<GameProfilePref>();
}
