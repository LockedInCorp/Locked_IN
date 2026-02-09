using Locked_IN_Backend.Data;
using Locked_IN_Backend.Data.Entities;
using Locked_IN_Backend.Misc.Enum;
using Microsoft.EntityFrameworkCore;

namespace Locked_IN_Backend.Repositories;

public class FriendshipRepository : IFriendshipRepository
{
    private readonly AppDbContext _context;

    public FriendshipRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Friendship?> GetFriendshipByIdAsync(int friendshipId)
    {
        return await _context.Friendships
            .Include(f => f.Status)
            .FirstOrDefaultAsync(f => f.Id == friendshipId);
    }

    public async Task<Friendship?> GetFriendshipBetweenUsersAsync(int userId1, int userId2)
    {
        return await _context.Friendships
            .Include(f => f.Status)
            .FirstOrDefaultAsync(f => 
                (f.UserId == userId1 && f.User2Id == userId2) || 
                (f.UserId == userId2 && f.User2Id == userId1));
    }

    public async Task<List<Friendship>> GetAcceptedFriendshipsAsync(int userId)
    {
        return await _context.Friendships
            .Include(f => f.User)
            .Include(f => f.User2)
            .Include(f => f.Status)
            .Where(f => (f.UserId == userId || f.User2Id == userId) 
                        && f.StatusId == (int)FriendshipStatusEnum.Accepted)
            .ToListAsync();
    }

    public async Task<List<Friendship>> GetPendingIncomingRequestsAsync(int userId)
    {
        // Запросы где User2 - это текущий пользователь, и статус Pending
        return await _context.Friendships
            .Include(f => f.User) // Загружаем инфо о том, КТО отправил (UserId)
            .Where(f => f.User2Id == userId && f.StatusId == (int)FriendshipStatusEnum.Pending)
            .ToListAsync();
    }

    public async Task AddFriendshipAsync(Friendship friendship)
    {
        await _context.Friendships.AddAsync(friendship);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateFriendshipAsync(Friendship friendship)
    {
        _context.Friendships.Update(friendship);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteFriendshipAsync(Friendship friendship)
    {
        _context.Friendships.Remove(friendship);
        await _context.SaveChangesAsync();
    }
}