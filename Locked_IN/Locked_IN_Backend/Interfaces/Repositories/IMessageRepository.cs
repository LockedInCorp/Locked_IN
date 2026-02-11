using Locked_IN_Backend.Data.Entities;
using Locked_IN_Backend.DTOs;

namespace Locked_IN_Backend.Interfaces.Repositories;

public interface IMessageRepository
{
    Task<PagedResult<Message>> GetChatMessagesAsync(int chatId, int pageNumber, int pageSize);
    Task<Message?> GetMessageByIdAsync(int messageId);
    Task<Message?> GetLastMessageAsync(int chatId);
    Task AddMessageAsync(Message message);
    Task UpdateMessageAsync(Message message);
    Task DeleteMessageAsync(Message message);
    Task SaveChangesAsync();
}
