using AutoMapper;
using Locked_IN_Backend.Data.Entities;
using Locked_IN_Backend.DTO;
using Locked_IN_Backend.DTOs;

using Locked_IN_Backend.DTOs.User;
using Locked_IN_Backend.DTOs.Chat;
using Locked_IN_Backend.Data.Enums;
using System.Text.Json;

namespace Locked_IN_Backend.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
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
            .ForMember(dest => dest.GameName, opt => opt.MapFrom(src => src.Game.Name))
            .ForMember(dest => dest.IsPrivate, opt => opt.MapFrom(src => src.Isprivate))
            .ForMember(dest => dest.IsBlitz, opt => opt.MapFrom(src => src.Isblitz))
            .ForMember(dest => dest.ExperienceLevel, opt => opt.MapFrom(src => src.ExperienceTag.Experiencelevel))
            .ForMember(dest => dest.CurrentMemberCount, opt => opt.MapFrom(src => src.TeamMembers.Count))
            .ForMember(dest => dest.Members, opt => opt.MapFrom(src => src.TeamMembers))
            .ForMember(dest => dest.PreferenceTags, opt => opt.MapFrom(src => 
                src.TeamPreferencetagRelations
                    .Select(tpr => tpr.PreferenceTag.Name)
                    .Where(name => !string.IsNullOrEmpty(name))
                    .ToList()))
            .ForMember(dest => dest.SearchRank, opt => opt.Ignore());

        CreateMap<TeamMember, GetTeamMemberDto>()
            .ForMember(dest => dest.IsLeader, opt => opt.MapFrom(src => src.Isleader))
            .ForMember(dest => dest.JoinTimestamp, opt => opt.MapFrom(src => src.Jointimestamp))
            .ForMember(dest => dest.MemberStatusName, opt => opt.MapFrom(src => src.MemberStatus.Statusname))
            .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User));

        CreateMap<User, GetUserForTeamViewDto>();

        CreateMap<Chat, ChatResponseDto>()
            .ForMember(dest => dest.Participants, opt => opt.MapFrom(src => src.Chatparticipants));

        CreateMap<Chatparticipant, ChatParticipantDto>()
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.User.UserName))
            .ForMember(dest => dest.AvatarUrl, opt => opt.MapFrom(src => src.User.AvatarUrl));

        CreateMap<Message, MessageResponseDto>()
            .ForMember(dest => dest.SenderId, opt => opt.MapFrom(src => src.ChatparticipantChatparticipant.UserId))
            .ForMember(dest => dest.SenderUsername, opt => opt.MapFrom(src => src.ChatparticipantChatparticipant.User.UserName))
            .ForMember(dest => dest.SenderAvatarUrl, opt => opt.MapFrom(src => src.ChatparticipantChatparticipant.User.AvatarUrl))
            .ForMember(dest => dest.ChatId, opt => opt.MapFrom(src => src.ChatparticipantChatparticipant.ChatId));

        CreateMap<Team, GetTeamsCardDto>()
            .ForMember(dest => dest.GameName, opt => opt.MapFrom(src => src.Game.Name))
            .ForMember(dest => dest.IsPrivate, opt => opt.MapFrom(src => src.Isprivate))
            .ForMember(dest => dest.IsBlitz, opt => opt.MapFrom(src => src.Isblitz))
            .ForMember(dest => dest.ExperienceLevel, opt => opt.MapFrom(src => src.ExperienceTag.Experiencelevel))
            .ForMember(dest => dest.CurrentMemberCount, opt => opt.MapFrom(src => src.TeamMembers.Count))
            .ForMember(dest => dest.PreferenceTags, opt => opt.MapFrom(src => 
                src.TeamPreferencetagRelations
                    .Select(tpr => tpr.PreferenceTag.Name)
                    .Where(name => !string.IsNullOrEmpty(name))
                    .ToList()))
            .ForMember(dest => dest.SearchRank, opt => opt.Ignore())
            .ForMember(dest => dest.TeamLeaderUsername, opt => opt.Ignore());
    }
}
