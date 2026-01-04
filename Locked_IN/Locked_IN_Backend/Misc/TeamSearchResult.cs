using Locked_IN_Backend.Data.Entities;

namespace Locked_IN_Backend.Misc;

public class TeamSearchResult
{
    public Team Team { get; set; } = null!;
    public float SearchRank { get; set; }
    public string? TeamLeaderNickname { get; set; }
}
