using AutoMapper;
using Locked_IN_Backend.Data.Entities;
using Locked_IN_Backend.DTOs.Friendship;
using Locked_IN_Backend.Exceptions;
using Locked_IN_Backend.Interfaces.Repositories;
using Locked_IN_Backend.Misc.Enum;
using Locked_IN_Backend.Repositories;

namespace Locked_IN_Backend.Services;

public class FriendshipService : IFriendshipService
{
    private readonly IFriendshipRepository _friendshipRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public FriendshipService(IFriendshipRepository friendshipRepository, IUserRepository userRepository, IMapper mapper)
    {
        _friendshipRepository = friendshipRepository;
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task SendFriendRequestAsync(int requesterId, int receiverId)
    {
        if (requesterId == receiverId) throw new BadRequestException("You cannot send a friend request to yourself.");

        var receiver = await _userRepository.GetUserById(receiverId);
        if (receiver == null) throw new NotFoundException("User not found.");

        var existingFriendship = await _friendshipRepository.GetFriendshipBetweenUsersAsync(requesterId, receiverId);

        if (existingFriendship != null)
        {
            if (existingFriendship.StatusId == (int)FriendshipStatusEnum.Blocked)
                throw new ForbiddenException("Unable to send friend request."); 
            
            if (existingFriendship.StatusId == (int)FriendshipStatusEnum.Accepted)
                throw new ConflictException("You are already friends.");

            if (existingFriendship.StatusId == (int)FriendshipStatusEnum.Pending)
                throw new ConflictException("A friend request is already pending.");
        }

        var friendship = new Friendship
        {
            UserId = requesterId,
            User2Id = receiverId,
            StatusId = (int)FriendshipStatusEnum.Pending,
            RequestTimestamp = DateTime.UtcNow
        };

        await _friendshipRepository.AddFriendshipAsync(friendship);
    }

    public async Task AcceptFriendRequestAsync(int userId, int friendshipId)
    {
        var friendship = await _friendshipRepository.GetFriendshipByIdAsync(friendshipId);
        if (friendship == null) throw new NotFoundException("Friend request not found.");

        if (friendship.User2Id != userId) throw new ForbiddenException("You are not authorized to accept this request.");

        if (friendship.StatusId != (int)FriendshipStatusEnum.Pending) throw new BadRequestException("Request is not pending.");

        friendship.StatusId = (int)FriendshipStatusEnum.Accepted;
        await _friendshipRepository.UpdateFriendshipAsync(friendship);
    }

    public async Task DeclineFriendRequestAsync(int userId, int friendshipId)
    {
        var friendship = await _friendshipRepository.GetFriendshipByIdAsync(friendshipId);
        if (friendship == null) throw new NotFoundException("Friend request not found.");

        if (friendship.User2Id != userId) throw new ForbiddenException("You are not authorized to decline this request.");

        if (friendship.StatusId != (int)FriendshipStatusEnum.Pending) throw new BadRequestException("Request is not pending.");

        await _friendshipRepository.DeleteFriendshipAsync(friendship);
    }

    public async Task CancelFriendRequestAsync(int userId, int friendshipId)
    {
        var friendship = await _friendshipRepository.GetFriendshipByIdAsync(friendshipId);
        if (friendship == null) throw new NotFoundException("Friend request not found.");

        if (friendship.UserId != userId) throw new ForbiddenException("You are not authorized to cancel this request.");

        if (friendship.StatusId != (int)FriendshipStatusEnum.Pending) throw new BadRequestException("Request is not pending.");

        await _friendshipRepository.DeleteFriendshipAsync(friendship);
    }

    public async Task RemoveFriendAsync(int userId, int friendId)
    {
        var friendship = await _friendshipRepository.GetFriendshipBetweenUsersAsync(userId, friendId);
        
        if (friendship == null || friendship.StatusId != (int)FriendshipStatusEnum.Accepted)
        {
            throw new NotFoundException("Friendship not found.");
        }

        await _friendshipRepository.DeleteFriendshipAsync(friendship);
    }

    public async Task<List<FriendshipDto>> GetFriendsListAsync(int userId)
    {
        var friendships = await _friendshipRepository.GetAcceptedFriendshipsAsync(userId);
        
        return friendships.Select(f => {
            var isUser1 = f.UserId == userId;
            var friend = isUser1 ? f.User2 : f.User;
            
            return new FriendshipDto
            {
                FriendshipId = f.Id,
                FriendId = friend.Id,
                FriendUsername = friend.UserName,
                FriendAvatarUrl = friend.AvatarUrl,
                Status = f.Status.StatusName,
                Since = f.RequestTimestamp
            };
        }).ToList();
    }

    public async Task<List<PendingFriendshipRequestDto>> GetPendingRequestsAsync(int userId)
    {
        var requests = await _friendshipRepository.GetPendingIncomingRequestsAsync(userId);
        return _mapper.Map<List<PendingFriendshipRequestDto>>(requests);
    }

    public async Task<string> GetFriendshipStatusAsync(int userId1, int userId2)
    {
        var friendship = await _friendshipRepository.GetFriendshipBetweenUsersAsync(userId1, userId2);
        return friendship?.Status.StatusName ?? "None";
    }

    public async Task BlockUserAsync(int blockerId, int userToBlockId)
    {
        if (blockerId == userToBlockId) throw new BadRequestException("Cannot block yourself.");

        var friendship = await _friendshipRepository.GetFriendshipBetweenUsersAsync(blockerId, userToBlockId);

        if (friendship != null)
        {
            friendship.StatusId = (int)FriendshipStatusEnum.Blocked;
            friendship.UserId = blockerId; 
            friendship.User2Id = userToBlockId;
            await _friendshipRepository.UpdateFriendshipAsync(friendship);
        }
        else
        {
            friendship = new Friendship
            {
                UserId = blockerId,
                User2Id = userToBlockId,
                StatusId = (int)FriendshipStatusEnum.Blocked,
                RequestTimestamp = DateTime.UtcNow
            };
            await _friendshipRepository.AddFriendshipAsync(friendship);
        }
    }

    public async Task UnblockUserAsync(int blockerId, int userToUnblockId)
    {
        var friendship = await _friendshipRepository.GetFriendshipBetweenUsersAsync(blockerId, userToUnblockId);

        if (friendship == null || friendship.StatusId != (int)FriendshipStatusEnum.Blocked)
        {
            throw new NotFoundException("Block record not found.");
        }
        
        if (friendship.UserId == blockerId)
        {
            await _friendshipRepository.DeleteFriendshipAsync(friendship);
        }
    }
}