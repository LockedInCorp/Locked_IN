using Locked_IN_Backend.Data.Entities;

namespace Locked_IN_Backend.Interfaces.Repositories;

public interface IChatRepository
{
    Task<Chat?> GetChatByIdAsync(int chatId);
    Task<Chat?> GetDirectChatAsync(HashSet<int> participantIds);
    Task<Chat?> GetTeamChatAsync(int teamId);
    
    Task AddChatAsync(Chat chat);
    Task UpdateChatAsync(Chat chat);
    Task DeleteChatAsync(Chat chat);
    
    Task SaveChangesAsync();
}
