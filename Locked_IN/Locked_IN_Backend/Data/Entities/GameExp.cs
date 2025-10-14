using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Locked_IN_Backend.Data.Entities;

[Table("game_exp")]
public partial class GameExp
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("experience")]
    [StringLength(20)]
    public string Experience { get; set; } = null!;

    [InverseProperty("GameExp")]
    public virtual ICollection<GameProfile> GameProfiles { get; set; } = new List<GameProfile>();
}
