using Locked_IN_Backend.DTOs.Friendship;

namespace Locked_IN_Backend.Services
{
    public record FriendshipResult(bool Success, string Message, object? Data = null);

    public interface IFriendshipService
    {
        Task<FriendshipResult> SendFriendRequestAsync(SendFriendRequestDto dto);
        Task<FriendshipResult> AcceptFriendRequestAsync(FriendshipActionDto dto);
        Task<FriendshipResult> DeclineFriendRequestAsync(FriendshipActionDto dto);
        Task<FriendshipResult> CancelFriendRequestAsync(FriendshipActionDto dto);
        Task<FriendshipResult> GetFriendsListAsync(int userId);
        Task<FriendshipResult> GetPendingRequestsAsync(int userId);
        Task<FriendshipResult> GetFriendshipStatusAsync(int userId1, int userId2);
        Task<FriendshipResult> BlockUserAsync(BlockUserDto dto);
        Task<FriendshipResult> UnblockUserAsync(UnblockUserDto dto);
    }
}