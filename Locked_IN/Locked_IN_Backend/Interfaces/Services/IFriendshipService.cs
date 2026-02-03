using Locked_IN_Backend.DTOs.Friendship;

namespace Locked_IN_Backend.Services
{
    public interface IFriendshipService
    {
        Task SendFriendRequestAsync(SendFriendRequestDto dto);
        Task AcceptFriendRequestAsync(FriendshipActionDto dto);
        Task DeclineFriendRequestAsync(FriendshipActionDto dto);
        Task CancelFriendRequestAsync(FriendshipActionDto dto);
        Task<List<FriendshipDto>> GetFriendsListAsync(int userId);
        Task<List<PendingFriendshipRequestDto>> GetPendingRequestsAsync(int userId);
        Task<string> GetFriendshipStatusAsync(int userId1, int userId2);
        Task BlockUserAsync(BlockUserDto dto);
        Task UnblockUserAsync(UnblockUserDto dto);
    }
}