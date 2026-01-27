using Locked_IN_Backend.Data.Entities;

namespace Locked_IN_Backend.Interfaces.Repositories;

public interface IChatParticipantRepository
{
    Task<List<Chatparticipant>> GetUserChatParticipantsAsync(int userId);
    Task<Chatparticipant?> GetParticipantAsync(int chatId, int userId);
    Task<List<Chatparticipant>> GetOtherParticipantsAsync(int chatId, int userId);
    Task<Role?> GetDefaultRoleAsync();
    Task AddParticipantAsync(Chatparticipant participant);
    Task UpdateParticipantAsync(Chatparticipant participant);
    Task RemoveParticipantAsync(Chatparticipant participant);
    Task SaveChangesAsync();
}
