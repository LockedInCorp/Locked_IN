using Locked_IN_Backend.DTOs;
using Locked_IN_Backend.DTOs.Chat;

namespace Locked_IN_Backend.Interfaces;

public interface IMessageService
{
    Task<GetMessageDto> SendMessageAsync(int userId, SendMessageDto sendMessageDto);
    Task<PagedResult<GetMessageDto>> GetChatMessagesAsync(int userId, int chatId, int pageNumber = 1, int pageSize = 50);
    Task<GetMessageDto> EditMessageAsync(int userId, EditMessageDto editMessageDto);
    Task DeleteMessageAsync(int userId, int messageId);
}
