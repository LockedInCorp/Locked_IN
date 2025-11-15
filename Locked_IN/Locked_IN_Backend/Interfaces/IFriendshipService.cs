using Locked_IN_Backend.DTOs.Friendship;

namespace Locked_IN_Backend.Services
{
    public record FriendshipResult(bool Success, string Message, object? Data = null);

    public interface IFriendshipService
    {
        Task<FriendshipResult> SendFriendRequestAsync(int requesterId, int receiverId);
        Task<FriendshipResult> AcceptFriendRequestAsync(int friendshipId, int currentUserId);
        Task<FriendshipResult> DeclineFriendRequestAsync(int friendshipId, int currentUserId);
        Task<FriendshipResult> CancelFriendRequestAsync(int friendshipId, int currentUserId);
        Task<FriendshipResult> GetFriendsListAsync(int userId);
        Task<FriendshipResult> GetPendingRequestsAsync(int userId);
        Task<FriendshipResult> GetFriendshipStatusAsync(int userId1, int userId2);
        Task<FriendshipResult> BlockUserAsync(int blockerId, int userToBlockId);
        Task<FriendshipResult> UnblockUserAsync(int blockerId, int userToUnblockId);
    }
}