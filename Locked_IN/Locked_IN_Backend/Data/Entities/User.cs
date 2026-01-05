using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Locked_IN_Backend.Data.Entities;

[Table("User")]
public partial class User : IdentityUser<int>
{
    [Column("nickname")]
    [StringLength(50)]
    public string Nickname { get; set; } = null!;

    [Column("avatar_url")]
    [StringLength(255)]
    public string? AvatarUrl { get; set; }

    [Column("availability", TypeName = "json")]
    public string Availability { get; set; } = null!;

    [InverseProperty("User")]
    public virtual ICollection<Chatparticipant> Chatparticipants { get; set; } = new List<Chatparticipant>();

    [InverseProperty("User2")]
    public virtual ICollection<Friendship> FriendshipUser2s { get; set; } = new List<Friendship>();

    [InverseProperty("User")]
    public virtual ICollection<Friendship> FriendshipUsers { get; set; } = new List<Friendship>();

    [InverseProperty("User")]
    public virtual ICollection<GameProfile> GameProfiles { get; set; } = new List<GameProfile>();

    [InverseProperty("User")]
    public virtual ICollection<TeamMember> TeamMembers { get; set; } = new List<TeamMember>();
}