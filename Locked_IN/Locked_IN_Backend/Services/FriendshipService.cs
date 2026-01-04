using Locked_IN_Backend.Data;
using Locked_IN_Backend.Data.Entities;
using Locked_IN_Backend.DTOs.Friendship;
using Locked_IN_Backend.Misc.Enum;
using Microsoft.EntityFrameworkCore;

namespace Locked_IN_Backend.Services
{
    public class FriendshipService : IFriendshipService
    {
        private readonly AppDbContext _context;

        public FriendshipService(AppDbContext context)
        {
            _context = context;
        }

        private async Task<Friendship?> GetExistingFriendship(int user1Id, int user2Id)
        {
            return await _context.Friendships
                .FirstOrDefaultAsync(f => (f.UserId == user1Id && f.User2Id == user2Id) ||
                                          (f.UserId == user2Id && f.User2Id == user1Id));
        }

        public async Task<FriendshipResult> SendFriendRequestAsync(int requesterId, int receiverId)
        {
            if (requesterId == receiverId)
            {
                return new FriendshipResult(false, "Cannot send friend request to yourself.");
            }

            if (await _context.Users.FindAsync(receiverId) == null)
            {
                return new FriendshipResult(false, "Receiver user not found.");
            }

            var existingFriendship = await GetExistingFriendship(requesterId, receiverId);

            if (existingFriendship != null)
            {
                if (existingFriendship.StatusId == (int)FriendshipStatusEnum.Accepted)
                {
                    return new FriendshipResult(false, "You are already friends with this user.");
                }
                if (existingFriendship.StatusId == (int)FriendshipStatusEnum.Pending)
                {
                    return new FriendshipResult(false, "A friend request is already pending.");
                }
                if (existingFriendship.StatusId == (int)FriendshipStatusEnum.Blocked)
                {
                    string blockedMessage = existingFriendship.UserId == requesterId 
                        ? "You have blocked this user." 
                        : "This user has blocked you.";
                    return new FriendshipResult(false, $"Cannot send request. {blockedMessage}");
                }
            }

            var newFriendship = new Friendship
            {
                UserId = requesterId,
                User2Id = receiverId,
                StatusId = (int)FriendshipStatusEnum.Pending,
                RequestTimestamp = DateTime.UtcNow
            };

            _context.Friendships.Add(newFriendship);
            await _context.SaveChangesAsync();

            return new FriendshipResult(true, "Friend request sent successfully.");
        }

        public async Task<FriendshipResult> AcceptFriendRequestAsync(int friendshipId, int currentUserId)
        {
            var friendship = await _context.Friendships.FindAsync(friendshipId);

            if (friendship == null)
            {
                return new FriendshipResult(false, "Friend request not found.");
            }

            if (friendship.User2Id != currentUserId)
            {
                return new FriendshipResult(false, "Unauthorized to accept this request.");
            }

            if (friendship.StatusId != (int)FriendshipStatusEnum.Pending)
            {
                return new FriendshipResult(false, "This request is no longer pending.");
            }

            friendship.StatusId = (int)FriendshipStatusEnum.Accepted;
            friendship.RequestTimestamp = DateTime.UtcNow; 
            
            _context.Friendships.Update(friendship);
            await _context.SaveChangesAsync();

            return new FriendshipResult(true, "Friend request accepted.");
        }

        public async Task<FriendshipResult> DeclineFriendRequestAsync(int friendshipId, int currentUserId)
        {
            var friendship = await _context.Friendships.FindAsync(friendshipId);

            if (friendship == null)
            {
                return new FriendshipResult(false, "Friend request not found.");
            }

            if (friendship.User2Id != currentUserId)
            {
                return new FriendshipResult(false, "Unauthorized to decline this request.");
            }

            if (friendship.StatusId != (int)FriendshipStatusEnum.Pending)
            {
                return new FriendshipResult(false, "This request is no longer pending.");
            }

            _context.Friendships.Remove(friendship);
            await _context.SaveChangesAsync();

            return new FriendshipResult(true, "Friend request declined and removed.");
        }

        public async Task<FriendshipResult> CancelFriendRequestAsync(int friendshipId, int currentUserId)
        {
            var friendship = await _context.Friendships.FindAsync(friendshipId);

            if (friendship == null)
            {
                return new FriendshipResult(false, "Friend request not found.");
            }

            if (friendship.UserId != currentUserId)
            {
                return new FriendshipResult(false, "Unauthorized to cancel this request.");
            }

            if (friendship.StatusId != (int)FriendshipStatusEnum.Pending)
            {
                return new FriendshipResult(false, "This request is no longer pending.");
            }
            
            _context.Friendships.Remove(friendship);
            await _context.SaveChangesAsync();

            return new FriendshipResult(true, "Friend request cancelled.");
        }

        public async Task<FriendshipResult> GetFriendsListAsync(int userId)
        {
            var friends = await _context.Friendships
                .Where(f => (f.UserId == userId || f.User2Id == userId) && f.StatusId == (int)FriendshipStatusEnum.Accepted)
                .Include(f => f.User)
                .Include(f => f.User2)
                .Select(f => new FriendshipDto
                {
                    FriendshipId = f.Id,
                    FriendId = f.UserId == userId ? f.User2Id : f.UserId,
                    FriendNickname = f.UserId == userId ? f.User2.Nickname : f.User.Nickname,
                    Status = f.Status.StatusName,
                    Since = f.RequestTimestamp
                })
                .ToListAsync();

            return new FriendshipResult(true, "Friends list retrieved.", friends);
        }

        public async Task<FriendshipResult> GetPendingRequestsAsync(int userId)
        {
            var requests = await _context.Friendships
                .Where(f => f.User2Id == userId && f.StatusId == (int)FriendshipStatusEnum.Pending)
                .Include(f => f.User)
                .Select(f => new PendingFriendshipRequestDto
                {
                    FriendshipId = f.Id,
                    RequesterId = f.UserId,
                    RequesterNickname = f.User.Nickname,
                    RequestTimestamp = f.RequestTimestamp
                })
                .ToListAsync();

            return new FriendshipResult(true, "Pending requests retrieved.", requests);
        }

        public async Task<FriendshipResult> GetFriendshipStatusAsync(int userId1, int userId2)
        {
            var friendship = await _context.Friendships
                .Include(f => f.Status)
                .FirstOrDefaultAsync(f => (f.UserId == userId1 && f.User2Id == userId2) ||
                                          (f.UserId == userId2 && f.User2Id == userId1));
            
            var status = friendship?.Status.StatusName ?? "None";
            
            return new FriendshipResult(true, "Status retrieved.", new { status = status });
        }

        public async Task<FriendshipResult> BlockUserAsync(int blockerId, int userToBlockId)
        {
            if (blockerId == userToBlockId)
            {
                return new FriendshipResult(false, "Cannot block yourself.");
            }

            if (await _context.Users.FindAsync(userToBlockId) == null)
            {
                return new FriendshipResult(false, "User to block not found.");
            }

            var existingFriendship = await GetExistingFriendship(blockerId, userToBlockId);

            if (existingFriendship != null)
            {
                if (existingFriendship.StatusId == (int)FriendshipStatusEnum.Blocked)
                {
                    return new FriendshipResult(false, "This user relationship is already blocked.");
                }

                existingFriendship.StatusId = (int)FriendshipStatusEnum.Blocked;
                existingFriendship.UserId = blockerId;
                existingFriendship.User2Id = userToBlockId;
                existingFriendship.RequestTimestamp = DateTime.UtcNow;
                _context.Friendships.Update(existingFriendship);
            }
            else
            {
                var newBlock = new Friendship
                {
                    UserId = blockerId,
                    User2Id = userToBlockId,
                    StatusId = (int)FriendshipStatusEnum.Blocked,
                    RequestTimestamp = DateTime.UtcNow
                };
                _context.Friendships.Add(newBlock);
            }

            await _context.SaveChangesAsync();
            return new FriendshipResult(true, "User successfully blocked.");
        }

        public async Task<FriendshipResult> UnblockUserAsync(int blockerId, int userToUnblockId)
        {
            var friendship = await _context.Friendships
                .FirstOrDefaultAsync(f => f.UserId == blockerId && 
                                          f.User2Id == userToUnblockId && 
                                          f.StatusId == (int)FriendshipStatusEnum.Blocked);

            if (friendship == null)
            {
                return new FriendshipResult(false, "No active block found from you to this user.");
            }

            _context.Friendships.Remove(friendship);
            await _context.SaveChangesAsync();

            return new FriendshipResult(true, "User successfully unblocked.");
        }
    }
}