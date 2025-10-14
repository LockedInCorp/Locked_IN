using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Locked_IN_Backend.Data.Entities;

[Table("game")]
public partial class Game
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    [StringLength(20)]
    public string Name { get; set; } = null!;

    [InverseProperty("Game")]
    public virtual ICollection<GameProfile> GameProfiles { get; set; } = new List<GameProfile>();

    [InverseProperty("Game")]
    public virtual ICollection<Team> Teams { get; set; } = new List<Team>();
}
