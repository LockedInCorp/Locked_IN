using Locked_IN_Backend.Data;
using Locked_IN_Backend.Data.Entities;
using Locked_IN_Backend.DTOs;
using Locked_IN_Backend.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Locked_IN_Backend.Repositories;

public class MessageRepository : IMessageRepository
{
    private readonly AppDbContext _context;

    public MessageRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<PagedResult<Message>> GetChatMessagesAsync(int chatId, int pageNumber, int pageSize)
    {
        var query = _context.Messages
            .Include(m => m.ChatparticipantChatparticipant)
                .ThenInclude(cp => cp.User)
            .Where(m => m.ChatparticipantChatparticipant.ChatId == chatId && !m.IsDeleted);

        var totalCount = await query.CountAsync();
        
        var items = await query
            .OrderByDescending(m => m.SentAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResult<Message>
        {
            Items = items,
            TotalCount = totalCount,
            Page = pageNumber,
            PageSize = pageSize,
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
        };
    }

    public async Task<Message?> GetMessageByIdAsync(int messageId)
    {
        return await _context.Messages
            .Include(m => m.ChatparticipantChatparticipant)
            .FirstOrDefaultAsync(m => m.Id == messageId);
    }

    public async Task<Message?> GetLastMessageAsync(int chatId)
    {
        return await _context.Messages
            .Include(m => m.ChatparticipantChatparticipant)
                .ThenInclude(cp => cp.User)
            .Where(m => m.ChatparticipantChatparticipant.ChatId == chatId && !m.IsDeleted)
            .OrderByDescending(m => m.SentAt)
            .FirstOrDefaultAsync();
    }

    public async Task AddMessageAsync(Message message)
    {
        await _context.Messages.AddAsync(message);
    }

    public async Task UpdateMessageAsync(Message message)
    {
        _context.Messages.Update(message);
        await Task.CompletedTask;
    }

    public async Task DeleteMessageAsync(Message message)
    {
        _context.Messages.Remove(message);
        await Task.CompletedTask;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
