using AutoMapper;
using Locked_IN_Backend.Data.Entities;
using Locked_IN_Backend.DTO;
using Locked_IN_Backend.DTOs;

namespace Locked_IN_Backend.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
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
            .ForMember(dest => dest.TeamLeaderNickname, opt => opt.Ignore()); // Will be set manually in AdvancedSearch
    }
}
