namespace Locked_IN_Backend.DTOs.Chat;

public class GetChatDetails
{
    public int Id { get; set; }
    public string? ChatName { get; set; }
    public string? ChatIconUrl { get; set; }
    public List<GetMessageDto> MessageDtos;
}