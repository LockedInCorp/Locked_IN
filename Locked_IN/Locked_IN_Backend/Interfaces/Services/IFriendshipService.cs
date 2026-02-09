using Locked_IN_Backend.DTOs.Friendship;

namespace Locked_IN_Backend.Services
{
    public interface IFriendshipService
    {
        Task SendFriendRequestAsync(int requesterId, int receiverId);
        Task AcceptFriendRequestAsync(int userId, int friendshipId);
        Task DeclineFriendRequestAsync(int userId, int friendshipId);
        Task CancelFriendRequestAsync(int userId, int friendshipId);
        Task RemoveFriendAsync(int userId, int friendId);
        Task<List<FriendshipDto>> GetFriendsListAsync(int userId);
        Task<List<PendingFriendshipRequestDto>> GetPendingRequestsAsync(int userId);
        Task<string> GetFriendshipStatusAsync(int userId1, int userId2);
        Task BlockUserAsync(int blockerId, int userToBlockId);
        Task UnblockUserAsync(int blockerId, int userToUnblockId);
    }
}