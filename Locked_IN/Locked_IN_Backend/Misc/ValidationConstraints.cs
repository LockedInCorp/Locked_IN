namespace Locked_IN_Backend.Misc;

public static class ValidationConstraints
{
    public static readonly HashSet<int> AllowedPageSizes = new() { 6, 12, 24 };
    public const int DefaultPageSize = 12;
    public const int MaxActiveJoinRequestsPerUser = 5;
    public const int DefaultMaxTeamSize = 20;
}
