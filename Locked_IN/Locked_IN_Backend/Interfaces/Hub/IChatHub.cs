using Locked_IN_Backend.DTO;
using Locked_IN_Backend.DTOs.Chat;

namespace Locked_IN_Backend.Interfaces;

public interface IChatHub
{
    Task UserJoined(GetUserForTeamViewDto user);
    Task UserLeft(int userId);
    Task ReceiveMessage(GetMessageDto message);
    Task MessageEdited(GetMessageDto message);
    Task MessageDeleted(int messageId);
    Task MessageRead(object readReceipt);
}