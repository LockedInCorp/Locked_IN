using AutoMapper;
using Locked_IN_Backend.DTOs.Team;
using Locked_IN_Backend.Data.Entities;
using Locked_IN_Backend.DTO;
using Locked_IN_Backend.DTOs;

using Locked_IN_Backend.DTOs.User;
using Locked_IN_Backend.DTOs.Chat;
using Locked_IN_Backend.Data.Enums;
using System.Text.Json;
using Locked_IN_Backend.DTOs.Friendship;
using Locked_IN_Backend.DTOs.GameProfile;

using Locked_IN_Backend.DTOs.ExperienceTag;
using Locked_IN_Backend.Misc.Enum;

namespace Locked_IN_Backend.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<ExperienceTag, GetExperienceTagDto>();
        CreateMap<PreferenceTag, GetPreferanceTagsDto>();
        CreateMap<Game, GetGameDto>();
        CreateMap<CreateTeamDto, Team>()
            .ForMember(dest => dest.MaxPlayerCount, opt => opt.MapFrom(src => src.MaxMembers))
            .ForMember(dest => dest.ExperienceTagId, opt => opt.MapFrom(src => src.Experience))
            .ForMember(dest => dest.IsAutoaccept, opt => opt.MapFrom(src => src.AutoAccept))
            .ForMember(dest => dest.MinCompScore, opt => opt.MapFrom(src => src.MinCompetitiveScore))
            .ForMember(dest => dest.Isprivate, opt => opt.MapFrom(src => src.IsPrivate)) 
            .ForMember(dest => dest.CreationTimestamp, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.IconUrl, opt => opt.Ignore())
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Chats, opt => opt.Ignore())
            .ForMember(dest => dest.ExperienceTag, opt => opt.Ignore())
            .ForMember(dest => dest.Game, opt => opt.Ignore())
            .ForMember(dest => dest.TeamMembers, opt => opt.Ignore())
            .ForMember(dest => dest.TeamPreferencetagRelations, opt => opt.Ignore())
            .ForMember(dest => dest.TeamCommunicationService, opt => opt.Ignore());

        CreateMap<RegisterDto, User>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Username))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.Availability, opt => opt.MapFrom(src => "{}"))
            .ForMember(dest => dest.AvatarUrl, opt => opt.Ignore());

        CreateMap<User, UserProfileDto>()
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.UserName))
            .ForMember(dest => dest.AvatarURL, opt => opt.MapFrom(src => src.AvatarUrl))
            .ForMember(dest => dest.Availability, opt => opt.MapFrom(src =>
                string.IsNullOrEmpty(src.Availability) 
                    ? new Dictionary<string, List<string>>() 
                    : JsonSerializer.Deserialize<Dictionary<string, List<string>>>(src.Availability, (JsonSerializerOptions?)null)));

        CreateMap<UserProfileDto, User>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Username))
            .ForMember(dest => dest.Availability, opt => opt.MapFrom(src =>
                src.Availability == null 
                    ? "{}" 
                    : JsonSerializer.Serialize(src.Availability, (JsonSerializerOptions?)null)));

        CreateMap<Team, GetTeamDto>()
            .ForMember(dest => dest.Game, opt => opt.MapFrom(src => src.Game))
            .ForMember(dest => dest.IsPrivate, opt => opt.MapFrom(src => src.Isprivate))
            .ForMember(dest => dest.ExperienceLevel, opt => opt.MapFrom(src => src.ExperienceTag))
            .ForMember(dest => dest.CurrentMemberCount,
                opt => opt.MapFrom(src => src.TeamMembers != null ? src.TeamMembers.Count : 0))
            .ForMember(dest => dest.Members, opt => opt.MapFrom(src => src.TeamMembers.Select(tm => tm.User)))
            .ForMember(dest => dest.AutoAccept, opt => opt.MapFrom(src => src.IsAutoaccept))
            .ForMember(dest => dest.PreferenceTags, opt => opt.MapFrom(src =>
                src.TeamPreferencetagRelations.Select(t => t.PreferenceTag)))
            .ForMember(dest => dest.SearchRank, opt => opt.Ignore())
            .ForMember(dest => dest.Leader,
                opt => opt.MapFrom(src => src.TeamMembers.FirstOrDefault(tm => tm.Isleader).User))
            .ForMember(dest => dest.CommunicationService,
                opt => opt.MapFrom(src => src.TeamCommunicationService.CommunicationService))
            .ForMember(dest => dest.CommunicationServiceLink,
                opt => opt.MapFrom(src => src.TeamCommunicationService.Link))
            .ForMember(dest => dest.IconUrl, opt => opt.MapFrom(src => src.IconUrl));

        CreateMap<TeamMember, GetTeamMemberDto>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.IsLeader, opt => opt.MapFrom(src => src.Isleader))
            .ForMember(dest => dest.JoinTimestamp, opt => opt.MapFrom(src => src.Jointimestamp))
            .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User));

        CreateMap<TeamMember, TeamJoinResponceDto>()
            .ForMember(dest => dest.TeamId, opt => opt.MapFrom(src => src.TeamId))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.User != null ? src.User.UserName : string.Empty))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => (TeamMemberStatus)src.MemberStatusId))
            .ForMember(dest => dest.RequestTimestamp, opt => opt.MapFrom(src => src.Jointimestamp))
            .ForMember(dest => dest.AvatarUrl, opt => opt.MapFrom(src => src.User.AvatarUrl));

        CreateMap<User, GetUserForTeamViewDto>()
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.UserName));

        CreateMap<Chat, GetUserChatsDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.ChatName, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.CreationTimestamp, opt => opt.MapFrom(src => src.CreatedAt))
            .ForMember(dest => dest.LastMessageTime, opt => opt.MapFrom(src => src.LastMessageAt));

        CreateMap<Chat, GetChatDetails>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.ChatName, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.ChatIconUrl,
                opt => opt.MapFrom(src => src.Team != null ? src.Team.IconUrl : string.Empty))
            .ForMember(dest => dest.ChatType, opt => opt.MapFrom(src => src.Type))
            .ForMember(dest => dest.TeamId, opt => opt.MapFrom(src => src.TeamId))
            .ForMember(dest => dest.ParticipantCount, opt => opt.MapFrom(src => src.Chatparticipants != null ? src.Chatparticipants.Count : 0));

        CreateMap<Message, GetMessageDto>()
            .ForMember(dest => dest.SenderId, opt => opt.MapFrom(src => src.ChatparticipantChatparticipant.UserId))
            .ForMember(dest => dest.SenderUsername,
                opt => opt.MapFrom(src =>  src.ChatparticipantChatparticipant.User.UserName))
            .ForMember(dest => dest.SenderAvatarUrl,
                opt => opt.MapFrom(src =>(src.ChatparticipantChatparticipant.User.AvatarUrl)))
            .ForMember(dest => dest.ChatId, opt => opt.MapFrom(src => src.ChatparticipantChatparticipant.ChatId ))
            .ForMember(dest => dest.SentAt, opt => opt.MapFrom(src => src.SentAt))
            .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content))
            .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.IsDeleted))
            .ForMember(dest => dest.AttachmentUrl, opt => opt.MapFrom(src => src.AttachmentUrl))
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));
        
        CreateMap<Team, GetTeamsCardDto>()
            .ForMember(dest => dest.Game, opt => opt.MapFrom(src => src.Game))
            .ForMember(dest => dest.IsPrivate, opt => opt.MapFrom(src => src.Isprivate))
            .ForMember(dest => dest.ExperienceLevel, opt => opt.MapFrom(src => src.ExperienceTag))
            .ForMember(dest => dest.CurrentMemberCount, opt => opt.MapFrom(src => src.TeamMembers != null ? src.TeamMembers.Count : 0))
            .ForMember(dest => dest.PreferenceTags, opt => opt.MapFrom(src => 
                src.TeamPreferencetagRelations.Select(t => t.PreferenceTag)))
            .ForMember(dest => dest.SearchRank, opt => opt.Ignore())
            .ForMember(dest => dest.TeamLeaderUsername, opt => opt.Ignore())
            .ForMember(dest => dest.ChatId, opt => opt.MapFrom(src => src.Chats.FirstOrDefault()!.Id));
        
        CreateMap<GameProfile, GameProfileDto>()
            .ForMember(dest => dest.GameName, opt => opt.MapFrom(src => src.Game != null ? src.Game.Name : string.Empty))
            .ForMember(dest => dest.IsFavorite, opt => opt.MapFrom(src => src.Isfavorite))
            .ForMember(dest => dest.Rank, opt => opt.MapFrom(src => src.Rank.ToString()));

        CreateMap<CreateGameProfileDto, GameProfile>()
            .ForMember(dest => dest.Isfavorite, opt => opt.MapFrom(src => src.IsFavorite))
            .ForMember(dest => dest.ExperienceTagId, opt => opt.MapFrom(src => src.ExperienceTagId))
            .ForMember(dest => dest.GameExpId, opt => opt.MapFrom(src => src.GameExpId))
            .ForMember(dest => dest.Rank, opt => opt.MapFrom(src => src.Rank))
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.UserId, opt => opt.Ignore())
            .ForMember(dest => dest.Game, opt => opt.Ignore())
            .ForMember(dest => dest.ExperienceTag, opt => opt.Ignore());

        CreateMap<CommunicationService, CommunicationServiceDto>();

        
        CreateMap<Friendship, PendingFriendshipRequestDto>()
            .ForMember(dest => dest.FriendshipId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.RequesterId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.RequesterUsername, opt => opt.MapFrom(src => src.User.UserName))
            .ForMember(dest => dest.RequesterAvatarUrl, opt => opt.MapFrom(src => src.User.AvatarUrl))
            .ForMember(dest => dest.RequestTimestamp, opt => opt.MapFrom(src => src.RequestTimestamp));
    }
}
