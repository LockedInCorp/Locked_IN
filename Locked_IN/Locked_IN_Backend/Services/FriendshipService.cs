using Locked_IN_Backend.Data;
using Locked_IN_Backend.Data.Entities;
using Locked_IN_Backend.DTOs.Friendship;
using Locked_IN_Backend.Exceptions;
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

        public async Task SendFriendRequestAsync(SendFriendRequestDto dto)
        {
            var existingFriendship = await GetExistingFriendship(dto.RequesterId, dto.ReceiverId);

            if (existingFriendship != null)
            {
                if (existingFriendship.StatusId == (int)FriendshipStatusEnum.Accepted)
                {
                    throw new ConflictException("You are already friends with this user.");
                }
                if (existingFriendship.StatusId == (int)FriendshipStatusEnum.Pending)
                {
                    throw new ConflictException("A friend request is already pending.");
                }
                if (existingFriendship.StatusId == (int)FriendshipStatusEnum.Blocked)
                {
                    string blockedMessage = existingFriendship.UserId == dto.RequesterId 
                        ? "You have blocked this user." 
                        : "This user has blocked you.";
                    throw new ForbiddenException($"Cannot send request. {blockedMessage}");
                }
            }

            var newFriendship = new Friendship
            {
                UserId = dto.RequesterId,
                User2Id = dto.ReceiverId,
                StatusId = (int)FriendshipStatusEnum.Pending,
                RequestTimestamp = DateTime.UtcNow
            };

            _context.Friendships.Add(newFriendship);
            await _context.SaveChangesAsync();
        }

        public async Task AcceptFriendRequestAsync(FriendshipActionDto dto)
        {
            var friendship = await _context.Friendships.FindAsync(dto.FriendshipId);

            if (friendship == null)
            {
                throw new NotFoundException("Friend request not found.");
            }

            if (friendship.User2Id != dto.CurrentUserId)
            {
                throw new ForbiddenException("Unauthorized to accept this request.");
            }

            if (friendship.StatusId != (int)FriendshipStatusEnum.Pending)
            {
                throw new ConflictException("This request is no longer pending.");
            }

            friendship.StatusId = (int)FriendshipStatusEnum.Accepted;
            friendship.RequestTimestamp = DateTime.UtcNow; 
            
            _context.Friendships.Update(friendship);
            await _context.SaveChangesAsync();
        }

        public async Task DeclineFriendRequestAsync(FriendshipActionDto dto)
        {
            var friendship = await _context.Friendships.FindAsync(dto.FriendshipId);

            if (friendship == null)
            {
                throw new NotFoundException("Friend request not found.");
            }

            if (friendship.User2Id != dto.CurrentUserId)
            {
                throw new ForbiddenException("Unauthorized to decline this request.");
            }

            if (friendship.StatusId != (int)FriendshipStatusEnum.Pending)
            {
                throw new ConflictException("This request is no longer pending.");
            }

            _context.Friendships.Remove(friendship);
            await _context.SaveChangesAsync();
        }

        public async Task CancelFriendRequestAsync(FriendshipActionDto dto)
        {
            var friendship = await _context.Friendships.FindAsync(dto.FriendshipId);

            if (friendship == null)
            {
                throw new NotFoundException("Friend request not found.");
            }

            if (friendship.UserId != dto.CurrentUserId)
            {
                throw new ForbiddenException("Unauthorized to cancel this request.");
            }

            if (friendship.StatusId != (int)FriendshipStatusEnum.Pending)
            {
                throw new ConflictException("This request is no longer pending.");
            }
            
            _context.Friendships.Remove(friendship);
            await _context.SaveChangesAsync();
        }

        public async Task<List<FriendshipDto>> GetFriendsListAsync(int userId)
        {
            return await _context.Friendships
                .Where(f => (f.UserId == userId || f.User2Id == userId) && f.StatusId == (int)FriendshipStatusEnum.Accepted)
                .Include(f => f.User)
                .Include(f => f.User2)
                .Select(f => new FriendshipDto
                {
                    FriendshipId = f.Id,
                    FriendId = f.UserId == userId ? f.User2Id : f.UserId,
                    FriendUsername = f.UserId == userId ? f.User2.UserName : f.User.UserName,
                    Status = f.Status.StatusName,
                    Since = f.RequestTimestamp
                })
                .ToListAsync();
        }

        public async Task<List<PendingFriendshipRequestDto>> GetPendingRequestsAsync(int userId)
        {
            return await _context.Friendships
                .Where(f => f.User2Id == userId && f.StatusId == (int)FriendshipStatusEnum.Pending)
                .Include(f => f.User)
                .Select(f => new PendingFriendshipRequestDto
                {
                    FriendshipId = f.Id,
                    RequesterId = f.UserId,
                    RequesterUsername = f.User.UserName,
                    RequestTimestamp = f.RequestTimestamp
                })
                .ToListAsync();
        }

        public async Task<string> GetFriendshipStatusAsync(int userId1, int userId2)
        {
            var friendship = await _context.Friendships
                .Include(f => f.Status)
                .FirstOrDefaultAsync(f => (f.UserId == userId1 && f.User2Id == userId2) ||
                                          (f.UserId == userId2 && f.User2Id == userId1));
            
            return friendship?.Status.StatusName ?? "None";
        }

        public async Task BlockUserAsync(BlockUserDto dto)
        {
            var existingFriendship = await GetExistingFriendship(dto.BlockerId, dto.UserToBlockId);

            if (existingFriendship != null)
            {
                if (existingFriendship.StatusId == (int)FriendshipStatusEnum.Blocked)
                {
                    throw new ConflictException("This user relationship is already blocked.");
                }

                existingFriendship.StatusId = (int)FriendshipStatusEnum.Blocked;
                existingFriendship.UserId = dto.BlockerId;
                existingFriendship.User2Id = dto.UserToBlockId;
                existingFriendship.RequestTimestamp = DateTime.UtcNow;
                _context.Friendships.Update(existingFriendship);
            }
            else
            {
                var newBlock = new Friendship
                {
                    UserId = dto.BlockerId,
                    User2Id = dto.UserToBlockId,
                    StatusId = (int)FriendshipStatusEnum.Blocked,
                    RequestTimestamp = DateTime.UtcNow
                };
                _context.Friendships.Add(newBlock);
            }

            await _context.SaveChangesAsync();
        }

        public async Task UnblockUserAsync(UnblockUserDto dto)
        {
            var friendship = await _context.Friendships
                .FirstOrDefaultAsync(f => f.UserId == dto.BlockerId && 
                                          f.User2Id == dto.UserToUnblockId && 
                                          f.StatusId == (int)FriendshipStatusEnum.Blocked);

            if (friendship == null)
            {
                throw new NotFoundException("No active block found from you to this user.");
            }

            _context.Friendships.Remove(friendship);
            await _context.SaveChangesAsync();
        }
    }
}