using Locked_IN_Backend.Data.Entities;
using Locked_IN_Backend.Misc.Enum;

namespace Locked_IN_Backend.Misc;

public class TeamSearchResult
{
    public Team Team { get; set; } = null!;
    public float SearchRank { get; set; }
    public string? TeamLeaderUsername { get; set; }
    public TeamMemberStatus TeamMemberStatus { get; set; }
}
