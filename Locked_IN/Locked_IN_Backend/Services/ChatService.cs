using Locked_IN_Backend.Data;
using Locked_IN_Backend.Data.Entities;
using Locked_IN_Backend.DTOs.Chat;
using Locked_IN_Backend.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Locked_IN_Backend.Services;

// Helper to convert UTC DateTime to Unspecified for PostgreSQL compatibility
public static class DateTimeHelper
{
    public static DateTime ToUnspecified(DateTime dateTime)
    {
        return DateTime.SpecifyKind(dateTime, DateTimeKind.Unspecified);
    }
}
//TODO refactore all of this

public class ChatService : IChatService
{
    private readonly AppDbContext _context;

    public ChatService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ChatResult> CreateChatAsync(int creatorId, CreateChatDto createChatDto)
    {
        try
        {
            // Validate user exists
            var creator = await _context.Users.FindAsync(creatorId);
            if (creator == null)
            {
                return new ChatResult(false, "Creator user not found.");
            }

            // Validate participants
            var participantIds = new HashSet<int>(createChatDto.ParticipantUserIds) { creatorId };
            var participants = await _context.Users
                .Where(u => participantIds.Contains(u.Id))
                .ToListAsync();

            if (participants.Count != participantIds.Count)
            {
                return new ChatResult(false, "One or more participants not found.");
            }

            // For Direct chats, check if chat already exists
            if (createChatDto.Type == "Direct" && participantIds.Count == 2)
            {
                var existingChat = await _context.Chats
                    .Where(c => c.Type == "Direct")
                    .SelectMany(c => c.Chatparticipants)
                    .GroupBy(cp => cp.ChatId)
                    .Where(g => g.Count() == 2 && participantIds.All(id => g.Any(cp => cp.UserId == id)))
                    .Select(g => g.First().Chat)
                    .FirstOrDefaultAsync();

                if (existingChat != null)
                {
                    var chatDto = await GetChatDtoAsync(existingChat.Id, creatorId);
                    return new ChatResult(true, "Direct chat already exists.", chatDto);
                }
            }

            // Check if default role exists, if not create it or use first available role
            var defaultRole = await _context.Roles.FirstOrDefaultAsync(r => r.Id == 1);
            if (defaultRole == null)
            {
                // Use first available role or create one
                defaultRole = await _context.Roles.FirstOrDefaultAsync();
                if (defaultRole == null)
                {
                    // Create a default role if none exist
                    defaultRole = new Role { Rolename = "Member" };
                    _context.Roles.Add(defaultRole);
                    await _context.SaveChangesAsync();
                }
            }

            // Create chat
            var chat = new Chat
            {
                Type = createChatDto.Type,
                Name = createChatDto.Name,
                TeamId = createChatDto.TeamId,
                CreatedAt = DateTimeHelper.ToUnspecified(DateTime.UtcNow)
            };

            _context.Chats.Add(chat);
            await _context.SaveChangesAsync();

            // Add participants
            var chatParticipants = participantIds.Select(userId => new Chatparticipant
            {
                ChatId = chat.Id,
                UserId = userId,
                RoleId = defaultRole.Id,
                JoinedAt = DateTimeHelper.ToUnspecified(DateTime.UtcNow),
                UnreadCount = 0
            }).ToList();

            _context.Chatparticipants.AddRange(chatParticipants);
            await _context.SaveChangesAsync();

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
            // Validate user is participant
            var participant = await _context.Chatparticipants
                .Include(cp => cp.Chat)
                .Include(cp => cp.User)
                .FirstOrDefaultAsync(cp => cp.ChatId == sendMessageDto.ChatId && cp.UserId == userId);

            if (participant == null)
            {
                return new ChatResult(false, "You are not a participant in this chat.");
            }

        // Persistence First: Save message to database
        var message = new Message
        {
            Content = sendMessageDto.Content,
            SentAt = DateTimeHelper.ToUnspecified(DateTime.UtcNow),
            AttachmentUrl = sendMessageDto.AttachmentUrl,
            ChatparticipantChatparticipantId = participant.ChatparticipantId,
            IsDeleted = false
        };

        _context.Messages.Add(message);

        // Update chat's last message timestamp
        participant.Chat.LastMessageAt = DateTimeHelper.ToUnspecified(DateTime.UtcNow);

            // Increment unread count for all other participants
            var otherParticipants = await _context.Chatparticipants
                .Where(cp => cp.ChatId == sendMessageDto.ChatId && cp.UserId != userId)
                .ToListAsync();

            foreach (var otherParticipant in otherParticipants)
            {
                otherParticipant.UnreadCount++;
            }

            await _context.SaveChangesAsync();

            // Return message DTO for broadcasting via SignalR
            var messageDto = new MessageResponseDto
            {
                Id = message.Id,
                Content = message.Content,
                SentAt = message.SentAt,
                EditedAt = message.EditedAt,
                IsDeleted = message.IsDeleted,
                AttachmentUrl = message.AttachmentUrl,
                SenderId = userId,
                SenderUsername = participant.User.UserName,
                SenderAvatarUrl = participant.User.AvatarUrl,
                ChatId = sendMessageDto.ChatId
            };

            return new ChatResult(true, "Message sent successfully.", messageDto);
        }
        catch (Exception ex)
        {
            return new ChatResult(false, $"Error sending message: {ex.Message}");
        }
    }

    public async Task<ChatResult> GetChatMessagesAsync(int userId, int chatId, int pageNumber = 1, int pageSize = 50)
    {
        // Validate user is participant
        var isParticipant = await _context.Chatparticipants
            .AnyAsync(cp => cp.ChatId == chatId && cp.UserId == userId);

        if (!isParticipant)
        {
            return new ChatResult(false, "You are not a participant in this chat.");
        }

        var messages = await _context.Messages
            .Include(m => m.ChatparticipantChatparticipant)
                .ThenInclude(cp => cp.User)
            .Where(m => m.ChatparticipantChatparticipant.ChatId == chatId && !m.IsDeleted)
            .OrderByDescending(m => m.SentAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(m => new MessageResponseDto
            {
                Id = m.Id,
                Content = m.Content,
                SentAt = m.SentAt,
                EditedAt = m.EditedAt,
                IsDeleted = m.IsDeleted,
                AttachmentUrl = m.AttachmentUrl,
                SenderId = m.ChatparticipantChatparticipant.UserId,
                SenderUsername = m.ChatparticipantChatparticipant.User.UserName,
                SenderAvatarUrl = m.ChatparticipantChatparticipant.User.AvatarUrl,
                ChatId = chatId
            })
            .OrderBy(m => m.SentAt) // Return in chronological order
            .ToListAsync();

        return new ChatResult(true, "Messages retrieved successfully.", messages);
    }

    public async Task<ChatResult> GetUserChatsAsync(int userId)
    {
        var participants = await _context.Chatparticipants
            .Include(cp => cp.Chat)
                .ThenInclude(c => c.Chatparticipants)
                    .ThenInclude(cp => cp.User)
            .Where(cp => cp.UserId == userId)
            .ToListAsync();

        var chatDtos = new List<ChatResponseDto>();

        foreach (var participant in participants)
        {
            var lastMessage = await _context.Messages
                .Include(m => m.ChatparticipantChatparticipant)
                    .ThenInclude(cp => cp.User)
                .Where(m => m.ChatparticipantChatparticipant.ChatId == participant.ChatId && !m.IsDeleted)
                .OrderByDescending(m => m.SentAt)
                .FirstOrDefaultAsync();

            chatDtos.Add(new ChatResponseDto
            {
                Id = participant.Chat.Id,
                Type = participant.Chat.Type,
                Name = participant.Chat.Name,
                CreatedAt = participant.Chat.CreatedAt,
                LastMessageAt = participant.Chat.LastMessageAt,
                TeamId = participant.Chat.TeamId,
                UnreadCount = participant.UnreadCount,
                Participants = participant.Chat.Chatparticipants.Select(p => new ChatParticipantDto
                {
                    UserId = p.UserId,
                    Username = p.User.UserName,
                    AvatarUrl = p.User.AvatarUrl,
                    RoleId = p.RoleId,
                    JoinedAt = p.JoinedAt
                }).ToList(),
                LastMessage = lastMessage != null ? new MessageResponseDto
                {
                    Id = lastMessage.Id,
                    Content = lastMessage.Content,
                    SentAt = lastMessage.SentAt,
                    EditedAt = lastMessage.EditedAt,
                    IsDeleted = lastMessage.IsDeleted,
                    AttachmentUrl = lastMessage.AttachmentUrl,
                    SenderId = lastMessage.ChatparticipantChatparticipant.UserId,
                    SenderUsername = lastMessage.ChatparticipantChatparticipant.User.UserName,
                    SenderAvatarUrl = lastMessage.ChatparticipantChatparticipant.User.AvatarUrl,
                    ChatId = participant.Chat.Id
                } : null
            });
        }

        var orderedChats = chatDtos.OrderByDescending(c => c.LastMessageAt ?? c.CreatedAt).ToList();

        return new ChatResult(true, "Chats retrieved successfully.", orderedChats);
    }

    public async Task<ChatResult> GetChatByIdAsync(int userId, int chatId)
    {
        var isParticipant = await _context.Chatparticipants
            .AnyAsync(cp => cp.ChatId == chatId && cp.UserId == userId);

        if (!isParticipant)
        {
            return new ChatResult(false, "You are not a participant in this chat.");
        }

        var chatDto = await GetChatDtoAsync(chatId, userId);
        return new ChatResult(true, "Chat retrieved successfully.", chatDto);
    }

    public async Task<ChatResult> EditMessageAsync(int userId, EditMessageDto editMessageDto)
    {
        var message = await _context.Messages
            .Include(m => m.ChatparticipantChatparticipant)
            .FirstOrDefaultAsync(m => m.Id == editMessageDto.MessageId);

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

        // Persistence First: Update in database
        message.Content = editMessageDto.Content;
        message.EditedAt = DateTimeHelper.ToUnspecified(DateTime.UtcNow);

        await _context.SaveChangesAsync();

        var messageDto = new MessageResponseDto
        {
            Id = message.Id,
            Content = message.Content,
            SentAt = message.SentAt,
            EditedAt = message.EditedAt,
            IsDeleted = message.IsDeleted,
            AttachmentUrl = message.AttachmentUrl,
            SenderId = message.ChatparticipantChatparticipant.UserId,
            ChatId = message.ChatparticipantChatparticipant.ChatId
        };

        return new ChatResult(true, "Message edited successfully.", messageDto);
    }

    public async Task<ChatResult> DeleteMessageAsync(int userId, int messageId)
    {
        var message = await _context.Messages
            .Include(m => m.ChatparticipantChatparticipant)
            .FirstOrDefaultAsync(m => m.Id == messageId);

        if (message == null)
        {
            return new ChatResult(false, "Message not found.");
        }

        if (message.ChatparticipantChatparticipant.UserId != userId)
        {
            return new ChatResult(false, "You can only delete your own messages.");
        }

        // Soft delete: Persistence First
        message.IsDeleted = true;
        message.DeletedAt = DateTimeHelper.ToUnspecified(DateTime.UtcNow);

        await _context.SaveChangesAsync();

        return new ChatResult(true, "Message deleted successfully.", new { MessageId = messageId });
    }

    public async Task<ChatResult> MarkChatAsReadAsync(int userId, int chatId)
    {
        var participant = await _context.Chatparticipants
            .FirstOrDefaultAsync(cp => cp.ChatId == chatId && cp.UserId == userId);

        if (participant == null)
        {
            return new ChatResult(false, "You are not a participant in this chat.");
        }

        // Persistence First: Update read status
        participant.LastReadAt = DateTimeHelper.ToUnspecified(DateTime.UtcNow);
        participant.UnreadCount = 0;

        await _context.SaveChangesAsync();

        return new ChatResult(true, "Chat marked as read.", new { ChatId = chatId, UnreadCount = 0 });
    }

    public async Task<ChatResult> JoinChatGroupAsync(int userId, int chatId)
    {
        var chat = await _context.Chats.FindAsync(chatId);
        if (chat == null)
        {
            return new ChatResult(false, "Chat not found.");
        }

        var existingParticipant = await _context.Chatparticipants
            .FirstOrDefaultAsync(cp => cp.ChatId == chatId && cp.UserId == userId);

        if (existingParticipant != null)
        {
            return new ChatResult(true, "Already a participant in this chat.");
        }

        var participant = new Chatparticipant
        {
            ChatId = chatId,
            UserId = userId,
            RoleId = 1, // Default member role
            JoinedAt = DateTimeHelper.ToUnspecified(DateTime.UtcNow),
            UnreadCount = 0
        };

        _context.Chatparticipants.Add(participant);
        await _context.SaveChangesAsync();

        return new ChatResult(true, "Joined chat successfully.");
    }

    public async Task<ChatResult> LeaveChatGroupAsync(int userId, int chatId)
    {
        var participant = await _context.Chatparticipants
            .FirstOrDefaultAsync(cp => cp.ChatId == chatId && cp.UserId == userId);

        if (participant == null)
        {
            return new ChatResult(false, "You are not a participant in this chat.");
        }

        _context.Chatparticipants.Remove(participant);
        await _context.SaveChangesAsync();

        return new ChatResult(true, "Left chat successfully.");
    }

    private async Task<ChatResponseDto?> GetChatDtoAsync(int chatId, int userId)
    {
        var participant = await _context.Chatparticipants
            .Include(cp => cp.Chat)
                .ThenInclude(c => c.Chatparticipants)
                    .ThenInclude(cp => cp.User)
            .FirstOrDefaultAsync(cp => cp.ChatId == chatId && cp.UserId == userId);

        if (participant == null)
        {
            return null;
        }

        var lastMessage = await _context.Messages
            .Include(m => m.ChatparticipantChatparticipant)
                .ThenInclude(cp => cp.User)
            .Where(m => m.ChatparticipantChatparticipant.ChatId == chatId && !m.IsDeleted)
            .OrderByDescending(m => m.SentAt)
            .FirstOrDefaultAsync();

        return new ChatResponseDto
        {
            Id = participant.Chat.Id,
            Type = participant.Chat.Type,
            Name = participant.Chat.Name,
            CreatedAt = participant.Chat.CreatedAt,
            LastMessageAt = participant.Chat.LastMessageAt,
            TeamId = participant.Chat.TeamId,
            UnreadCount = participant.UnreadCount,
            Participants = participant.Chat.Chatparticipants.Select(p => new ChatParticipantDto
            {
                UserId = p.UserId,
                Username = p.User.UserName,
                AvatarUrl = p.User.AvatarUrl,
                RoleId = p.RoleId,
                JoinedAt = p.JoinedAt
            }).ToList(),
            LastMessage = lastMessage != null ? new MessageResponseDto
            {
                Id = lastMessage.Id,
                Content = lastMessage.Content,
                SentAt = lastMessage.SentAt,
                EditedAt = lastMessage.EditedAt,
                IsDeleted = lastMessage.IsDeleted,
                AttachmentUrl = lastMessage.AttachmentUrl,
                SenderId = lastMessage.ChatparticipantChatparticipant.UserId,
                SenderUsername = lastMessage.ChatparticipantChatparticipant.User.UserName,
                SenderAvatarUrl = lastMessage.ChatparticipantChatparticipant.User.AvatarUrl,
                ChatId = chatId
            } : null
        };
    }
}

