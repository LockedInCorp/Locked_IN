using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Locked_IN_Backend.Data.Entities
{
    [Table("friendship_status")]
    public partial class FriendshipStatus
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("status_name")]
        [StringLength(20)]
        public string StatusName { get; set; } = null!;

        [InverseProperty("Status")]
        public virtual ICollection<Friendship> Friendships { get; set; } = new List<Friendship>();
    }
}