using AutoMapper;
using Locked_IN_Backend.Data.Entities;
using Locked_IN_Backend.DTOs;
using Locked_IN_Backend.DTOs.Chat;
using Locked_IN_Backend.Exceptions;
using Locked_IN_Backend.Hubs;
using Locked_IN_Backend.Interfaces;
using Locked_IN_Backend.Interfaces.Repositories;
using Locked_IN_Backend.Interfaces.Services;
using Locked_IN_Backend.Misc;
using Microsoft.AspNetCore.SignalR;

namespace Locked_IN_Backend.Services;

public class MessageService : IMessageService
{
    private readonly IMessageRepository _messageRepository;
    private readonly IChatParticipantRepository _participantRepository;
    private readonly IChatRepository _chatRepository;
    private readonly IFileUploadService _fileUploadService;
    private readonly IChatHubService _chatHubService;
    private readonly IMapper _mapper;

    public MessageService(
        IMessageRepository messageRepository,
        IChatParticipantRepository participantRepository,
        IChatRepository chatRepository,
        IFileUploadService fileUploadService,
        IChatHubService chatHubService,
        IMapper mapper)
    {
        _messageRepository = messageRepository;
        _participantRepository = participantRepository;
        _chatRepository = chatRepository;
        _fileUploadService = fileUploadService;
        _chatHubService = chatHubService;
        _mapper = mapper;
    }

    public async Task<GetMessageDto> SendMessageAsync(int userId, SendMessageDto sendMessageDto)
    {
        var participant = await _participantRepository.GetParticipantAsync(sendMessageDto.ChatId, userId);
        if (participant == null)
        {
            throw new ForbiddenException("You are not a participant in this chat.");
        }

        var attachmentUrl = "";
        if (sendMessageDto.AttachmentFile != null)
        {
            attachmentUrl = await _fileUploadService.UploadAttachmentAsync(sendMessageDto.AttachmentFile);
        }
        
        var currentTime = DateTimeHelper.ToUnspecified(DateTime.UtcNow);
        var message = new Message
        {
            Content = sendMessageDto.Content,
            SentAt = currentTime,
            AttachmentUrl = attachmentUrl,
            ChatparticipantChatparticipantId = participant.ChatparticipantId,
            IsDeleted = false
        };
    
        await _messageRepository.AddMessageAsync(message);
        await _messageRepository.SaveChangesAsync();
    
        participant.Chat.LastMessageAt = currentTime;
        await _chatRepository.UpdateChatAsync(participant.Chat);
        await _chatRepository.SaveChangesAsync();
    
        var otherParticipants = await _participantRepository.GetOtherParticipantsAsync(sendMessageDto.ChatId, userId);
        foreach (var otherParticipant in otherParticipants)
        {
            otherParticipant.UnreadCount++;
            await _participantRepository.UpdateParticipantAsync(otherParticipant);
        }
        await _participantRepository.SaveChangesAsync();
    
        var messageDto = _mapper.Map<GetMessageDto>(message);
        
        await _chatHubService.SendMessageToGroupAsync(sendMessageDto.ChatId, messageDto);

        return messageDto;
    }

    public async Task<PagedResult<GetMessageDto>> GetChatMessagesAsync(int userId, int chatId, int pageNumber = 1, int pageSize = 50)
    {
        var participant = await _participantRepository.GetParticipantAsync(chatId, userId);
        if (participant == null)
        {
            throw new ForbiddenException("You are not a participant in this chat.");
        }
    
        var pagedMessages = await _messageRepository.GetChatMessagesAsync(chatId, pageNumber, pageSize);
        
        return new PagedResult<GetMessageDto>
        {
            Items = _mapper.Map<List<GetMessageDto>>(pagedMessages.Items.OrderBy(m => m.SentAt)),
            TotalCount = pagedMessages.TotalCount,
            Page = pagedMessages.Page,
            PageSize = pagedMessages.PageSize,
            TotalPages = pagedMessages.TotalPages
        };
    }

    public async Task<GetMessageDto> EditMessageAsync(int userId, EditMessageDto editMessageDto)
    {
        var message = await _messageRepository.GetMessageByIdAsync(editMessageDto.MessageId);
    
        if (message == null)
        {
            throw new NotFoundException($"Message with id {editMessageDto.MessageId} not found.");
        }
    
        if (message.ChatparticipantChatparticipant.UserId != userId)
        {
            throw new ForbiddenException("You can only edit your own messages.");
        }
    
        if (message.IsDeleted)
        {
            throw new ConflictException("Message is already deleted.");
        }
    
        message.Content = editMessageDto.Content;
        message.EditedAt = DateTimeHelper.ToUnspecified(DateTime.UtcNow);
    
        await _messageRepository.UpdateMessageAsync(message);
        await _messageRepository.SaveChangesAsync();
    
        var messageDto = _mapper.Map<GetMessageDto>(message);

        await _chatHubService.SendEditedMessageToGroupAsync(message.ChatparticipantChatparticipant.ChatId, messageDto);

        return messageDto;
    }

    public async Task DeleteMessageAsync(int userId, int messageId)
    {
        var message = await _messageRepository.GetMessageByIdAsync(messageId);
    
        if (message == null)
        {
            throw new NotFoundException($"Message with id {messageId} not found.");
        }
    
        if (message.ChatparticipantChatparticipant.UserId != userId)
        {
            throw new ForbiddenException("You can only delete your own messages.");
        }
    
        if (message.IsDeleted)
        {
            throw new ConflictException("Message is already deleted.");
        }
    
        if (!string.IsNullOrEmpty(message.AttachmentUrl))
        {
            var parts = message.AttachmentUrl.Split("/");
            if (parts.Length >= 2)
            {
                var bucket = parts[0];
                var fileName = parts[1];
                await _fileUploadService.DeleteFileAsync(bucket, fileName);
            }
        }
        message.IsDeleted = true;
        message.DeletedAt = DateTimeHelper.ToUnspecified(DateTime.UtcNow);
    
        await _messageRepository.UpdateMessageAsync(message);
        await _messageRepository.SaveChangesAsync();

        await _chatHubService.SendDeletedMessageToGroupAsync(message.ChatparticipantChatparticipant.ChatId, messageId);
    }
}
