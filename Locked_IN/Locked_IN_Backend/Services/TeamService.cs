using AutoMapper;
using Locked_IN_Backend.DTO;
using Locked_IN_Backend.DTOs;
using Locked_IN_Backend.Interfaces;
using Locked_IN_Backend.Interfaces.Repositories;
using Microsoft.AspNetCore.Authorization;

using Locked_IN_Backend.DTOs.Team;
using Locked_IN_Backend.Data.Entities;
using Locked_IN_Backend.Misc.Enum;

namespace Locked_IN_Backend.Services;

public class TeamService : ITeamService
{
    private readonly ITeamRepository _teamRepository;
    private readonly IChatService _chatService;
    private readonly ITeamMemberRepository _teamMemberRepository;
    private readonly IFileUploadService _fileUploadService;
    private readonly IMapper _mapper;

    public TeamService(ITeamRepository teamRepository, ITeamMemberRepository teamMemberRepository, IMapper mapper, IChatService chatService, IFileUploadService fileUploadService)
    {
        _teamRepository = teamRepository;
        _teamMemberRepository = teamMemberRepository;
        _mapper = mapper;
        _chatService = chatService;
        _fileUploadService = fileUploadService;
    }
    
    public async Task<GetTeamDto?> GetTeamByIdAsync(int teamId)
    {
        var team = await _teamRepository.GetTeamById(teamId);
        return _mapper.Map<GetTeamDto>(team);
    }

    public async Task<List<GetTeamDto>> GetTeamsByGameIdAsync(int gameId)
    {
        var teams = await _teamRepository.GetTeamsByGameIdAsync(gameId);
        return _mapper.Map<List<GetTeamDto>>(teams);
    }

    public async Task<List<GetTeamDto>> GetTeamsByNameSearchAsync(string searchTerm = "")
    {
        var searchResults = await _teamRepository.GetTeamsByNameSearchAsync(searchTerm);
        return searchResults.Select(r =>
        {
            var dto = _mapper.Map<GetTeamDto>(r.Team);
            dto.SearchRank = r.SearchRank;
            return dto;
        }).ToList();
    }

    public async Task<PagedResult<GetTeamsCardDto>> SearchTeamsAdvancedAsync(List<int> gameIds, List<int> preferenceTagIds, string searchTerm, int page, int pageSize, string sortBy, int userId, bool OnlyShowPending)
    {
        var pagedResults = await _teamRepository.GetTeamsAdvancedAsync(gameIds, preferenceTagIds, searchTerm, page, pageSize, sortBy, userId, OnlyShowPending);
        
        
        return new PagedResult<GetTeamsCardDto>
        {
            Items = pagedResults.Items.Select(r =>
            {
                var dto = _mapper.Map<GetTeamsCardDto>(r.Team);
                dto.SearchRank = r.SearchRank;
                dto.TeamLeaderUsername = r.TeamLeaderUsername;
                dto.TeamMemberStatus = r.TeamMemberStatus;
                return dto;
            }).ToList(),
            TotalCount = pagedResults.TotalCount,
            Page = pagedResults.Page,
            PageSize = pagedResults.PageSize,
            TotalPages = pagedResults.TotalPages,
        };
    }

    public async Task<GetTeamDto> CreateTeamAsync(CreateTeamDto dto, int creatorId)
    {
        using var transaction = await _teamRepository.BeginTransactionAsync();

        try
        {
            var team = _mapper.Map<Team>(dto);

            if (dto.PreviewImage != null)
            {
                team.IconUrl = await _fileUploadService.UploadTeamIconAsync(dto.PreviewImage);
            }

            team.TeamMembers.Add(new TeamMember
            {
                UserId = creatorId,
                Isleader = true,
                MemberStatusId = (int)TeamMemberStatus.STATUS_LEADER,
                Jointimestamp = DateTime.UtcNow
            });

            if (dto.Tags is { Count: > 0 })
            {
                foreach (var tagId in dto.Tags)
                {
                    team.TeamPreferencetagRelations.Add(new TeamPreferencetagRelation
                    {
                        PreferenceTagId = tagId
                    });
                }
            }

            if (dto.CommunicationService.HasValue)
            {
                team.TeamCommunicationService = new TeamCommunicationService
                {
                    CommunicationServiceId = dto.CommunicationService.Value,
                    Link = dto.CommunicationServiceLink ?? string.Empty
                };
            }

            await _teamRepository.AddTeam(team);
            await _teamRepository.SaveChangesAsync();

            await _chatService.CreateTeamChatAsync(creatorId, team.Id, saveChanges: false);
            await _teamRepository.SaveChangesAsync();

            await transaction.CommitAsync();
            return _mapper.Map<GetTeamDto>(team);
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}