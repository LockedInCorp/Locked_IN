using Locked_IN_Backend.Data;
using Locked_IN_Backend.Data.Entities;
using Locked_IN_Backend.Data.Enums;
using Locked_IN_Backend.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Locked_IN_Backend.Repositories;

public class ChatRepository : IChatRepository
{
    private readonly AppDbContext _context;

    public ChatRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Chat?> GetChatByIdAsync(int chatId)
    {
        return await _context.Chats.FindAsync(chatId);
    }

    public async Task<Chat?> GetChatWithParticipantsAsync(int chatId)
    {
        return await _context.Chats
            .Include(c => c.Chatparticipants)
                .ThenInclude(cp => cp.User)
            .FirstOrDefaultAsync(c => c.Id == chatId);
    }

    public async Task<Chat?> GetDirectChatAsync(HashSet<int> participantIds)
    {
        var directType = ChatType.Direct.ToString();
        return await _context.Chats
            .Where(c => c.Type == directType)
            .Where(c => c.Chatparticipants.Count == participantIds.Count && 
                       c.Chatparticipants.All(cp => participantIds.Contains(cp.UserId)))
            .FirstOrDefaultAsync();
    }

    public async Task AddChatAsync(Chat chat)
    {
        await _context.Chats.AddAsync(chat);
    }

    public async Task UpdateChatAsync(Chat chat)
    {
        _context.Chats.Update(chat);
        await Task.CompletedTask;
    }

    public async Task DeleteChatAsync(Chat chat)
    {
        _context.Chats.Remove(chat);
        await Task.CompletedTask;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
