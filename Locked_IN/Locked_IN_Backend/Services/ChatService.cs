using AutoMapper;
using ErrorOr;
using Locked_IN_Backend.Data.Entities;
using Locked_IN_Backend.Data.Enums;
using Locked_IN_Backend.DTOs.Chat;
using Locked_IN_Backend.Exceptions;
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
    private readonly IFileUploadService _fileUploadService;
    private readonly IMapper _mapper;

    public ChatService(
        IChatRepository chatRepository, 
        IMessageRepository messageRepository,
        IChatParticipantRepository participantRepository,
        IUserRepository userRepository, 
        ITeamRepository teamRepository,
        IFileUploadService fileUploadService,
        IMapper mapper)
    {
        _chatRepository = chatRepository;
        _messageRepository = messageRepository;
        _participantRepository = participantRepository;
        _userRepository = userRepository;
        _teamRepository = teamRepository;
        _fileUploadService = fileUploadService;
        _mapper = mapper;
    }

    public async Task<GetChatDetails> CreateDirectChatAsync(int creatorId, int targetUserId)
    {
        var participantIds = new HashSet<int>{ creatorId, targetUserId };
        var existingChat = await _chatRepository.GetDirectChatAsync(participantIds);
        if (existingChat != null)
        {
            throw new ConflictException($"Direct chat already exists. Existing chat id: {existingChat.Id}");
        }
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
                throw new NotFoundException("One or more participants not found.");
            }
        }
        
        var defaultRole = await _participantRepository.GetDefaultRoleAsync();
        if (defaultRole == null)
        {
            throw new NotFoundException("Default role not found.");
        }
        var currentTimestamp = DateTimeHelper.ToUnspecified(DateTime.UtcNow);
        var chat = new Chat
        {
            Type = nameof(ChatType.Direct),
            CreatedAt = currentTimestamp
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
                JoinedAt = currentTimestamp,
                UnreadCount = 0
            };
            await _participantRepository.AddParticipantAsync(participant);
        }
            
        await _participantRepository.SaveChangesAsync();
        var result = await GetChatByIdAsync(creatorId, chat.Id);
        return result;
    }
    
    public async Task<GetChatDetails> CreateTeamChatAsync(int creatorId, int teamId)
    {
        var team = await _teamRepository.GetTeamById(teamId);
        if (team == null)
        {
            throw new NotFoundException($"Team with id {teamId} not found.");
        }
        var existingChat = await _chatRepository.GetTeamChatAsync(teamId);
        if (existingChat != null)
        {
            throw new ConflictException($"Team chat already exists. Existing chat id: {existingChat.Id}");
        }
        
        var defaultRole = await _participantRepository.GetDefaultRoleAsync();
        if (defaultRole == null)
        {
            throw new NotFoundException("Default role not found.");
        }
        var creator = await _userRepository.GetUserById(creatorId);
        if (creator == null)
        {
            throw new NotFoundException($"Creator with id {creatorId} not found.");
        }
        var currentTimestamp = DateTimeHelper.ToUnspecified(DateTime.UtcNow);
        var chat = new Chat
        {
            Type = nameof(ChatType.Team),
            TeamId = teamId,
            CreatedAt = currentTimestamp
        };
        
        await _chatRepository.AddChatAsync(chat);
        await _chatRepository.SaveChangesAsync();
        
        var participant = new Chatparticipant
        {
            ChatId = chat.Id,
            UserId = creatorId,
            RoleId = defaultRole.Id,
            JoinedAt = currentTimestamp,
            UnreadCount = 0
        };
        await _participantRepository.AddParticipantAsync(participant);
            
        await _participantRepository.SaveChangesAsync();
        var result = await GetChatByIdAsync(creatorId, chat.Id);
        return result;
    }

    public async Task<List<GetUserChatsDto>> GetUserChatsAsync(int userId)
    {
        var participants = await _participantRepository.GetUserChatParticipantsAsync(userId);
        var chatDtos = new List<GetUserChatsDto>();

        foreach (var participant in participants)
        {
            var chatDto = _mapper.Map<GetUserChatsDto>(participant.Chat);
            chatDto.UnreadMessageCount = participant.UnreadCount;
            if (participant.Chat.Type.Equals(nameof(ChatType.Direct)))
            {
<<<<<<< Updated upstream
                chatDto.ChatIconUrl = _participantRepository.GetOtherParticipantsAsync(participant.ChatId, userId)
                    .Result.FirstOrDefault(p => p.UserId != userId)?.User.AvatarUrl;
=======
                var otherParticipant = participant.Chat.Chatparticipants.FirstOrDefault(cp => cp.UserId != userId);
                chatDto.ChatName = otherParticipant?.User.UserName;
                chatDto.ChatIconUrl = otherParticipant?.User.AvatarUrl;
>>>>>>> Stashed changes
            }
            else if (participant.Chat.Type.Equals(nameof(ChatType.Team)) && participant.Chat.TeamId != null)
            {
                chatDto.ChatIconUrl = _teamRepository.GetTeamById(participant.Chat.TeamId ?? -1).Result?.IconUrl;
            }

            var lastMessage = await _messageRepository.GetLastMessageAsync(participant.ChatId);
            if (lastMessage != null)
            {
                chatDto.LastMessageContent = lastMessage.Content;
                chatDto.LastMessageUsername = lastMessage.ChatparticipantChatparticipant.User.UserName;
            }

            chatDtos.Add(chatDto);
        }

        var orderedChats = chatDtos.OrderByDescending(c => c.LastMessageTime ?? c.CreationTimestamp).ToList();
        return orderedChats;
    }
    

    public async Task<GetChatDetails> GetChatByIdAsync(int userId, int chatId)
    {
        var participant = await _participantRepository.GetParticipantAsync(chatId, userId);
        if (participant == null)
        {
            throw new ForbiddenException("You are not a participant in this chat.");
        }
        var chat = await _chatRepository.GetChatByIdAsync(chatId);
        if (chat == null)
        {
            throw new NotFoundException("Chat not found.");
        }
        var chatDto = _mapper.Map<GetChatDetails>(chat);
        var messages = _messageRepository.GetChatMessagesAsync(chatId, 1, 50).Result;
        chatDto.MessageDtos = _mapper.Map<List<GetMessageDto>>(messages.OrderBy(m => m.SentAt));
        return chatDto;
    }
    
    public async Task MarkChatAsReadAsync(int userId, int chatId)
    {
        var participant = await _participantRepository.GetParticipantAsync(chatId, userId);
        if (participant == null)
        {
            throw new ForbiddenException("You are not a participant in this chat.");
        }
    
        participant.LastReadAt = DateTimeHelper.ToUnspecified(DateTime.UtcNow);
        participant.UnreadCount = 0;
    
        await _participantRepository.UpdateParticipantAsync(participant);
        await _participantRepository.SaveChangesAsync();
    }

    public async Task JoinChatGroupAsync(int userId, int chatId)
    {
        var chat = await _chatRepository.GetChatByIdAsync(chatId);
        if (chat == null)
        {
            throw new NotFoundException("Chat not found.");
        }
    
        var existingParticipant = await _participantRepository.GetParticipantAsync(chatId, userId);
        if (existingParticipant != null)
        {
            throw new ConflictException($"You are already a participant in chat with id {chatId}.");
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
    }

    public async Task LeaveChatGroupAsync(int userId, int chatId)
    {
        var participant = await _participantRepository.GetParticipantAsync(chatId, userId);
        if (participant == null)
        {
            throw new ForbiddenException("You are not a participant in this chat.");
        }
    
        await _participantRepository.RemoveParticipantAsync(participant);
        await _participantRepository.SaveChangesAsync();
    }
    
    
}

