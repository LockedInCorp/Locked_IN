using Locked_IN_Backend.Data.Entities;

namespace Locked_IN_Backend.Repositories;

public interface IFriendshipRepository
{
    Task<Friendship?> GetFriendshipByIdAsync(int friendshipId);
    Task<Friendship?> GetFriendshipBetweenUsersAsync(int userId1, int userId2);
    Task<List<Friendship>> GetAcceptedFriendshipsAsync(int userId);
    Task<List<Friendship>> GetPendingIncomingRequestsAsync(int userId);
    Task AddFriendshipAsync(Friendship friendship);
    Task UpdateFriendshipAsync(Friendship friendship);
    Task DeleteFriendshipAsync(Friendship friendship);
}
