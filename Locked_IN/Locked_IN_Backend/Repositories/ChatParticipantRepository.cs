using Locked_IN_Backend.Data;
using Locked_IN_Backend.Data.Entities;
using Locked_IN_Backend.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Locked_IN_Backend.Repositories;

public class ChatParticipantRepository : IChatParticipantRepository
{
    private readonly AppDbContext _context;

    public ChatParticipantRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Chatparticipant>> GetUserChatParticipantsAsync(int userId)
    {
        return await _context.Chatparticipants
            .Include(cp => cp.Chat)
                .ThenInclude(c => c.Chatparticipants)
                    .ThenInclude(cp => cp.User)
            .Where(cp => cp.UserId == userId)
            .ToListAsync();
    }

    public async Task<Chatparticipant?> GetParticipantAsync(int chatId, int userId)
    {
        return await _context.Chatparticipants
            .Include(cp => cp.Chat)
            .Include(cp => cp.User)
            .FirstOrDefaultAsync(cp => cp.ChatId == chatId && cp.UserId == userId);
    }

    public async Task<List<Chatparticipant>> GetOtherParticipantsAsync(int chatId, int userId)
    {
        return await _context.Chatparticipants
            .Where(cp => cp.ChatId == chatId && cp.UserId != userId)
            .ToListAsync();
    }

    public async Task<Role?> GetDefaultRoleAsync()
    {
        var role = await _context.Roles.FirstOrDefaultAsync(r => r.Id == 1);
        if (role == null)
        {
            role = await _context.Roles.FirstOrDefaultAsync();
        }
        return role;
    }

    public async Task AddParticipantAsync(Chatparticipant participant)
    {
        await _context.Chatparticipants.AddAsync(participant);
    }

    public async Task UpdateParticipantAsync(Chatparticipant participant)
    {
        _context.Chatparticipants.Update(participant);
        await Task.CompletedTask;
    }

    public async Task RemoveParticipantAsync(Chatparticipant participant)
    {
        _context.Chatparticipants.Remove(participant);
        await Task.CompletedTask;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
