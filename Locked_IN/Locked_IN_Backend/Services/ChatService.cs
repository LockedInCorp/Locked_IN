using AutoMapper;
using Locked_IN_Backend.Data.Entities;
using Locked_IN_Backend.Data.Enums;
using Locked_IN_Backend.DTOs.Chat;
using Locked_IN_Backend.Interfaces;
using Locked_IN_Backend.Interfaces.Repositories;

namespace Locked_IN_Backend.Services;

// Helper to convert UTC DateTime to Unspecified for PostgreSQL compatibility
public static class DateTimeHelper
{
    public static DateTime ToUnspecified(DateTime dateTime)
    {
        return DateTime.SpecifyKind(dateTime, DateTimeKind.Unspecified);
    }
}

public class ChatService : IChatService
{
    private readonly IChatRepository _chatRepository;
    private readonly IMessageRepository _messageRepository;
    private readonly IChatParticipantRepository _participantRepository;
    private readonly IUserRepository _userRepository;
    private readonly ITeamRepository _teamRepository;
    private readonly IMapper _mapper;

    public ChatService(
        IChatRepository chatRepository, 
        IMessageRepository messageRepository,
        IChatParticipantRepository participantRepository,
        IUserRepository userRepository, 
        ITeamRepository teamRepository,
        IMapper mapper)
    {
        _chatRepository = chatRepository;
        _messageRepository = messageRepository;
        _participantRepository = participantRepository;
        _userRepository = userRepository;
        _teamRepository = teamRepository;
        _mapper = mapper;
    }

    public async Task<ChatResult> CreateDirectChatAsync(int creatorId, int targetUserId)
    {
        
        var createChatDto = new CreateChatDto
        {
            Type = ChatType.Direct,
            ParticipantUserIds = new List<int> { targetUserId, creatorId }
        };
        return await CreateChatAsync(creatorId, createChatDto);
    }

    public async Task<ChatResult> CreateTeamChatAsync(int creatorId, int teamId)
    {
        try
        {
            var team = await _teamRepository.GetTeamById(teamId);
            if (team == null)
            {
                return new ChatResult(false, "Team not found.");
            }

            var participantUserIds = team.TeamMembers.Select(tm => tm.UserId).ToList();
            if (!participantUserIds.Contains(creatorId))
            {
                return new ChatResult(false, "You are not a member of this team.");
            }

            var createChatDto = new CreateChatDto
            {
                Type = ChatType.Team,
                Name = team.Name,
                TeamId = teamId,
                ParticipantUserIds = participantUserIds
            };

            return await CreateChatAsync(creatorId, createChatDto);
        }
        catch (Exception ex)
        {
            return new ChatResult(false, $"Error creating team chat: {ex.Message}");
        }
    }

    public async Task<ChatResult> CreateChatAsync(int creatorId, CreateChatDto createChatDto)
    {
        try
        {
            var creator = await _userRepository.GetUserById(creatorId);
            if (creator == null)
            {
                return new ChatResult(false, "Creator user not found.");
            }

            var participantIds = new HashSet<int>(createChatDto.ParticipantUserIds) { creatorId };
            var participants = new List<User>();
            foreach (var id in participantIds)
            {
                var user = await _userRepository.GetUserById(id);
                if (user != null)
                {
                    participants.Add(user);
                }
                else
                {
                    return new ChatResult(false, "One or more participants not found.");
                }
            }
               
            if (createChatDto.Type == ChatType.Direct && participantIds.Count == 2)
            {
                var existingChat = await _chatRepository.GetDirectChatAsync(participantIds);
                if (existingChat != null)
                {
                    var chatDto = await GetChatDtoAsync(existingChat.Id, creatorId);
                    return new ChatResult(true, "Direct chat already exists.", chatDto);
                }
            }

            var defaultRole = await _participantRepository.GetDefaultRoleAsync();
            if (defaultRole == null)
            {
                return new ChatResult(false, "Default role not found.");
            }

            var chat = new Chat
            {
                Type = createChatDto.Type.ToString(),
                Name = createChatDto.Name,
                TeamId = createChatDto.TeamId,
                CreatedAt = DateTimeHelper.ToUnspecified(DateTime.UtcNow)
            };

            await _chatRepository.AddChatAsync(chat);
            await _chatRepository.SaveChangesAsync();

            foreach (var userId in participantIds)
            {
                var participant = new Chatparticipant
                {
                    ChatId = chat.Id,
                    UserId = userId,
                    RoleId = defaultRole.Id,
                    JoinedAt = DateTimeHelper.ToUnspecified(DateTime.UtcNow),
                    UnreadCount = 0
                };
                await _participantRepository.AddParticipantAsync(participant);
            }
            
            await _participantRepository.SaveChangesAsync();

            var createdChatDto = await GetChatDtoAsync(chat.Id, creatorId);
            return new ChatResult(true, "Chat created successfully.", createdChatDto);
        }
        catch (Exception ex)
        {
            return new ChatResult(false, $"Error creating chat: {ex.Message}");
        }
    }

    public async Task<ChatResult> SendMessageAsync(int userId, SendMessageDto sendMessageDto)
    {
        try
        {
            var participant = await _participantRepository.GetParticipantAsync(sendMessageDto.ChatId, userId);
            if (participant == null)
            {
                return new ChatResult(false, "You are not a participant in this chat.");
            }

            var message = new Message
            {
                Content = sendMessageDto.Content,
                SentAt = DateTimeHelper.ToUnspecified(DateTime.UtcNow),
                AttachmentUrl = sendMessageDto.AttachmentUrl,
                ChatparticipantChatparticipantId = participant.ChatparticipantId,
                IsDeleted = false
            };

            await _messageRepository.AddMessageAsync(message);

            participant.Chat.LastMessageAt = DateTimeHelper.ToUnspecified(DateTime.UtcNow);
            await _chatRepository.UpdateChatAsync(participant.Chat);

            var otherParticipants = await _participantRepository.GetOtherParticipantsAsync(sendMessageDto.ChatId, userId);
            foreach (var otherParticipant in otherParticipants)
            {
                otherParticipant.UnreadCount++;
                await _participantRepository.UpdateParticipantAsync(otherParticipant);
            }

            await _chatRepository.SaveChangesAsync();

            var messageDto = _mapper.Map<MessageResponseDto>(message);
            return new ChatResult(true, "Message sent successfully.", messageDto);
        }
        catch (Exception ex)
        {
            return new ChatResult(false, $"Error sending message: {ex.Message}");
        }
    }

    public async Task<ChatResult> GetChatMessagesAsync(int userId, int chatId, int pageNumber = 1, int pageSize = 50)
    {
        var participant = await _participantRepository.GetParticipantAsync(chatId, userId);
        if (participant == null)
        {
            return new ChatResult(false, "You are not a participant in this chat.");
        }

        var messages = await _messageRepository.GetChatMessagesAsync(chatId, pageNumber, pageSize);
        var messageDtos = _mapper.Map<List<MessageResponseDto>>(messages.OrderBy(m => m.SentAt));

        return new ChatResult(true, "Messages retrieved successfully.", messageDtos);
    }

    public async Task<ChatResult> GetUserChatsAsync(int userId)
    {
        var participants = await _participantRepository.GetUserChatParticipantsAsync(userId);
        var chatDtos = new List<ChatResponseDto>();

        foreach (var participant in participants)
        {
            var chatDto = _mapper.Map<ChatResponseDto>(participant.Chat);
            chatDto.UnreadCount = participant.UnreadCount;
            
            var lastMessage = await _messageRepository.GetLastMessageAsync(participant.ChatId);
            if (lastMessage != null)
            {
                chatDto.LastMessage = _mapper.Map<MessageResponseDto>(lastMessage);
            }
            
            chatDtos.Add(chatDto);
        }

        var orderedChats = chatDtos.OrderByDescending(c => c.LastMessageAt ?? c.CreatedAt).ToList();
        return new ChatResult(true, "Chats retrieved successfully.", orderedChats);
    }

    public async Task<ChatResult> GetChatByIdAsync(int userId, int chatId)
    {
        var participant = await _participantRepository.GetParticipantAsync(chatId, userId);
        if (participant == null)
        {
            return new ChatResult(false, "You are not a participant in this chat.");
        }

        var chatDto = await GetChatDtoAsync(chatId, userId);
        return new ChatResult(true, "Chat retrieved successfully.", chatDto);
    }

    public async Task<ChatResult> EditMessageAsync(int userId, EditMessageDto editMessageDto)
    {
        var message = await _messageRepository.GetMessageByIdAsync(editMessageDto.MessageId);

        if (message == null)
        {
            return new ChatResult(false, "Message not found.");
        }

        if (message.ChatparticipantChatparticipant.UserId != userId)
        {
            return new ChatResult(false, "You can only edit your own messages.");
        }

        if (message.IsDeleted)
        {
            return new ChatResult(false, "Cannot edit deleted message.");
        }

        message.Content = editMessageDto.Content;
        message.EditedAt = DateTimeHelper.ToUnspecified(DateTime.UtcNow);

        await _messageRepository.UpdateMessageAsync(message);
        await _messageRepository.SaveChangesAsync();

        var messageDto = _mapper.Map<MessageResponseDto>(message);
        return new ChatResult(true, "Message edited successfully.", messageDto);
    }

    public async Task<ChatResult> DeleteMessageAsync(int userId, int messageId)
    {
        var message = await _messageRepository.GetMessageByIdAsync(messageId);

        if (message == null)
        {
            return new ChatResult(false, "Message not found.");
        }

        if (message.ChatparticipantChatparticipant.UserId != userId)
        {
            return new ChatResult(false, "You can only delete your own messages.");
        }

        if (message.IsDeleted)
        {
            return new ChatResult(false, "Message is already deleted.");
        }

        if (message.AttachmentUrl != null)
        {
            //#TODO delete file if it will be uploaded
        }
        message.IsDeleted = true;
        message.DeletedAt = DateTimeHelper.ToUnspecified(DateTime.UtcNow);

        await _messageRepository.UpdateMessageAsync(message);
        await _messageRepository.SaveChangesAsync();

        return new ChatResult(true, "Message deleted successfully.", new { MessageId = messageId });
    }

    public async Task<ChatResult> MarkChatAsReadAsync(int userId, int chatId)
    {
        var participant = await _participantRepository.GetParticipantAsync(chatId, userId);
        if (participant == null)
        {
            return new ChatResult(false, "You are not a participant in this chat.");
        }

        participant.LastReadAt = DateTimeHelper.ToUnspecified(DateTime.UtcNow);
        participant.UnreadCount = 0;

        await _participantRepository.UpdateParticipantAsync(participant);
        await _participantRepository.SaveChangesAsync();

        return new ChatResult(true, "Chat marked as read.", new { ChatId = chatId, UnreadCount = 0 });
    }

    public async Task<ChatResult> JoinChatGroupAsync(int userId, int chatId)
    {
        var chat = await _chatRepository.GetChatByIdAsync(chatId);
        if (chat == null)
        {
            return new ChatResult(false, "Chat not found.");
        }

        var existingParticipant = await _participantRepository.GetParticipantAsync(chatId, userId);
        if (existingParticipant != null)
        {
            return new ChatResult(true, "Already a participant in this chat.");
        }

        var defaultRole = await _participantRepository.GetDefaultRoleAsync();
        var participant = new Chatparticipant
        {
            ChatId = chatId,
            UserId = userId,
            RoleId = defaultRole?.Id ?? 1,
            JoinedAt = DateTimeHelper.ToUnspecified(DateTime.UtcNow),
            UnreadCount = 0
        };

        await _participantRepository.AddParticipantAsync(participant);
        await _participantRepository.SaveChangesAsync();

        return new ChatResult(true, "Joined chat successfully.");
    }

    public async Task<ChatResult> LeaveChatGroupAsync(int userId, int chatId)
    {
        var participant = await _participantRepository.GetParticipantAsync(chatId, userId);
        if (participant == null)
        {
            return new ChatResult(false, "You are not a participant in this chat.");
        }

        await _participantRepository.RemoveParticipantAsync(participant);
        await _participantRepository.SaveChangesAsync();

        return new ChatResult(true, "Left chat successfully.");
    }

    private async Task<ChatResponseDto?> GetChatDtoAsync(int chatId, int userId)
    {
        var participant = await _participantRepository.GetParticipantAsync(chatId, userId);
        if (participant == null)
        {
            return null;
        }

        var chat = await _chatRepository.GetChatWithParticipantsAsync(chatId);
        if (chat == null) return null;

        var chatDto = _mapper.Map<ChatResponseDto>(chat);
        chatDto.UnreadCount = participant.UnreadCount;

        var lastMessage = await _messageRepository.GetLastMessageAsync(chatId);
        if (lastMessage != null)
        {
            chatDto.LastMessage = _mapper.Map<MessageResponseDto>(lastMessage);
        }

        return chatDto;
    }
}

