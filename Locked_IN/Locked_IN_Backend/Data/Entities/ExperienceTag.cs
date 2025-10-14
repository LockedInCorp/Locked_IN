using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Locked_IN_Backend.Data.Entities;

[Table("experience_tag")]
public partial class ExperienceTag
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("experiencelevel")]
    [StringLength(20)]
    public string Experiencelevel { get; set; } = null!;

    [InverseProperty("ExperienceTag")]
    public virtual ICollection<GameProfile> GameProfiles { get; set; } = new List<GameProfile>();

    [InverseProperty("ExperienceTag")]
    public virtual ICollection<Team> Teams { get; set; } = new List<Team>();
}
