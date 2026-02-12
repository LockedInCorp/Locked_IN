using AutoMapper;
using Locked_IN_Backend.DTO;
using Locked_IN_Backend.DTOs;
using Locked_IN_Backend.Interfaces;
using Locked_IN_Backend.Interfaces.Repositories;
using Microsoft.AspNetCore.Authorization;

using Locked_IN_Backend.DTOs.Team;
using Locked_IN_Backend.Data.Entities;
using Locked_IN_Backend.Misc.Enum;
using Locked_IN_Backend.Exceptions;

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
        var team = await _teamRepository.GetTeamWithDetailsByIdAsync(teamId);
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
        // using var transaction = await _teamRepository.BeginTransactionAsync();

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

            await _chatService.CreateTeamChatAsync(creatorId, team.Id, saveChanges: true);
            await _teamRepository.SaveChangesAsync();

            // await transaction.CommitAsync();
            
            var createdTeam = await _teamRepository.GetTeamWithDetailsByIdAsync(team.Id);
            return _mapper.Map<GetTeamDto>(createdTeam);
        }
        catch (Exception)
        {
            // await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<GetTeamDto> UpdateTeamAsync(int teamId, UpdateTeamDto dto, int userId)
    {
        var team = await _teamRepository.GetTeamWithDetailsByIdAsync(teamId);
        if (team == null)
        {
            throw new NotFoundException($"Team with ID {teamId} not found");
        }

        var isLeader = team.TeamMembers.Any(tm => tm.UserId == userId && tm.Isleader);
        if (!isLeader)
        {
            throw new ForbiddenException("Only the team leader can update team details");
        }

        _mapper.Map(dto, team);

        if (dto.PreviewImage != null)
        {
            team.IconUrl = await _fileUploadService.UploadTeamIconAsync(dto.PreviewImage);
        }

        if (dto.Tags != null)
        {
            team.TeamPreferencetagRelations.Clear();
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
            if (team.TeamCommunicationService == null)
            {
                team.TeamCommunicationService = new TeamCommunicationService();
            }

            team.TeamCommunicationService.CommunicationServiceId = dto.CommunicationService.Value;
            team.TeamCommunicationService.Link = dto.CommunicationServiceLink ?? string.Empty;
        }
        else
        {
            team.TeamCommunicationService = null;
        }

        await _teamRepository.UpdateTeam(team);
        await _teamRepository.SaveChangesAsync();

        var updatedTeam = await _teamRepository.GetTeamWithDetailsByIdAsync(teamId);
        return _mapper.Map<GetTeamDto>(updatedTeam);
    }
}