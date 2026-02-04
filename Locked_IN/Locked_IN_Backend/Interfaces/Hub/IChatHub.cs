using Locked_IN_Backend.DTOs.Chat;

namespace Locked_IN_Backend.Interfaces;

public interface IChatHub
{
    Task UserJoined(string connectionId);
    Task UserLeft(string connectionId);
    Task ReceiveMessage(GetMessageDto message);
    Task MessageEdited(GetMessageDto message);
    Task MessageDeleted(int messageId);
    Task MessageRead(object readReceipt);
}